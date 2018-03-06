using System.Collections.Generic;
using System.Text;

namespace RapNet.Preprocessors
{
    /// <summary>
    /// Represents '#define [name] [value]'
    /// </summary>
    public class ConfigDefinePreprocessors
    {
        private static readonly List<RapDefinePreprocessor> _defines = new List<RapDefinePreprocessor>();

        /// <summary>
        /// Adds defines to the start of the config.
        /// </summary>
        /// <param name="defines">Defines to be added.</param>
        public static void AddDefines(List<RapDefinePreprocessor> defines)
        {
            foreach (var define in defines) {
                if (!_defines.Contains(define)) {
                    _defines.Add(define);
                }
            }
        }

        /// <summary>
        /// Gets all defines as string.
        /// </summary>
        /// <returns>Returns raP entries as string.</returns>
        public static string GetDefines()
        {
            var sb = new StringBuilder();
            _defines.ForEach(o => sb.AppendLine(o.ToConfigFormat()));

            return sb.ToString();
        }
    }
}
