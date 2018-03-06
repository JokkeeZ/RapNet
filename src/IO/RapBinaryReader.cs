using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using RapNet.EntryTypes;

namespace RapNet.IO
{
    /// <summary>
    /// Provides raP funcionality for a BinaryReader class.
    /// </summary>
    public class RapBinaryReader : IDisposable
    {
        private readonly BinaryReader _reader;

        /// <summary>
        /// Initializes a new instance of the RapBinaryReader class, with binary data.
        /// </summary>
        /// <param name="binaryData">Binarized raP data.</param>
        public RapBinaryReader(byte[] binaryData)
        {
            _reader = new BinaryReader(new MemoryStream(binaryData, false));
        }

        /// <summary>
        /// Reads null terminated string.
        /// </summary>
        /// <returns>Returns next available Asciiz string on stream.</returns>
        public string ReadAsciiz()
        {
            var bytes = new List<byte>();

            while (_reader.PeekChar() != 0) {
                bytes.Add(_reader.ReadByte());
            }

            if (_reader.PeekChar() == 0) {

                // Asciiz string is terminated with NULL char (0x00 / '\0').
                // Since we don't need that in C#, let's remove it.
                _reader.ReadByte();
            }

            return Encoding.ASCII.GetString(bytes.ToArray());
        }

        /// <summary>
        /// Reads compressed integer.
        /// <para>https://community.bistudio.com/wiki/raP_File_Format_-_OFP#CompressedInteger</para>
        /// </summary>
        /// <returns>Returns next available compressed integer on stream.</returns>
        public int ReadCompressedInteger()
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
        public bool ReadHeader()
        {
            var bits = _reader.ReadBytes(4);
            return bits[0] == '\0' && bits[1] == 'r' && bits[2] == 'a' && bits[3] == 'P';
        }

        /// <summary>
        /// Reads next available RapArray on stream.
        /// </summary>
        /// <param name="parent">Defines if array is child/parent array.</param>
        /// <returns>Returns next available RapArray on stream.</returns>
        public RapArray ReadRapArray(bool parent = false)
        {
            var array = new RapArray();

            if (!parent) {

                // A recursive array recurses into further entries with no name attached.
                // https://community.bistudio.com/wiki/raP_File_Format_-_Elite#ArrayElements
                array.Name = ReadAsciiz();
            }

            array.Entries = ReadCompressedInteger();
            return array;
        }

        /// <summary>
        /// Reads byte.
        /// </summary>
        /// <returns>Returns next byte available on stream.</returns>
        public byte ReadByte() => _reader.ReadByte();

        /// <summary>
        /// Reads Int32.
        /// </summary>
        /// <returns>Returns next Int32 available on stream.</returns>
        public int ReadInt() => _reader.ReadInt32();

        /// <summary>
        /// Reads float.
        /// </summary>
        /// <returns>Returns next Single available on stream.</returns>
        public float ReadFloat() => _reader.ReadSingle();

        /// <summary>
        /// Reads next UInt32.
        /// </summary>
        /// <returns>Returns next Uint32 available on stream.</returns>
        public uint ReadUint() => _reader.ReadUInt32();

        /// <summary>
        /// Sets current position on stream.
        /// </summary>
        /// <param name="position">New position on stream.</param>
        public void SetPosition(uint position) => _reader.BaseStream.Position = position;

        /// <summary>
        /// Reads binarized IRapEntry.
        /// </summary>
        /// <typeparam name="TRapEntry">Object with type IBinarizedRapEntry.</typeparam>
        /// <returns>Returns next TRapEntry available on stream.</returns>
        public TRapEntry ReadBinarizedRapEntry<TRapEntry>() where TRapEntry : class, IBinarizedRapEntry, new()
        {
            var tRapEntry = new TRapEntry();
            return (TRapEntry)tRapEntry.FromBinary(this);
        }

        /// <summary>
        /// Checks if binarized config file is using Operation Flashpoint format.
        /// </summary>
        /// <returns>Returns true, if config file is using Operation Flashpoint format; otherwise false.</returns>
        public bool IsOperationFlashpointFormat()
        {
            var pos = _reader.BaseStream.Position;

            // Skip '\0raP\0'
            SetPosition(5);
            var bits = _reader.ReadBytes(4);

            // 04\0\0
            var isOfp = bits[0] == '0' && bits[1] == '4' && bits[2] == '\0' && bits[3] == '\0';

            // Reset position
            SetPosition((uint)pos);
            return isOfp;
        }

        /// <summary>
        /// Disposes current stream instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && (_reader != null)) {
                _reader.Dispose();
            }
        }
    }
}
