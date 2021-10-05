using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using Octostache;
using YamlDotNet.RepresentationModel;

namespace OctostacheCmd
{
    class Program
    {
        static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Option<string>("--variable-file",
                    () => string.Empty,
                    "An optional file containing variables to use in the substitution."),
                new Option<string>("--template-files",
                    "A comma separated list of files that contains the octo tokens needing to be substituted."),
                new Option<string>("--output-files",
                    () => string.Empty,
                    "An optional comma separated list of files to output. If defined, the program assumes the index of the output file is the same as the index of the template file in the template-files list. They therefore need to have the same number of elements.")
            };

            rootCommand.Description =
                "This program takes in a list of template files, an option variable file, and an optional list of output files and performs variable substitution in the files using Octostache.";

            rootCommand.Handler = CommandHandler.Create<string, string, string>(processFile);

            return rootCommand.InvokeAsync(args).Result;
        }

        static int processFile(string variableFile, string templateFiles, string outputFiles)
        {
            if (string.IsNullOrEmpty(templateFiles))
            {
                Console.WriteLine("The template-files argument needs a value.");
                return 1;
            }

            Console.WriteLine("Action Arguments");
            if (!string.IsNullOrEmpty(variableFile))
            {
                Console.WriteLine(variableFile);
            }
            if (!string.IsNullOrEmpty(templateFiles))
            {
                Console.WriteLine(templateFiles);
            }
            if (!string.IsNullOrEmpty(outputFiles))
            {
                Console.WriteLine(outputFiles);
            }

            var templateFilesList = templateFiles.Split(',').Select(file => file.Trim()).ToList();
            var outputFilesList = outputFiles.Split(',').Select(file => file.Trim()).ToList();

            if (outputFilesList.Any() && outputFilesList.Count != templateFilesList.Count)
            {
                Console.WriteLine("The number of output files must match the number of template files. Exiting.");
                return 1;
            }


            var variableFileString = File.ReadAllText(variableFile);
            var resultYamlDictionary = new YamlDotNet.Serialization.Deserializer().Deserialize<Dictionary<string, string>>(variableFileString);

            var varDictionary = new VariableDictionary();

            Console.WriteLine($"{Environment.NewLine}Variables to use in octostache replacement found in {variableFile}:{Environment.NewLine}");
            resultYamlDictionary.ForEach(entry =>
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
                varDictionary.Add(entry.Key, entry.Value);
            });


            Console.WriteLine($"{Environment.NewLine}Environment variables to use in octostache replacement:{Environment.NewLine}");
            EnvironmentVariableRetriever.GetAllVariables()
                .ForEach(entry =>
                {
                    Console.WriteLine($"{entry.Key}: {entry.Value}");
                    varDictionary.Add(entry.Key, entry.Value);
                });

            for (var i = 0; i < templateFilesList.Count(); i++)
            {
                var outputFile = outputFilesList.Any() ? outputFilesList[i] : templateFilesList[i];
                new FileSubstitution(templateFilesList[i], outputFile).DoSubstitutions(varDictionary);
            }
            return 0;
        }
    }
}
