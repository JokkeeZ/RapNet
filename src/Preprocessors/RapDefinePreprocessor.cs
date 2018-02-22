using RapNet.EntryTypes;

namespace RapNet.Preprocessors
{
    /// <summary>
    /// Represents a config preprocessor define. (#define [name] [value])
    /// </summary>
    public class RapDefinePreprocessor : IRapEntry
    {
        /// <summary>
        /// Preprocessor name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Preprocessor value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the RapDefinePreprocessor class, with name and value.
        /// </summary>
        /// <param name="name">Preprocessor name.</param>
        /// <param name="value">Preprocessor value.</param>
        public RapDefinePreprocessor(string name, int value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Converts object to final config format.
        /// </summary>
        /// <returns>Returns object as final config format.</returns>
        public string ToConfigFormat() => $"#define { Name }  { Value }";
    }
}
