namespace RapNet.ValueTypes; 

public class RapFloatValue : BaseRapValue<float> {
    public RapFloatValue(float val) => Value = val;
    public override string ToConfigFormat() => Value.ToString();
}