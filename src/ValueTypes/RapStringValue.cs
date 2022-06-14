namespace RapNet.ValueTypes;

internal sealed class RapStringValue : BaseRapValue<string> {
    internal RapStringValue(string str) => Value = str;
    private string Quoted => $"\"{Value}\"";
    public override string ToConfigFormat() => Quoted;

}