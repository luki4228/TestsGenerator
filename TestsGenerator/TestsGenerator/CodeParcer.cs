using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TestsGenerator
{
    public class CodeParcer
    {
        public List<CInfo> Parce(string src)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(src);
            CompilationUnitSyntax compilationUnit = syntaxTree.GetCompilationUnitRoot();
            return GetClasses(compilationUnit);
        }

        private List<CInfo> GetClasses(CompilationUnitSyntax compilationUnit)
        {
            string classNamespace, className;
            var classes = new List<CInfo>();

            foreach (ClassDeclarationSyntax classDecl in compilationUnit.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                classNamespace = ((NamespaceDeclarationSyntax)classDecl.Parent).Name.ToString();
                className = classDecl.Identifier.ValueText;
                classes.Add(new CInfo(className, classNamespace, GetMethods(classDecl)));
            }

            return classes;
        }

        private List<string> GetMethods(ClassDeclarationSyntax classDecl)
        {
            string methodName;
            var classMethods = new List<string>();

            foreach (MethodDeclarationSyntax methodDecl in classDecl.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where(methodDecl => methodDecl.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword))))
            {
                methodName = methodDecl.Identifier.ValueText;
                classMethods.Add(methodName);
            }

            return classMethods;
        }
    }

}