using System;
using Octostache;

namespace OctostacheAction
{
    class Program
    {
        static int Main(string[] args)
        {
            if(args.Length < 2){
                Console.WriteLine("Usage: <variableFile> <templateFile> [<outputFile>]");
                return 1;
            }
            string variableFile = args[0];
            string templateFile = args[1];
            string outputFile = args.Length > 2 ? args[2] : null;

            var varDictionary = new VariableDictionary(variableFile);
            var output = varDictionary.Evaluate(System.IO.File.ReadAllText(templateFile), out string err, haltOnError: false);
            if(outputFile != null){
                System.IO.File.WriteAllText(outputFile, output);
            }else{  // Output to console
                Console.WriteLine(output);
            }
            return 0;
        }
    }
}
