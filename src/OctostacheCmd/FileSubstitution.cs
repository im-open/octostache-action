using System;
using Octostache;

namespace OctostacheCmd
{
    public class FileSubstitution
    {
        private readonly string _templateFile;
        private readonly string _outputFile;

        public FileSubstitution(string templateFile, string outputFile = "")
        {
            this._templateFile = templateFile;
            this._outputFile = outputFile;
        }

        public void DoSubstitutions(VariableDictionary variableDictionary)
        {
            var output = variableDictionary.Evaluate(System.IO.File.ReadAllText(_templateFile), out string err, haltOnError: false);
            if (!string.IsNullOrEmpty(_outputFile))
            {
                System.IO.File.WriteAllText(_outputFile, output);
            }
            else
            {
                Console.WriteLine(output);
            }
        }
    }
}
