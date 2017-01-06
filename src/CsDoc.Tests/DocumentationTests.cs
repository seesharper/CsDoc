using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsDoc.Tests
{    
    using Model;
    using Moq;
    using Rendering;
    using Shouldly;

    public class DocumentationTests
    {
        public void ShouldGenerateSummary()
        {
            GetMethodDoc("Summary").Summary.ShouldBe("This is the summary.");            
        }

        public void ShouldGenerateRemarks()
        {
            GetMethodDoc("Remarks").Remarks.ShouldBe("This is the remarks.");
        }

        public void ShouldGenerateExample()
        {
            GetMethodDoc("Example").Examples.Single().Description.ShouldBe("This is the example.");
        }

        public void ShouldGenerateExampleWithCode()
        {
            var doc = GetMethodDoc("ExampleWithCode");

            doc.Examples.Single().Description.ShouldBe("This is the example.");
            doc.Examples.Single().Code.ShouldBe("public class Foo\r\n{\r\n}");
        }

        public void ShouldGenerateExampleWithCData()
        {
            var doc = GetMethodDoc("ExampleWithCData");
            doc.Examples.Single().Code.ShouldBe("public class Foo<T>\r\n{\r\n}");
        }


        public void ShouldGenerateException()
        {
            var doc = GetMethodDoc("Exception");
            doc.Exceptions.Single().Type.ShouldBe("InvalidOperationException");
            doc.Exceptions.Single().ThrownWhen.ShouldBe("This is the exception.");
        }

        public void ShouldGenerateParam()
        {
            var doc = GetMethodDoc("Param");
            doc.Parameters.Single().Name.ShouldBe("value");
            doc.Parameters.Single().Description.ShouldBe("This is the param.");
        }

        public void ShouldGenerateParamWithDefaultValue()
        {
            var doc = GetMethodDoc("ParamWithDefaultValue");

            doc.Parameters.Single().Name.ShouldBe("value");
            doc.Parameters.Single().Description.ShouldBe("This is the param.");
            doc.Parameters.Single().DefaultValue.ShouldBe("42");
        }

        public void ShouldGenerateGenericTypeParameter()
        {
            var doc = GetMethodDoc("TypeParameter");

            doc.TypeParameters.Single().Name.ShouldBe("T");
            doc.TypeParameters.Single().Description.ShouldBe("This is the generic type parameter.");
        }

        public void ShouldGenerateReturns()
        {
            var doc = GetMethodDoc("Returns");

            doc.ReturnType.Name.ShouldBe("int");
            doc.ReturnType.Description.ShouldBe("This is the return value.");
        }


        public void ShouldRenderBulletList()
        {
            Mock<IRenderer> rendererMock = new Mock<IRenderer>();
            var generator = new Generator(rendererMock.Object);
            generator.Generate("SampleScript.csx");

            rendererMock.Verify(r => r.RenderList(It.Is<ListDocInfo>(info => info.Type == "bullet")), Times.Once);
            rendererMock.Verify(r => r.RenderList(It.Is<ListDocInfo>(info => info.Header.Term == "This is the header term.")), Times.Once);
            rendererMock.Verify(r => r.RenderList(It.Is<ListDocInfo>(info => info.Header.Description == "This is the header description.")), Times.Once);
            rendererMock.Verify(r => r.RenderList(It.Is<ListDocInfo>(info => info.Items.First().Term == "This is the first term.")), Times.Once);
            rendererMock.Verify(r => r.RenderList(It.Is<ListDocInfo>(info => info.Items.First().Description == "This is the first item.")), Times.Once);
        }


        private static MethodDocInfo GetMethodDoc(string name)
        {
            var generator = new Generator(new MarkdownRenderer());
            var doc = generator.Generate("SampleScript.csx");
            return doc.Classes.Single(c => c.Name == "Sample").Methods.Single(m => m.Name == name);
        }
    }
}
