using RapNet.Enums;
using RapNet.IO;
using RapNet.Preprocessors;

namespace RapNet.EntryTypes
{
    /// <summary>
    /// Represents raP entry 'value'.
    /// </summary>
    public class RapValue : IBinarizedRapEntry
    {
        /// <summary>
        /// Tells which type of value this raP entry holds.
        /// </summary>
        public RapValueType SubType { get; set; }

        /// <summary>
        /// raP entry name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// raP entry value as string.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Converts raP entry in to a <see cref="IRapEntry"/> object.
        /// </summary>
        /// <param name="reader">Reader used for reading entry.</param>
        /// <param name="parent">Used for reading arrays recursive.</param>
        /// <returns>Returns a <see cref="IRapEntry"/> object.</returns>
        public IRapEntry FromBinary(RapBinaryReader reader, bool parent = false)
        {
            var rapValue = new RapValue {
                SubType = (RapValueType)reader.ReadByte(),
                Name = reader.ReadAsciiz(),
            };

            object value = null;
            if (rapValue.SubType == RapValueType.String 
                || rapValue.SubType == RapValueType.Variable) {
                value = reader.ReadAsciiz();
            }
            else if (rapValue.SubType == RapValueType.Float) {
                value = reader.ReadFloat();
            }
            else if (rapValue.SubType == RapValueType.Long) {
                value = reader.ReadInt();

                if (Program.GetAppSettings().IncludePreprocessors) {

                    var defines = DefinePreprocessors.GetDefinesForRapValue(rapValue.Name);
                    if (defines.Count != 0) {

                        var define = defines.Find(x => x.Value == (int)value);
                        if (define != null) {
                            value = define.Name;
                            ConfigDefinePreprocessors.AddDefines(defines);
                        }
                    }
                }
            }

            rapValue.Value = value.ToString();
            return rapValue;
        }

        /// <summary>
        /// Converts object to human-readable config format.
        /// </summary>
        /// <returns>Returns object as human-readable config format.</returns>
        public string ToConfigFormat()
        {
            var val = Value;
            if (SubType == RapValueType.String) {
                val = $"\"{ Value }\"";
            }
            else if (SubType == RapValueType.Float) {
                val = val.Replace(',', '.');
            }

            return $"{ Name } = { val };";
        }
    }
}
