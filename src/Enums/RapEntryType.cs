namespace RapNet.Enums
{
    /// <summary>
    /// Represents raP entry type.
    /// </summary>
    public enum RapEntryType : byte
    {
        /// <summary>
        /// EntryType 0: RapClass
        /// </summary>
        RapClass = 0,

        /// <summary>
        /// EntryType 1: Value Eq
        /// </summary>
        RapValue = 1,

        /// <summary>
        /// EntryType 2: array[]
        /// </summary>
        RapArray = 2,

        /// <summary>
        /// EntryType 3: ExternClass
        /// </summary>
        RapExternClass = 3,

        /// <summary>
        /// EntryType 4: DeleteClass
        /// </summary>
        RapDeleteClass = 4
    }
}
