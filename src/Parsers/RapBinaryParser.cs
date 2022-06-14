using System;
using System.Collections.Generic;
using System.IO;

using RapNet.EntryTypes;
using RapNet.Enums;
using RapNet.IO;

namespace RapNet.Parsers;

/// <inheritdoc />
/// <summary>
/// Parses raPified binary files in to a Config instance.
/// </summary>
internal sealed class RapBinaryParser : IDisposable {
    /// <summary>
    /// Offset to stream position where enums begin.
    /// </summary>
    private uint _enumOffset;

    private readonly RapBinaryReader _reader;

    /// <summary>
    /// Initializes a new instance of the RapBinaryParser class, with raP binary file path.
    /// </summary>
    /// <param name="filePath">Rap binary file path</param>
    public RapBinaryParser(string filePath) => _reader = new RapBinaryReader(File.ReadAllBytes(filePath));

    /// <summary>
    /// Creates a new instance of the Config object, with file contents decoded.
    /// </summary>
    /// <returns>Returns a new instance of the Config object, with file contents decoded.</returns>
    public Config? ParseConfig() 
    {
        var config = new Config();

        if (!_reader.ReadHeader()) {
            Console.WriteLine("Invalid header.");
            return null;
        }

        if (_reader.IsOperationFlashpointFormat()) {
            Console.WriteLine("OFP format is not supported.");
            return null;
        }

        var always0 = _reader.ReadUint();
        var always8 = _reader.ReadUint();

        if (always0 != 0 && always8 != 8) {
            Console.WriteLine("Invalid const values.");
            return null;
        }

        _enumOffset = _reader.ReadUint();

        if (!ReadParentClasses(config)) {
            Console.WriteLine("No parent class bodies.");
            return null;
        }

        if (!ReadChildClasses(config)) {
            Console.WriteLine("No child classes.");
            return null;
        }

        config.Enums.AddRange(ReadEnums());
        return config;
    }

    private IEnumerable<RapEnum> ReadEnums() 
    {
        _reader.SetPosition(_enumOffset);

        var enumsCount = _reader.ReadInt();
        if (enumsCount == 0) {
            return new List<RapEnum>(0);
        }

        var enums = new List<RapEnum>();
        for (var i = 0; i < enumsCount; ++i) enums.Add(_reader.ReadBinarizedRapEntry<RapEnum>());

        return enums;
    }

    private bool ReadParentClasses(Config config) 
    {
        _reader.ReadAsciiZ();
        config.Entries = _reader.ReadCompressedInteger();

        for (var i = 0; i < config.Entries; ++i) {
            // This byte defines entry type.
            // Don't need it, since there is only one type of entries. (classes)
            _reader.ReadByte();
            config.Classes.Add(_reader.ReadBinarizedRapEntry<RapClass>());
        }

        return config.Classes.Count > 0;
    }

    private bool ReadChildClasses(Config config) 
    {
        config.Classes.ForEach(LoadChildrenClasses);
        return config.Classes.Count > 0;
    }

    private void LoadChildrenClasses(RapClass child) 
    {
        _reader.SetPosition(child.Offset);

        child.InheritedClassname = _reader.ReadAsciiZ();
        child.Entries = _reader.ReadCompressedInteger();

        // Just used for repeating X times to add all entries.
        for (var i = 0; i < child.Entries; ++i) {
            AddEntryToClass(child);
        }

        // Recursively load child class children.
        child.Classes.ForEach(LoadChildrenClasses);
    }

    private void AddEntryToClass(RapClass rapClass) 
    {
        var entryType = (RapEntryType) _reader.ReadByte();

        var entry = RapEntryFactory.CreateEntryForType(entryType);
        rapClass.AddEntry(entry, _reader);
    }

    public void Dispose() 
    {
        _reader.Dispose();
        GC.SuppressFinalize(this);
    }
}
