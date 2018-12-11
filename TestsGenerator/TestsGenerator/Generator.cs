using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TestsGenerator
{
    public class Generator
    {
        int maxReading;
        int maxWriting;
        int maxProcessing;

        public Generator(int _maxReading, int _maxWriting, int _maxProcessing)
        {
            maxReading = _maxReading;
            maxWriting = _maxWriting;
            maxProcessing = _maxProcessing;
        }

        public async Task Generate(List<string> inputFiles, string outputPath)
        {
            var linkOptions = new DataflowLinkOptions();
            linkOptions.PropagateCompletion = true;
            var readOptions = new ExecutionDataflowBlockOptions();
            readOptions.MaxDegreeOfParallelism = maxReading;
            var processOptions = new ExecutionDataflowBlockOptions();
            processOptions.MaxDegreeOfParallelism = maxProcessing;
            var writeOptions = new ExecutionDataflowBlockOptions();
            writeOptions.MaxDegreeOfParallelism = maxWriting;

            var readBlock = new TransformBlock<string, string>(fileName => Reader.Read(fileName), readOptions);
            var processBlock = new TransformBlock<string, List<Test>>(src => GenerateTests(src), processOptions);
            var writeBlock = new ActionBlock<List<Test>>(output => Writer.Write(outputPath, output).Wait(), writeOptions);

            readBlock.LinkTo(processBlock, linkOptions);
            processBlock.LinkTo(writeBlock, linkOptions);

            foreach (string file in inputFiles)
            {
                readBlock.Post(file);
            }
            readBlock.Complete();
            await writeBlock.Completion;
        }

        public List<Test> CreateTests(List<CInfo> pClasses)
        {
            string content, fileName;
            var res = new List<Test>();

            foreach (CInfo cInfo in pClasses)
            {
                CompilationUnitSyntax cus = CompilationUnit()
                    .WithUsings(GetUsings(cInfo))
                    .WithMembers(SingletonList<MemberDeclarationSyntax>(GetNamespaceDecl(cInfo)
                        .WithMembers(SingletonList<MemberDeclarationSyntax>(ClassDeclaration(cInfo.Name + "Tests")
                            .WithAttributeLists(
                                SingletonList<AttributeListSyntax>(
                                    AttributeList(
                                        SingletonSeparatedList<AttributeSyntax>(
                                            Attribute(
                                                IdentifierName("TestClass"))))))
                                                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                                        .WithMembers(GetMembers(cInfo))))));

                fileName = cInfo.Name + "Test.cs";
                content = cus.NormalizeWhitespace().ToFullString();
                res.Add(new Test(fileName, content));
            }

            return res;
        }

        public List<Test> GenerateTests(string src)
        {
            var codeParcer = new CodeParcer();
            List<CInfo> res = codeParcer.Parce(src);
            List<Test> tests = CreateTests(res);
            return tests;
        }

        private NamespaceDeclarationSyntax GetNamespaceDecl(CInfo cInfo)
        {
            NamespaceDeclarationSyntax namespaceDecl = NamespaceDeclaration(QualifiedName(
                IdentifierName(cInfo.Namespace), IdentifierName("Tests")));
            return namespaceDecl;
        }

        private SyntaxList<UsingDirectiveSyntax> GetUsings(CInfo cInfo)
        {
            var usings = new List<UsingDirectiveSyntax>()
            {
                UsingDirective(IdentifierName("System")),
                UsingDirective(IdentifierName("System.Collections.Generic")),
                UsingDirective(IdentifierName("System.Linq")),
                UsingDirective(IdentifierName("System.Text")),
                UsingDirective(IdentifierName("Microsoft.VisualStudio.TestTools.UnitTesting")),
                UsingDirective(IdentifierName(cInfo.Namespace))
            };

            return new SyntaxList<UsingDirectiveSyntax>(usings);
        }

        private SyntaxList<MemberDeclarationSyntax> GetMembers(CInfo cInfo)
        {
            var methods = new List<MemberDeclarationSyntax>();

            foreach (string mName in cInfo.Methods)
            {
                methods.Add(GetMethodDec(mName));
            }

            return new SyntaxList<MemberDeclarationSyntax>(methods);
        }

        private MethodDeclarationSyntax GetMethodDec(string method)
        {
            MethodDeclarationSyntax methDecl;
            var Members = new List<StatementSyntax>();

            Members.Add(
                ExpressionStatement(
                    InvocationExpression(
                        GetAssertFail())
                    .WithArgumentList(GetMemberArgs())));

            methDecl = MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.VoidKeyword)),
                Identifier(method + "Test"))
                .WithAttributeLists(
                    SingletonList<AttributeListSyntax>(
                        AttributeList(
                            SingletonSeparatedList<AttributeSyntax>(
                                Attribute(
                                    IdentifierName("TestMethod"))))))
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithBody(Block(Members));

            return methDecl;
        }

        private MemberAccessExpressionSyntax GetAssertFail()
        {
            MemberAccessExpressionSyntax fail = MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("Assert"),
                IdentifierName("Fail"));
            return fail;
        }

        private ArgumentListSyntax GetMemberArgs()
        {
            ArgumentListSyntax args = ArgumentList(
                SingletonSeparatedList(
                    Argument(
                        LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            Literal("autogenerated")))));
            return args;
        }
    }
}