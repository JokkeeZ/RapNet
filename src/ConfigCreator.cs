using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using RapNet;
using RapNet.EntryTypes;
using RapNet.Preprocessors;

namespace RapNet
{
    class ConfigCreator : IDisposable
    {
        private int _paddingSize = 1;

        private readonly Config _config;
        private readonly StreamWriter _writer;
        private readonly string _path;

        private string Padding
        {
            get {
                if (!Program.GetAppSettings().UseSpaces) {
                    return new string('\t', _paddingSize);
                }

                return string.Join(" ", Enumerable.Repeat(" ", _paddingSize));
            }
        }

        public ConfigCreator(Config config, string path)
        {
            _config = config;
            _path = path;

            _writer = new StreamWriter(path.Replace(Path.GetExtension(path), ".cpp"));
        }

        private void WriteClass(RapClass rapClass, StreamWriter headerWriter = null)
        {
            var writer = headerWriter ?? _writer;

            writer.Write(rapClass.ToConfigFormat());

            if (rapClass.Entries == 0) {
                writer.WriteLine("};");
                return;
            }

            writer.WriteLine();
            WriteEntriesFromClass(rapClass, writer);

            writer.WriteLine("};");
        }

        private void WriteHeaderComment(StreamWriter headerWriter = null)
        {
            var writer = headerWriter ?? _writer;

            writer.Write("// Original file: {0}", Path.GetFileName(_path));
            writer.Write("// File generated with RapNet / https://github.com/jokkeez");

            writer.WriteLine();
        }

        public void CreateFile()
        {
            WriteHeaderComment();

            if (Program.GetAppSettings().IncludePreprocessors) {
                _writer.WriteLine(ConfigDefinePreprocessors.GetDefines());
            }

            for (var i = 0; i < _config.Classes.Count; ++i) {
                if (Program.GetAppSettings().ExtractHeaderFiles) {
                    WriteHeaderFileClass(_config.Classes[i]);
                    continue;
                }

                WriteClass(_config.Classes[i]);
                var lastClass = i != (_config.Classes.Count - 1);

                if (lastClass) {
                    _writer.WriteLine();
                }
            }

            if (_config.Enums.Count > 0) {
                WriteEnums();
            }

            _writer.Flush();
        }

        private void WriteEnums()
        {
            _writer.WriteLine("enums {");

            var enums = new List<string>();
            _config.Enums.ForEach(o => enums.Add(Program.GetAppSettings().UseSpaces ? "  " : "\t" + o.ToConfigFormat()));

            _writer.Write(string.Join($" ,{ Environment.NewLine }", enums));
            _writer.WriteLine();
            _writer.Write("};");
        }

        private void WriteHeaderFileClass(RapClass rapClass)
        {
            var filePath = _path.Replace(Path.GetFileName(_path), $"{ rapClass.Name }.hpp");
            using (var stream = new StreamWriter(filePath)) {
                WriteHeaderComment(stream);
                WriteClass(rapClass, stream);
                stream.Flush();
            }

            AddIncludePreprocessorForFile(filePath);
        }

        private void AddIncludePreprocessorForFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            _writer.WriteLine($"#include \"{ fileName }\"");
        }

        private void WriteChildrenClass(RapClass child, StreamWriter headerWriter = null)
        {
            var writer = headerWriter ?? _writer;

            writer.Write(Padding + child.ToConfigFormat());

            if (child.Entries != 0) {
                writer.WriteLine();

                _paddingSize++;
                WriteEntriesFromClass(child, writer);
                _paddingSize--;

                writer.WriteLine(Padding + "};");
            }
            else {
                writer.Write(" };");
            }
        }

        private void WriteEntriesFromClass(RapClass child, StreamWriter headerWriter = null)
        {
            var writer = headerWriter ?? _writer;

            child.Deletes.ForEach(o => writer.WriteLine(Padding + o.ToConfigFormat()));
            child.Externs.ForEach(o => writer.WriteLine(Padding + o.ToConfigFormat()));

            child.Values.ForEach(o => writer.WriteLine(Padding + o.ToConfigFormat()));
            child.Arrays.ForEach(o => writer.WriteLine(Padding + o.ToConfigFormat()));

            for (var i = 0; i < child.Classes.Count; ++i) {
                var endWithNewLine = i != (child.Classes.Count - 1);
                WriteChildrenClass(child.Classes[i], writer);

                if (endWithNewLine) {
                    writer.WriteLine();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && (_writer != null)) {
                _writer.Dispose();
            }
        }
    }
}
