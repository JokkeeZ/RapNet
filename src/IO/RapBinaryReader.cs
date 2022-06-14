using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using RapNet.EntryTypes;
using RapNet.Enums;
using RapNet.ValueTypes;

namespace RapNet.IO;
/// <summary>
/// Provides raP functionality for a BinaryReader class.
/// </summary>
internal sealed class RapBinaryReader : IDisposable 
{
    private readonly BinaryReader _reader;

    /// <summary>
    /// Initializes a new instance of the RapBinaryReader class, with binary data.
    /// </summary>
    /// <param name="binaryData">Binarized raP data.</param>
    internal RapBinaryReader(byte[] binaryData) 
    {
        _reader = new BinaryReader(new MemoryStream(binaryData, false));
    }

    /// <summary>
    /// Reads null terminated string.
    /// </summary>
    /// <returns>Returns next available AsciiZ string on stream.</returns>
    internal string ReadAsciiZ() 
    {
        var bytes = new List<byte>();

        while (_reader.PeekChar() != 0) bytes.Add(_reader.ReadByte());

        if (_reader.PeekChar() == 0) {
            // AsciiZ string is terminated with NULL char (0x00 / '\0').
            // Since we don't need that in C#, let's remove it.
            _reader.ReadByte();
        }

        return Encoding.ASCII.GetString(bytes.ToArray());
    }

    internal RapStringValue ReadRapString() => new(ReadAsciiZ());
    internal RapVariableValue ReadRapVariable() => new(ReadAsciiZ());
    internal RapIntegerValue ReadRapInt() => new(ReadInt());
    internal RapIntegerValue ReadRapUInt() => new(ReadUint());
    internal RapFloatValue ReadRapFloat() => new(ReadFloat());

    /// <summary>
    /// Reads next available RapArray on stream.
    /// </summary>
    /// <param name="parent">Defines if array is child/parent array.</param>
    /// <returns>Returns next available RapArray on stream.</returns>
    internal RapArrayValue ReadRapArray() 
    {
        var output = new RapArrayValue() {
            EntryCount = ReadCompressedInteger()
        };
        if (output.EntryCount == 0) return output;

        for (var i = 0; i < output.EntryCount; ++i) {
            switch ((RapValueType) ReadByte()) {
                case RapValueType.String:
                    output.Value.Add(ReadRapString());
                    break;
                case RapValueType.Float:
                    output.Value.Add(ReadRapFloat());
                    break;
                case RapValueType.Long:
                    output.Value.Add(ReadRapUInt());
                    break;
                case RapValueType.Array:
                    output.Value.Add(ReadRapArray());
                    break;
                case RapValueType.Variable:
                    output.Value.Add(ReadRapVariable());
                    break;
                default: throw new Exception("How the fuck did you get here?");
            }
        }

        return output;
    }

    /// <summary>
    /// Reads compressed integer.
    /// <para>https://community.bistudio.com/wiki/raP_File_Format_-_OFP#CompressedInteger</para>
    /// </summary>
    /// <returns>Returns next available compressed integer on stream.</returns>
    internal int ReadCompressedInteger() 
    {
        int value;
        if ((value = _reader.ReadByte()) == 0) {
            return 0;
        }

        while ((value & 0x80) != 0) {
            int extra;
            if ((extra = _reader.ReadByte()) == 0) {
                return 0;
            }

            value += (extra - 1) * 0x80;
        }

        return value;
    }

    /// <summary>
    /// Reads file header. If file starts with correct header ('\0raP') returns true; otherwise false.
    /// </summary>
    /// <returns>Returns true if file starts with correct header; otherwise false.</returns>
    internal bool ReadHeader() 
    {
        var bits = _reader.ReadBytes(4);
        return bits[0] == '\0' && bits[1] == 'r' && bits[2] == 'a' && bits[3] == 'P';
    }

    /// <summary>
    /// Reads byte.
    /// </summary>
    /// <returns>Returns next byte available on stream.</returns>
    internal byte ReadByte() => _reader.ReadByte();

    /// <summary>
    /// Reads Int32.
    /// </summary>
    /// <returns>Returns next Int32 available on stream.</returns>
    internal int ReadInt() => _reader.ReadInt32();

    /// <summary>
    /// Reads float.
    /// </summary>
    /// <returns>Returns next Single available on stream.</returns>
    internal float ReadFloat() => _reader.ReadSingle();

    /// <summary>
    /// Reads next UInt32.
    /// </summary>
    /// <returns>Returns next Uint32 available on stream.</returns>
    internal uint ReadUint() => _reader.ReadUInt32();

    /// <summary>
    /// Sets current position on stream.
    /// </summary>
    /// <param name="position">New position on stream.</param>
    internal void SetPosition(uint position) => _reader.BaseStream.Position = position;

    /// <summary>
    /// Reads binarized IRapEntry.
    /// </summary>
    /// <typeparam name="TRapEntry">Object with type IBinarizedRapEntry.</typeparam>
    /// <returns>Returns next TRapEntry available on stream.</returns>
    internal TRapEntry ReadBinarizedRapEntry<TRapEntry>(bool arr = false)
        where TRapEntry : class, IBinarizedRapEntry, new() 
    {
        var tRapEntry = new TRapEntry();
        return (TRapEntry) tRapEntry.FromBinary(this, arr);
    }

    /// <summary>
    /// Checks if binarized config file is using Operation FlashPoint format.
    /// </summary>
    /// <returns>Returns true, if config file is using Operation FlashPoint format; otherwise false.</returns>
    internal bool IsOperationFlashpointFormat() 
    {
        var pos = _reader.BaseStream.Position;

        // Skip '\0raP\0'
        SetPosition(5);
        var bits = _reader.ReadBytes(4);

        // 04\0\0
        var isOfp = bits[0] == '0' && bits[1] == '4' && bits[2] == '\0' && bits[3] == '\0';

        // Reset position
        SetPosition((uint) pos);
        return isOfp;
    }

    /// <summary>
    /// Disposes current stream instance.
    /// </summary>
    public void Dispose() 
    {
        _reader.Dispose();
        GC.SuppressFinalize(this);
    }
    
}