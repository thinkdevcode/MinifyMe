using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.IO;
using System.Linq;
using System.Text;
using Yahoo.Yui.Compressor;
using System.Dynamic;

namespace MinifyMe
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = jsonConfig.Deserialize("Config.json");

            if (config.minify_js)
                Compose(JavaScriptCompressor.Compress, config.js_files, "js");
            if (config.minify_css)
                Compose(CssCompressor.Compress, config.css_files, "css");

            Console.WriteLine("Minifying complete");
        }

        public static void Compose(Func<string, string> engine, string[] queries, string extension)
        {
            foreach (var query in queries)
            {
                // * wildcard makes app go through all subdirectorys for files
                if (query.Contains('*'))
                    CompressFiles(engine, Directory.GetFiles(Path.GetFullPath(query.Replace("*", "")), "*." + extension, SearchOption.AllDirectories));
                else
                {
                    //directory
                    if (String.IsNullOrWhiteSpace(Path.GetFileNameWithoutExtension(query)))
                        CompressFiles(engine, Directory.GetFiles(Path.GetFullPath(query), "*." + extension));
                    //file
                    else
                        CompressFiles(engine, new string[] { Path.GetFullPath(query) });
                }
            }
        }

        public static void CompressFiles(Func<string, string> engine, string[] files)
        {
            foreach (var file in files)
            {
                var file_full_path = Path.GetFullPath(file);
                var file_extension = Path.GetExtension(file);
                if (file_extension == ".bak") continue;
                Compress(engine, file_full_path.Replace(Path.GetFileName(file), "") + Path.GetFileNameWithoutExtension(file) + file_extension + ".bak", file_full_path);
            }
        }

        public static void Compress(Func<string, string> engine, string newFile, string oldFile)
        {
            try
            {
                Console.WriteLine("Minifying:" + Path.GetFileName(oldFile));
                if (File.Exists(newFile)) File.Delete(newFile);
                File.Copy(oldFile, newFile);
                File.Delete(oldFile);
                File.WriteAllText(oldFile, engine(File.ReadAllText(newFile)));
            }
            catch
            {
                Console.WriteLine("Error occurred while minifying \"" + Path.GetFileName(oldFile) + "\"... continuing with next file");
            }
        }
    }

    public class jsonConfig
    {
        public bool minify_css { get; set; }
        public bool minify_js { get; set; }
        public string minify_rename { get; set; }
        public string[] js_files { get; set; }
        public string[] css_files { get; set; }
        public static jsonConfig Deserialize(string path)
        {
            return new JavaScriptSerializer().Deserialize<jsonConfig>(File.ReadAllText(path));
        }
    }
}
