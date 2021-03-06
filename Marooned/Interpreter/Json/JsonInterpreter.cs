using System;
using System.IO;
using System.Linq;

namespace Marooned.Interpreter.Json
{
    public class JsonInterpreter
    {
        public JsonInterpreter(GameContext gameContext)
        {
            GameContext = gameContext;
        }

        public GameContext GameContext { get; set; }
        public virtual string Path { get; set; }
        public virtual string ContentPath {
            get => $"{GameContext.Content.RootDirectory}\\{Path}";
        }

        public static string RemoveExtension(string path)
        {
            return System.IO.Path.ChangeExtension(path, null);
        }

        public T Load<T>(string path)
        {
            return GameContext.Content.Load<T>(RemoveExtension(path));
        }

        public virtual string FindJsonByName(string name, bool fullPath)
        {
            string foundJsonPath = Directory.EnumerateFiles(ContentPath, "*.json", SearchOption.AllDirectories)
                    .Where(f => System.IO.Path.GetFileNameWithoutExtension(f) == name)
                    .First();
            if (fullPath)
            {
                return foundJsonPath;
            }
            return foundJsonPath.Substring(foundJsonPath.IndexOf("\\") + 1);
        }

        public string GetFileContents(string name)
        {
            // Need the full path to manually parse JSON
            string path = FindJsonByName(name, true);
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (File.Exists(path) && stream.Length > 0)
                {
                    string fileContents;
                    using (StreamReader reader = new StreamReader(path))
                    {
                        fileContents = reader.ReadToEnd();
                    }
                    return fileContents;
                }
                else
                {
                    throw new Exception($"Could not find json: {path}");
                }
            }
        }
    }
}
