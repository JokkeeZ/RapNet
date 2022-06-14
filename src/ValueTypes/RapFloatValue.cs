using System.Globalization;

namespace RapNet.ValueTypes;

internal sealed class RapFloatValue : BaseRapValue<float> {
    internal RapFloatValue(float val) => Value = val;
    public override string ToConfigFormat() => Value.ToString(CultureInfo.CurrentCulture);
}