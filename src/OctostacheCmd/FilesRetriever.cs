using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace OctostacheCmd
{
    public static class FilesRetriever
    {
        public static IEnumerable<string> GetFiles(string fileGlobs)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var globs = fileGlobs.Split(',').Select(file => file.Trim());
            
            var matcher = new Matcher();
            matcher.AddIncludePatterns(globs);
            
            var files = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(currentDirectory)));
            return files.Files.Select(file => file.Path);
        }
    }
}