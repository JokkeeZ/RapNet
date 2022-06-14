using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapNet.Preprocessors;

/// <summary>
/// Represents '#define [name] [value]'
/// </summary>
internal static class ConfigDefinePreprocessors
{
    private static readonly List<RapDefinePreprocessor> Defines = new();

    /// <summary>
    /// Adds defines to the start of the config.
    /// </summary>
    /// <param name="defines">Defines to be added.</param>
    public static void AddDefines(IEnumerable<RapDefinePreprocessor> defines) {
        var defs = Defines ?? throw new NullReferenceException();
        foreach (var define in defines.Where(o => !defs.Contains(o))) defs.Add(define);
    }

    /// <summary>
    /// Gets all defines as string.
    /// </summary>
    /// <returns>Returns raP entries as string.</returns>
    public static string GetDefines()
    {
        var sb = new StringBuilder();
        Defines.ForEach(o => sb.AppendLine(o.ToConfigFormat()));

        return sb.ToString();
    }
}
