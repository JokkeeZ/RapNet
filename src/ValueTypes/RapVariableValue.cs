using System;

namespace RapNet.ValueTypes;

internal sealed class RapVariableValue : BaseRapValue<string> {
    internal RapVariableValue(string varName) => Value = varName;
    public override string ToConfigFormat() => Value ?? throw new NullReferenceException();
}