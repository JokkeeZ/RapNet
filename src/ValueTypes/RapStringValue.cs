namespace RapNet.ValueTypes; 

public class RapStringValue : BaseRapValue<string> {
    public RapStringValue(string str) => Value = str;
    private string Quoted => $"\"{Value}\"";
    public override string ToConfigFormat() => Quoted;

}