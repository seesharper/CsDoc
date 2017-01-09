namespace CsDoc.Rendering
{
    using System.Text;
    using Model;

    public class MarkdownRenderer : IRenderer
    {
        public string RenderDocumentation(Documentation documentation)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var methodDocInfo in documentation.Methods)
            {
                RenderMethod(methodDocInfo, sb);
            }

            foreach (var classDocInfo in documentation.Classes)
            {
                sb.AppendLine($"## {classDocInfo.Name}");
                sb.AppendLine("---");
                sb.AppendLine(classDocInfo.Summary);
                RenderTypeParameters(classDocInfo.TypeParameters.ToArray(), sb);
                sb.AppendLine();
                RenderExamples(classDocInfo.Examples.ToArray(), sb);
                RenderMethods(classDocInfo.Methods.ToArray(), sb);
                RenderProperties(classDocInfo.Properties.ToArray(), sb);

            }

            return sb.ToString();
        }


        private void RenderExamples(ExampleDocInfo[] examples, StringBuilder sb)
        {
            if (examples.Length > 0)
            {
                foreach (var example in examples)
                {
                    sb.AppendLine("Example");
                    sb.AppendLine();
                    sb.AppendLine(example.Description);
                    sb.AppendLine();
                    sb.AppendLine("```csharp");
                    sb.AppendLine(example.Code);
                    sb.AppendLine("```");
                }
            }
        }

        private void RenderProperties(PropertyDocInfo[] properties, StringBuilder sb)
        {
            if (properties.Length == 0)
            {
                return;
            }
            sb.AppendLine("## Properties");
            foreach (var property in properties)
            {
                RenderProperty(property, sb);
            }
        }

        private void RenderProperty(PropertyDocInfo property, StringBuilder sb)
        {
            sb.AppendLine($"### {property.Type} {property.Name} {property.Accessors}");

            sb.AppendLine(property.Summary);
            sb.AppendLine();
        }

        private void RenderMethods(MethodDocInfo[] methods, StringBuilder sb)
        {
            if (methods.Length == 0)
            {
                return;
            }
            sb.AppendLine("## Methods");
            foreach (var method in methods)
            {
                RenderMethod(method, sb);
            }
        }


        private void RenderMethod(MethodDocInfo method, StringBuilder sb)
        {
            sb.AppendLine($"### {method.Name}");
            sb.AppendLine(method.Summary);
            if (method.Parameters.Length > 0)
            {
                RenderParameters(method.Parameters, sb);
            }
            sb.AppendLine();
            sb.AppendLine("**Return Value**");
            sb.AppendLine($"Type: {method.ReturnType.Name}");
            if (method.ReturnType.Name != "void")
            {
                sb.AppendLine(method.ReturnType.Description);
            }
            sb.AppendLine();
        }

        private void RenderTypeParameters(TypeDocInfo[] typeParameters, StringBuilder sb)
        {
            if (typeParameters.Length == 0)
            {
                return;
            }
            sb.AppendLine();
            sb.AppendLine("**Type Parameters**");
            sb.AppendLine();
            sb.AppendLine("| Name | Description |");
            sb.AppendLine("| ---- | ----------- |");
            foreach (var parameter in typeParameters)
            {
                sb.AppendLine($"| {parameter.Name} | {parameter.Description} |");
            }
        }



        private void RenderParameters(ParameterDocInfo[] parameters, StringBuilder sb)
        {
            if (parameters.Length == 0)
            {
                return;
            }
            sb.AppendLine();
            sb.AppendLine("**Parameters**");
            sb.AppendLine();
            sb.AppendLine("| Name | Type | Default | Description |");
            sb.AppendLine("| ---- | ---- | ------- | ----------- |");
            foreach (var parameter in parameters)
            {
                sb.AppendLine($"| {parameter.Name} | {parameter.Type} | {parameter.DefaultValue} | {parameter.Description} |");
            }
        }

        public string RenderParamRef(string reference)
        {
            return $"**{reference}**";
        }

        public string RenderList(ListDocInfo listDocInfo)
        {
            //throw new System.NotImplementedException();
            return string.Empty;
        }
    }
}