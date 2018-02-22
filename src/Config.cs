using System.Collections.Generic;

using RapNet.EntryTypes;

namespace RapNet
{
    /// <summary>
    /// Represents a raPified file that is being readed.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// RapClass entries count.
        /// </summary>
        public int Entries { get; set; }

        /// <summary>
        /// Classes inside this file.
        /// </summary>
        public List<RapClass> Classes { get; set; } = new List<RapClass>();

        /// <summary>
        /// Enums inside this file.
        /// </summary>
        public List<RapEnum> Enums { get; set; } = new List<RapEnum>();
    }
}
