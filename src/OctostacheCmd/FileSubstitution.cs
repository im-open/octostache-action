using System;
using Octostache;

namespace OctostacheCmd
{
    public static class FileSubstitution
    {
        public static void DoOctostacheSubstitutions(string file, VariableDictionary variableDictionary)
        {
            var output = variableDictionary.Evaluate(System.IO.File.ReadAllText(file), out string err, haltOnError: false);

            if (string.IsNullOrEmpty(err))
            {
                System.IO.File.WriteAllText(file, output);
            }
            else
            {
                Console.WriteLine($"{Environment.NewLine}An error occurred while invoking octostache's substitution:{Environment.NewLine}");
                Console.WriteLine(err);
            }
        }
    }
}
