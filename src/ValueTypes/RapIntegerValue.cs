namespace RapNet.ValueTypes; 

public class RapIntegerValue : BaseRapValue<uint> {
    public RapIntegerValue(uint val) => Value = val;
    public RapIntegerValue(int val) => Value = unchecked((uint) val);
    public override string ToConfigFormat() => Value.ToString();
}