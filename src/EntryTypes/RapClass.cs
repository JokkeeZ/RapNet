using System.Collections.Generic;
using RapNet.Enums;
using RapNet.IO;

namespace RapNet.EntryTypes
{
    /// <summary>
    /// Represents raP entry 'class'.
    /// </summary>
    public class RapClass : IBinarizedRapEntry
    {
        /// <summary>
        /// rap entry name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Inherited class name.
        /// </summary>
        public string InheritedClassname { get; set; } = "";

        /// <summary>
        /// Count of entries.
        /// </summary>
        public int Entries { get; set; } = -1;

        /// <summary>
        /// Start offset in binary file.
        /// </summary>
        public uint Offset { get; set; }

        /// <summary>
        /// Children class entries.
        /// </summary>
        public List<RapClass> Classes { get; set; } = new List<RapClass>();

        /// <summary>
        /// Value & Array entries.
        /// </summary>
        public List<RapValue> Values { get; set; } = new List<RapValue>();
        
        /// <summary>
        /// Extern class entries.
        /// </summary>
        public List<RapExtern> Externs { get; set; } = new List<RapExtern>();

        /// <summary>
        /// Delete class entries.
        /// </summary>
        public List<RapDelete> Deletes { get; set; } = new List<RapDelete>();

        /// <summary>
        /// Converts raP entry in to a IRapEntry object.
        /// </summary>
        /// <param name="reader">Reader used for reading entry.</param>
        /// <param name="parent">Used for reading arrays recursive.</param>
        /// <returns>Returns a IRapEntry object.</returns>
        public IRapEntry FromBinary(RapBinaryReader reader, bool parent = false) => new RapClass
        {
            Name = reader.ReadAsciiz(),
            Offset = reader.ReadUint()
        };

        /// <summary>
        /// Adds an IRapEntry to entry collection based on Type.
        /// </summary>
        /// <param name="entry">Entry to be added.</param>
        /// <param name="reader">Reader for reading binarized rap entry.</param>
        public void AddEntry(IRapEntry entry, RapBinaryReader reader)
        {
            if (entry is RapClass){
                Classes.Add(reader.ReadBinarizedRapEntry<RapClass>());
            }

            if (entry is RapExtern){
                Externs.Add(reader.ReadBinarizedRapEntry<RapExtern>());
            }

            if (entry is RapDelete){
                Deletes.Add(reader.ReadBinarizedRapEntry<RapDelete>());
            }
            
            if (entry is RapValue val){
                if (val.SubType == RapValueType.Array) Values.Add(reader.ReadBinarizedRapEntry<RapValue>(true)); 
                else Values.Add(reader.ReadBinarizedRapEntry<RapValue>());
            }
            
        }

        /// <summary>
        /// Converts object to human-readable config format.
        /// </summary>
        /// <returns>Returns object as human-readable config format.</returns>
        public string ToConfigFormat()
        {
            var value = $"class { Name }";
            if (InheritedClassname.Length == 0) {
                return value += " {";
            }

            value += $" : { InheritedClassname } ";
            value += '{';
            return value;
        }
    }
}
