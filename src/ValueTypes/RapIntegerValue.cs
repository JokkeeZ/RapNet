namespace RapNet.ValueTypes;

internal sealed class RapIntegerValue : BaseRapValue<uint> {
    internal RapIntegerValue(uint val) => Value = val;
    internal RapIntegerValue(int val) => Value = unchecked((uint) val);
    public override string ToConfigFormat() => Value.ToString();
}