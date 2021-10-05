using System;
using System.Collections.Generic;
using System.Linq;

namespace OctostacheCmd
{
    public class EnvironmentVariableRetriever
    {
        public static IDictionary<string, string> GetAllVariables()
        {
            var allEnvironmentVariables = Environment.GetEnvironmentVariables();
            var environmentVariableDictionary =
                allEnvironmentVariables
                    .Keys
                    .Cast<object>()
                    .ToDictionary(key => key.ToString(), key => allEnvironmentVariables[key].ToString());

            return environmentVariableDictionary;
        }
    }
}
