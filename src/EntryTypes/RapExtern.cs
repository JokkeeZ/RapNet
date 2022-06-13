using System.Text;
using RapNet.IO;

namespace RapNet.EntryTypes
{
    /// <summary>
    /// Represents raP entry 'extern'.
    /// </summary>
    public class RapExtern : IBinarizedRapEntry
    {
        /// <summary>
        /// Extern class reference name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Converts raP entry in to a <see cref="IRapEntry"/> object.
        /// </summary>
        /// <param name="reader">Reader used for reading entry.</param>
        /// <param name="parent">Used for reading arrays recursive.</param>
        /// <returns>Returns a <see cref="IRapEntry"/> object.</returns>
        public IRapEntry FromBinary(RapBinaryReader reader, bool parent = false) => new RapExtern
        {
            Name = reader.ReadAsciiz()
        };

        /// <summary>
        /// Converts object to human-readable config format.
        /// </summary>
        /// <returns>Returns object as human-readable config format.</returns>
        public string ToConfigFormat() => new StringBuilder("/*external*/ class ").Append(Name).Append(';').ToString();
    }
}
