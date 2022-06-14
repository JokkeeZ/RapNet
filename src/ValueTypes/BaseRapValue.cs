using RapNet.EntryTypes;

namespace RapNet.ValueTypes;

internal abstract class BaseRapValue<T> : IRapEntry {
    protected T? Value { get; init; }
    public abstract string ToConfigFormat();
}