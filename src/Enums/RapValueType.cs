namespace RapNet.Enums;
/// <summary>
/// Represents raP value type.
/// </summary>
internal enum RapValueType : byte
{
    /// <summary>
    /// Type ID: 0, Value: AsciiZ string.
    /// </summary>
    String = 0,

    /// <summary>
    /// Type ID: 1, Value: 4-byte float.
    /// </summary>
    Float = 1,

    /// <summary>
    /// Type ID: 2, Value: 4-byte long. (UInt32)
    /// </summary>
    Long = 2,

    /// <summary>
    /// Type ID: 3, Value: RapArray. (not used)
    /// </summary>
    Array = 3,

    /// <summary>
    /// Type ID: 4, Value: public or private variable. (XBOX only)
    /// </summary>
    Variable = 4
}
