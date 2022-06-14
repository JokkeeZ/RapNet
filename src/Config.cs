using System.Collections.Generic;

using RapNet.EntryTypes;

namespace RapNet;
/// <summary>
/// Represents a raPified file that is being readed.
/// </summary>
internal sealed class Config
{
    /// <summary>
    /// RapClass entries count.
    /// </summary>
    internal int Entries { get; set; }

    /// <summary>
    /// Classes inside this file.
    /// </summary>
    internal List<RapClass> Classes { get; set; } = new();

    /// <summary>
    /// Enums inside this file.
    /// </summary>
    internal List<RapEnum> Enums { get; set; } = new();
}