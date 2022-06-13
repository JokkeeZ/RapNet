using System;
using System.Text;
using RapNet.Enums;
using RapNet.IO;
using RapNet.Preprocessors;
using RapNet.ValueTypes;

namespace RapNet.EntryTypes
{
    /// <summary>
    /// Represents raP entry 'value'.
    /// </summary>
    public class RapValue : IBinarizedRapEntry {
        
        /// <summary>
        /// Tells which type of value this raP entry holds.
        /// </summary>
        public RapValueType SubType { get; set; }

        /// <summary>
        /// raP entry name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// raP entry value
        /// </summary>
        public IRapEntry? Value { get; set; }
        


        /// <summary>
        /// Converts raP entry in to a <see cref="IRapEntry"/> object.
        /// </summary>
        /// <param name="reader">Reader used for reading entry.</param>
        /// <param name="parent">Used for reading arrays recursive.</param>
        /// <returns>Returns a <see cref="IRapEntry"/> object.</returns>
        public IRapEntry FromBinary(RapBinaryReader reader, bool parent = false)
        {
            if (parent || SubType == RapValueType.Array) {
                return new RapValue() {
                    SubType = RapValueType.Array,
                    Name = reader.ReadAsciiz(),
                    Value = reader.ReadRapArray()
                };
            }
            
            var rapValue = new RapValue {
                SubType = (RapValueType)reader.ReadByte(),
                Name = reader.ReadAsciiz()
            };

            IRapEntry value = null;
            if (rapValue.SubType == RapValueType.String) {
                value = reader.ReadRapString();
            } else if ( rapValue.SubType == RapValueType.Variable) {
                value = reader.ReadRapVariable();
            } else if (rapValue.SubType == RapValueType.Float) {
                value = reader.ReadRapFloat();
            } else if (rapValue.SubType == RapValueType.Long) {
                value = reader.ReadRapInt();
                //TODO: Completely rework "preprocessor" system as I seem to have broke it
                // if (Program.GetAppSettings().IncludePreprocessors) {

                    // var defines = DefinePreprocessors.GetDefinesForRapValue(rapValue.Name);
                    // if (defines.Count != 0) {

                        // var define = defines.Find(x => x.Value == (RapIntegerValue)value.);
                        // if (define != null) {
                            // value = define.Name;
                            // ConfigDefinePreprocessors.AddDefines(defines);
                        // }
                    // }
                // }
            } else if (rapValue.SubType == RapValueType.Array) {
                value = reader.ReadRapArray();
            }
            rapValue.Value = value;
            return rapValue;
        }

        /// <summary>
        /// Converts object to human-readable config format.
        /// </summary>
        /// <returns>Returns object as human-readable config format.</returns>
        public string ToConfigFormat() {
            var builder = new StringBuilder(Name);
            if (SubType == RapValueType.Array) builder.Append("[]");
            return builder.Append(" = ").Append(Value.ToConfigFormat()).Append(';').ToString();
        }
    }
}
