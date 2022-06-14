using System;
using System.Text;
using RapNet.IO;

namespace RapNet.EntryTypes;

/// <summary>
/// Represents raP entry 'enum' value.
/// </summary>
internal sealed class RapEnum : IBinarizedRapEntry
{
    /// <summary>
    /// Entry name.
    /// </summary>
    private string? Name { get; set; }

    /// <summary>
    /// Entry value.
    /// </summary>
    private int Value { get; set; }

    /// <summary>
    /// Converts raP entry in to a <see cref="IRapEntry"/> object.
    /// </summary>
    /// <param name="reader">Reader used for reading entry.</param>
    /// <param name="parent">Used for reading arrays recursive.</param>
    /// <returns>Returns a <see cref="IRapEntry"/> object.</returns>
    public IRapEntry FromBinary(RapBinaryReader reader, bool parent = false) => new RapEnum
    {
        Name = reader.ReadAsciiZ(),
        Value = reader.ReadInt()
    };

    /// <summary>
    /// Converts object to human-readable config format.
    /// </summary>
    /// <returns>Returns object as human-readable config format.</returns>
    public string ToConfigFormat() => new StringBuilder(Name ?? throw new NullReferenceException()).Append(" = ").Append(Value).ToString();
}
