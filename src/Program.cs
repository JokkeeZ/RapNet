using System;
using System.IO;
using System.Linq;

using RapNet.Parsers;

namespace RapNet
{
    class Program
    {
        static AppSettings _settings;

        static void Main(string[] args)
        {
            Console.Title = "RapNet v0.0.1";

            Console.WriteLine();
            Console.WriteLine("RapNet - ArmA config extractor, by JokkeeZ");
            Console.WriteLine();

            if (args?.Length == 0) {
                PrintUsageInformation();
                return;
            }

            var filePath = args.Where(x => !x.StartsWith("-")).FirstOrDefault();
            if (!Path.HasExtension(filePath)) {
                Console.WriteLine("File path excepted as an argument.");
                return;
            }

            if (!File.Exists(filePath)) {
                Console.WriteLine("File doesn't exist.");
                return;
            }

            _settings = new AppSettings {
                UseSpaces = args.Contains("-s"),
                ExtractHeaderFiles = args.Contains("-e"),
                IncludePreprocessors = args.Contains("-d")
            };

            Start(filePath);
        }

        static void PrintUsageInformation()
        {
            Console.WriteLine("Usage: dotnet rapnet.dll [-d / -s / -e] FILEPATH");

            Console.WriteLine("Options:");
            Console.WriteLine("\t-d Includes '#define' preprocessors in top of a file. (If any)");
            Console.WriteLine("\t-s Use spaces instead of tabs.");
            Console.WriteLine("\t-e Extracts classes in to own header files.");

            Console.WriteLine("Example:");
            Console.WriteLine("\tdotnet rapnet.dll -d -s C:/path/to/config.bin");
        }

        static void Start(string filePath)
        {
            using (var parser = new RapBinaryParser(filePath)) {

                var config = parser.ParseConfig(filePath);
                if (config != null) {
                    CreateConfigFile(config, filePath);

                    var filename = Path.GetFileName(filePath);
                    var configFile = filename.Replace(Path.GetExtension(filename), ".cpp");

                    Console.WriteLine($"{ filename } -> { configFile }");
                }
            }
        }

        static void CreateConfigFile(Config config, string filePath)
        {
            using (var configCreator = new ConfigCreator(config, filePath)) {
                configCreator.CreateFile();
            }
        }

        public static AppSettings GetAppSettings() => _settings;
    }
}
