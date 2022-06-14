using System;
using System.Text;
using RapNet.Enums;
using RapNet.IO;

namespace RapNet.EntryTypes;
/// <summary>
/// Represents raP entry 'value'.
/// </summary>
internal sealed class RapValue : IBinarizedRapEntry 
{
    /// <summary>
    /// Tells which type of value this raP entry holds.
    /// </summary>
    internal RapValueType SubType { get; set; }

    /// <summary>
    /// raP entry name.
    /// </summary>
    private string? Name { get; set; }

    /// <summary>
    /// raP entry value
    /// </summary>
    private IRapEntry? Value { get; set; }


    /// <summary>
    /// Converts raP entry in to a <see cref="IRapEntry"/> object.
    /// </summary>
    /// <param name="reader">Reader used for reading entry.</param>
    /// <param name="parent">Used for reading arrays recursive.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <returns>Returns a <see cref="IRapEntry"/> object.</returns>
    public IRapEntry FromBinary(RapBinaryReader reader, bool parent = false) 
    {
        if (parent || SubType == RapValueType.Array) {
            return new RapValue() {
                SubType = RapValueType.Array,
                Name = reader.ReadAsciiZ(),
                Value = reader.ReadRapArray()
            };
        }

        var rapValue = new RapValue {
            SubType = (RapValueType) reader.ReadByte(),
            Name = reader.ReadAsciiZ()
        };

        IRapEntry value = rapValue.SubType switch {
            RapValueType.String => reader.ReadRapString(),
            RapValueType.Variable => reader.ReadRapVariable(),
            RapValueType.Float => reader.ReadRapFloat(),
            RapValueType.Long => reader.ReadRapInt(),
            RapValueType.Array => reader.ReadRapArray(),
            _ => throw new Exception("How did we get here?")
        };

        rapValue.Value = value;
        return rapValue;
    }

    /// <summary>
    /// Converts object to human-readable config format.
    /// </summary>
    /// <returns>Returns object as human-readable config format.</returns>
    public string ToConfigFormat() 
    { 
        var builder = new StringBuilder(Name);
        var value = Value ?? throw new NullReferenceException();
        if (SubType == RapValueType.Array) builder.Append("[]");
        return builder.Append(" = ").Append(value.ToConfigFormat()).Append(';').ToString();
    }
}
