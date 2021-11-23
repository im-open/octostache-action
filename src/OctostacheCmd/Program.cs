using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using Octostache;

namespace OctostacheCmd
{
    class Program
    {
        static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Option<string>("--variables-file",
                    () => string.Empty,
                    "An optional file containing variables to use in the substitution."),
                new Option<string>("--files-with-substitutions",
                    "A comma separated list of files with #{variables} that need substitution.")
            };

            rootCommand.Description =
                "This program takes in a list of files with substitutions, an optional variables file, and an optional list of output files and performs variable substitution in the files using Octostache.";


            rootCommand.Handler = CommandHandler.Create<string, string>(processFile);

            return rootCommand.InvokeAsync(args).Result;
        }

        static int processFile(string variablesFile, string filesWithSubstitutions)
        {
            if (string.IsNullOrEmpty(filesWithSubstitutions))
            {
                Console.WriteLine("The template-files argument needs a value.");
                return 1;
            }

            Console.WriteLine("Action Arguments");
            if (!string.IsNullOrEmpty(variablesFile))
            {
                Console.WriteLine(variablesFile);
            }
            if (!string.IsNullOrEmpty(filesWithSubstitutions))
            {
                Console.WriteLine(filesWithSubstitutions);
            }

            var templateFilesList = FilesRetriever.GetFiles(filesWithSubstitutions).ToList();

            var varDictionary = new VariableDictionary();

            if (!string.IsNullOrEmpty(variablesFile))
            {
                var variableFileString = File.ReadAllText(variablesFile);
                var resultYamlDictionary = new YamlDotNet.Serialization.Deserializer().Deserialize<Dictionary<string, string>>(variableFileString);

                Console.WriteLine($"{Environment.NewLine}Variables to use in octostache replacement found in {variablesFile}:{Environment.NewLine}");
                resultYamlDictionary.ForEach(entry =>
                {
                    Console.WriteLine($"{entry.Key}: {entry.Value}");
                    varDictionary.Add(entry.Key, entry.Value);
                });
            }

            Console.WriteLine($"{Environment.NewLine}Environment variables to use in octostache replacement:{Environment.NewLine}");
            EnvironmentVariableRetriever.GetAllVariables()
                .ForEach(entry =>
                {
                    Console.WriteLine($"{entry.Key}: {entry.Value}");
                    varDictionary.Add(entry.Key, entry.Value);
                });

            templateFilesList.ForEach(file => FileSubstitution.DoOctostacheSubstitutions(file, varDictionary));

            return 0;
        }
    }
}
