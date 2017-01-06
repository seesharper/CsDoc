namespace CsDoc
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Model;
    using Rendering;
    using Walkers;

    /// <summary>
    /// 
    /// </summary>
    public class Generator
    {
        private readonly IRenderer renderer;

        public Generator(IRenderer renderer)
        {
            this.renderer = renderer;
        }


        public Documentation Generate(string pathToCompilationUnit)
        {
            Documentation documentation = new Documentation();

            var root = GetRootSyntaxNode(pathToCompilationUnit);
            var rootMethods = root.Find<MethodDeclarationSyntax>(m => m.Parent is CompilationUnitSyntax).ToArray();
            foreach (var rootMethod in rootMethods)
            {
                documentation.Methods.Add(ProcessMethod(rootMethod));
            }

            var classes = root.Find<ClassDeclarationSyntax>(c => c.Modifiers.Select(m => m.Text).Contains("public"));
            foreach (var @class in classes)
            {
                documentation.Classes.Add(ProcessClass(@class));
            }

            return documentation;
        }

        private static CSharpSyntaxNode GetRootSyntaxNode(string pathToScriptFile)
        {
            var script = CSharpScript.Create(File.ReadAllText(pathToScriptFile));
            var compilation = script.GetCompilation();
            var syntaxTree = (CSharpSyntaxTree)compilation.SyntaxTrees.First();            
            var root = syntaxTree.GetRoot();
            return root;
        }

        private ClassDocInfo ProcessClass(ClassDeclarationSyntax classDeclaration)
        {
            ClassDocInfo classDocInfo = new ClassDocInfo();
            classDocInfo.Name = classDeclaration.Identifier.Text;
            classDocInfo.Modifies = classDeclaration.Modifiers.Select(m => m.Text).ToArray();
            classDocInfo.BaseTypes = classDocInfo.BaseTypes;
            classDocInfo.TypeParameters.AddRange(GetTypeParameters(classDeclaration.TypeParameterList?.Parameters));
            var documentationTrivia = classDeclaration.GetLeadingTrivia().SingleOrDefault(t => t.HasStructure && !t.IsDirective).GetStructure();
            var walker = new DocumentationWalker<MemberDocInfo>(renderer);
            walker.ApplyDocumentation(documentationTrivia, classDocInfo);
            var methods = classDeclaration.Find<MethodDeclarationSyntax>(m => m.Modifiers.IsPublic()).ToArray();
            foreach (var methodDeclarationSyntax in methods)
            {
                classDocInfo.Methods.Add(ProcessMethod(methodDeclarationSyntax));
            }
            var properties = classDeclaration.Find<PropertyDeclarationSyntax>(p => true);
            {
                foreach (var propertyDeclarationSyntax in properties)
                {
                    classDocInfo.Properties.Add(ProcessProperty(propertyDeclarationSyntax));
                }
            }
            return classDocInfo;
        }

        private ParameterDocInfo[] GetParameters(IEnumerable<ParameterSyntax> parameters)
        {
            List<ParameterDocInfo> result = new List<ParameterDocInfo>();
            foreach (var parameter in parameters)
            {
                var parameterDocInfo = new ParameterDocInfo();
                parameterDocInfo.Name = parameter.Identifier.Text;
                parameterDocInfo.DefaultValue = parameter.Default?.Value.ToString();
                parameterDocInfo.Type = parameter.Type.ToString();
                parameterDocInfo.Description = "N/A";
                result.Add(parameterDocInfo);
            }
            return result.ToArray();
        }

        private TypeDocInfo[] GetTypeParameters(IEnumerable<TypeParameterSyntax> typeParameters)
        {
            List<TypeDocInfo> result = new List<TypeDocInfo>();
            if (typeParameters != null)
            {
                foreach (var typeParameter in typeParameters)
                {
                    var parameterDocInfo = new TypeDocInfo();
                    parameterDocInfo.Name = typeParameter.Identifier.Text;
                    parameterDocInfo.Description = "N/A";
                    result.Add(parameterDocInfo);
                }
            }

            return result.ToArray();
        }



        private MethodDocInfo ProcessMethod(MethodDeclarationSyntax method)
        {

            var documentationTrivia = method.GetLeadingTrivia().SingleOrDefault(t => t.HasStructure && !t.IsDirective).GetStructure();
            var methodDocInfo = new MethodDocInfo();
            methodDocInfo.Parameters = GetParameters(method.ParameterList.Parameters.ToArray());
            methodDocInfo.TypeParameters.AddRange(GetTypeParameters(method.Find<TypeParameterSyntax>(t => true)).ToArray());
            methodDocInfo.ReturnType.Name = method.ReturnType.ToString();
            methodDocInfo.Name = method.Identifier.Text;
            var walker = new MethodDocumentationWalker(renderer);
            walker.ApplyDocumentation(documentationTrivia, methodDocInfo);
            return methodDocInfo;
        }


        private PropertyDocInfo ProcessProperty(PropertyDeclarationSyntax property)
        {
            var documentationTrivia = property.GetLeadingTrivia().SingleOrDefault(t => t.HasStructure && !t.IsDirective).GetStructure();
            var publicAccessors =
                property.AccessorList?.Accessors.Where(a => a.Modifiers.IsPublic()).Select(a => a.Keyword.Text);
            var accessorString = publicAccessors?.Aggregate((current, next) => current + $";{next}");
            PropertyDocInfo propertyDocInfo = new PropertyDocInfo();
            propertyDocInfo.Name = property.Identifier.Text;
            propertyDocInfo.Type = property.Type.ToString();
            propertyDocInfo.Accessors = accessorString;
            var walker = new DocumentationWalker<PropertyDocInfo>(renderer);
            walker.ApplyDocumentation(documentationTrivia, propertyDocInfo);
            return propertyDocInfo;
        }
    }
}