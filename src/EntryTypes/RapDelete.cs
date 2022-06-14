using System;
using System.Text;
using RapNet.IO;

namespace RapNet.EntryTypes;
/// <inheritdoc />
/// <summary>
/// Represents raP entry 'delete'.
/// </summary>
internal sealed class RapDelete : IBinarizedRapEntry
{
    /// <summary>
    /// Entry name.
    /// </summary>
    private string? Name { get; set; }

    /// <summary>
    /// Converts raP entry in to a IRapEntry object.
    /// </summary>
    /// <param name="reader">Reader used for reading entry.</param>
    /// <param name="parent">Used for reading arrays recursive.</param>
    /// <returns>Returns a IRapEntry object.</returns>
    public IRapEntry FromBinary(RapBinaryReader reader, bool parent = false) => new RapDelete
    {
        Name = reader.ReadAsciiZ()
    };

    /// <summary>
    /// Converts object to human-readable config format.
    /// </summary>
    /// <returns>Returns object as human-readable config format.</returns>
    public string ToConfigFormat() => new StringBuilder("delete /*class*/ ").Append(Name ?? throw new NullReferenceException()).Append(';').ToString();
}

