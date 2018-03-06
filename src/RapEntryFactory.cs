using System.Collections.Generic;

using RapNet.EntryTypes;
using RapNet.Enums;

namespace RapNet
{
    /// <summary>
    /// Represents collection of valid raP entries.
    /// </summary>
    public class RapEntryFactory
    {
        /// <summary>
        /// Gets a new instance of the IBinarizedRapEntry object defined by type param.
        /// </summary>
        /// <param name="type">Defines which type of IBinarizedRapEntry is returned.</param>
        /// <returns>Returns a new instance of the IBinarizedRapEntry object defined by type param.</returns>
        public static IBinarizedRapEntry CreateEntryForType(RapEntryType type)
        {
            switch (type) {
                case RapEntryType.RapClass:
                return new RapClass();

                case RapEntryType.RapValue:
                return new RapValue();

                case RapEntryType.RapArray:
                return new RapArray();

                case RapEntryType.RapExternClass:
                return new RapExtern();

                case RapEntryType.RapDeleteClass:
                return new RapDelete();
            }

            return null;
        }
    }
}
