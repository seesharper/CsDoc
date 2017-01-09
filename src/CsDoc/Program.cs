using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsDoc
{
    using System.IO;
    using Microsoft.Extensions.CommandLineUtils;
    using Rendering;

    class Program
    {
        static void Main(string[] args)
        {
            var commandLineApplication = new CommandLineApplication(false)
            {
                Name = "CsDoc",
                Description = "Builds documentation from script files."
            };
            commandLineApplication.HelpOption("-? | -h | --help");

            var sourceArgument = commandLineApplication.Argument("source", "The script file to be processed");
            var outArgument = commandLineApplication.Argument("out", "The output file");

            var defaultText = commandLineApplication.Option("-d|--default", "The default text to be used when no documentation is found",CommandOptionType.SingleValue);

           

            commandLineApplication.OnExecute(() =>
            {
                

                var sourceFile = sourceArgument.Value;
                var outFile = outArgument.Value;

                if (sourceFile == null || !File.Exists(sourceFile))
                {
                    Console.WriteLine($"{sourceFile} not found");
                    commandLineApplication.ShowHelp();
                    return 1;
                }

                if (outFile == null)
                {
                    commandLineApplication.ShowHelp();
                    return 1;
                }


                if (defaultText.HasValue())
                {
                    Options.DefaultText = defaultText.Value();
                }        
                
                GenerateDocumentation(sourceFile, outFile);        
                return 0;
            });


            commandLineApplication.Execute(args);          
        }


        private static void GenerateDocumentation(string sourceFile, string outFile)
        {
            var renderer = new MarkdownRenderer();
            var generator = new Generator(renderer);
            var doc = generator.Generate(sourceFile);
            File.WriteAllText(outFile, renderer.RenderDocumentation(doc));
            Console.WriteLine("Done!");
        }
    }
}
