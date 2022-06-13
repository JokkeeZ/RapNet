namespace RapNet.ValueTypes; 

public class RapVariableValue : BaseRapValue<string> {
    public RapVariableValue(string varName) => Value = varName;
    public override string ToConfigFormat() => Value;
}