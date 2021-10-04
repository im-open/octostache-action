using System;
using System.Collections.Generic;
using System.Linq;

namespace OctostacheCmd
{
    public class EnvironmentVariableRetriever
    {
        private static string[] _predefinedVariablePrefixes =
            {"runner.", "azure_http_user_agent", "common.", "system."};

        public static IDictionary<string, string> GetAllVariables()
        {
            var allEnvironmentVariables = Environment.GetEnvironmentVariables();
            var environmentVariableDictionary =
                allEnvironmentVariables
                    .Keys
                    .Cast<object>()
                    .Where(key => !isPredefinedVariable(key.ToString()))
                    .ToDictionary(key => key.ToString(), key => allEnvironmentVariables[key].ToString());

            return environmentVariableDictionary;
        }

        private static bool isPredefinedVariable(string variable)
        {
            return _predefinedVariablePrefixes.Any(prefix => variable.ToLower().StartsWith(prefix));
        }
    }
}
