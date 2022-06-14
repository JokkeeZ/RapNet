using System;
using System.Collections.Generic;

using RapNet.EntryTypes;
using RapNet.Enums;

namespace RapNet;
/// <summary>
/// Represents collection of valid raP entries.
/// </summary>
internal class RapEntryFactory
{
    /// <summary>
    /// Gets a new instance of the IBinarizedRapEntry object defined by type param.
    /// </summary>
    /// <param name="type">Defines which type of IBinarizedRapEntry is returned.</param>
    /// <returns>Returns a new instance of the IBinarizedRapEntry object defined by type param.</returns>
    internal static IBinarizedRapEntry CreateEntryForType(RapEntryType type) => type switch 
    {
        RapEntryType.RapClass => new RapClass(),
        RapEntryType.RapValue => new RapValue(),
        RapEntryType.RapArray => new RapValue() {SubType = RapValueType.Array},
        RapEntryType.RapExternClass => new RapExtern(),
        RapEntryType.RapDeleteClass => new RapDelete(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}
