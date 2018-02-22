using System.Collections.Generic;

using RapNet.Enums;
using RapNet.IO;

namespace RapNet.EntryTypes
{
    /// <summary>
    /// Represents raP entry 'array'.
    /// </summary>
    public class RapArray : IBinarizedRapEntry
    {
        /// <summary>
        /// raP array name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 'Entries' (items) inside raP array.
        /// </summary>
        public int Entries { get; set; }

        /// <summary>
        /// String entries inside raP array.
        /// </summary>
        public List<string> Strings { get; set; } = new List<string>();

        /// <summary>
        /// Float entries inside raP array.
        /// </summary>
        public List<float> Floats { get; set; } = new List<float>();

        /// <summary>
        /// Int64 entries inside raP array.
        /// </summary>
        public List<long> Longs { get; set; } = new List<long>();

        /// <summary>
        /// Recursive raP arrays inside raP array.
        /// </summary>
        public List<RapArray> Arrays { get; set; } = new List<RapArray>();

        /// <summary>
        /// Variable entries inside raP array.
        /// </summary>
        public List<string> Variables { get; set; } = new List<string>();

        /// <summary>
        /// Converts raP entry in to a IRapEntry object.
        /// </summary>
        /// <param name="reader">Reader used for reading entry.</param>
        /// <param name="parent">Used for reading arrays recursive.</param>
        /// <returns>Returns a IRapEntry object.</returns>
        public IRapEntry FromBinary(RapBinaryReader reader, bool parent = false)
        {
            var array = reader.ReadRapArray(parent);

            // Empty array.
            if (array.Entries == 0) {
                return array;
            }

            for (var i = 0; i < array.Entries; ++i) {
                var type = (RapValueType)reader.ReadByte();
                if (type == RapValueType.String) {
                    array.Strings.Add('"' + reader.ReadAsciiz() + '"');
                }
                else if (type == RapValueType.Float) {
                    array.Floats.Add(reader.ReadFloat());
                }
                else if (type == RapValueType.Long) {
                    array.Longs.Add(reader.ReadUint());
                }
                else if (type == RapValueType.Array) {
                    array.Arrays.Add((RapArray)FromBinary(reader, true));
                }
                else if (type == RapValueType.Variable) {
                    array.Variables.Add(reader.ReadAsciiz());
                }
            }

            return array;
        }

        /// <summary>
        /// Converts object to human-readable config format.
        /// </summary>
        /// <returns>Returns object as human-readable config format.</returns>
        public string ToConfigFormat()
        {
            var str = $"{ Name }[] = " + GetEntries();
            Arrays.ForEach(o => str += AppendChildArrays(o) + ", ");
            str += "};";

            return str.Replace(", }", "}").Replace("}{", "}, {");
        }

        private string AppendChildArrays(RapArray child)
        {
            var str = child.GetEntries();
            child.Arrays.ForEach(o => str += AppendChildrenRecursiveArrays(o));

            return str + "}";
        }

        private string AppendChildrenRecursiveArrays(RapArray recursiveChildrenArray)
        {
            var str = recursiveChildrenArray.GetEntries();
            recursiveChildrenArray.Arrays.ForEach(o => str += AppendChildArrays(o));

            return str + "}";
        }

        private string GetEntries()
        {
            var str = "{";

            str += string.Join(", ", Strings);
            str += string.Join(", ", Floats);
            str += string.Join(", ", Longs);

            if (Arrays.Count > 0 && str.Length > 0) {
                str += ", ";
            }

            return str;
        }
    }
}
