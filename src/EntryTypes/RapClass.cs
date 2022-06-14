using System;
using System.Collections.Generic;
using System.Text;
using RapNet.Enums;
using RapNet.IO;

namespace RapNet.EntryTypes;

/// <inheritdoc />
/// <summary>
/// Represents raP entry 'class'.
/// </summary>
internal sealed class RapClass : IBinarizedRapEntry 
{
    /// <summary>
    /// rap entry name.
    /// </summary>
    internal string? Name { get; set; }

    /// <summary>
    /// Inherited class name.
    /// </summary>
    internal string InheritedClassname { get; set; } = "";

    /// <summary>
    /// Count of entries.
    /// </summary>
    internal int Entries { get; set; } = -1;

    /// <summary>
    /// Start offset in binary file.
    /// </summary>
    internal uint Offset { get; set; }

    /// <summary>
    /// Children class entries.
    /// </summary>
    internal List<RapClass> Classes { get; set; } = new List<RapClass>();

    /// <summary>
    /// Value & Array entries.
    /// </summary>
    internal List<RapValue> Values { get; set; } = new List<RapValue>();

    /// <summary>
    /// Extern class entries.
    /// </summary>
    internal List<RapExtern> ExternalClasses { get; set; } = new List<RapExtern>();

    /// <summary>
    /// Delete class entries.
    /// </summary>
    internal List<RapDelete> DeleteStatements { get; set; } = new List<RapDelete>();

    /// <summary>
    /// Converts raP entry in to a IRapEntry object.
    /// </summary>
    /// <param name="reader">Reader used for reading entry.</param>
    /// <param name="parent">Used for reading arrays recursive.</param>
    /// <returns>Returns a IRapEntry object.</returns>
    public IRapEntry FromBinary(RapBinaryReader reader, bool parent = false) => new RapClass 
    {
        Name = reader.ReadAsciiZ(),
        Offset = reader.ReadUint()
    };

    /// <summary>
    /// Adds an IRapEntry to entry collection based on Type.
    /// </summary>
    /// <param name="entry">Entry to be added.</param>
    /// <param name="reader">Reader for reading binarized rap entry.</param>
    internal void AddEntry(IRapEntry entry, RapBinaryReader reader) 
    {
        switch (entry) {
            case RapClass:
                Classes.Add(reader.ReadBinarizedRapEntry<RapClass>());
                break;
            case RapExtern:
                ExternalClasses.Add(reader.ReadBinarizedRapEntry<RapExtern>());
                break;
            case RapDelete:
                DeleteStatements.Add(reader.ReadBinarizedRapEntry<RapDelete>());
                break;
            case RapValue val:
                if (val.SubType == RapValueType.Array) {
                    Values.Add(reader.ReadBinarizedRapEntry<RapValue>(true));
                    break;
                }

                Values.Add(reader.ReadBinarizedRapEntry<RapValue>());
                break;
            default: throw new Exception("How did we get here?");
        }
    }

    /// <summary>
    /// Converts object to human-readable config format.
    /// </summary>
    /// <returns>Returns object as human-readable config format.</returns>
    public string ToConfigFormat() 
    {
        var builder = new StringBuilder("class ").Append(Name ?? throw new NullReferenceException());
        return InheritedClassname.Length == 0
            ? builder.Append(" {").ToString()
            : builder.Append(" : ").Append(InheritedClassname).Append(" {").ToString();
    }
}