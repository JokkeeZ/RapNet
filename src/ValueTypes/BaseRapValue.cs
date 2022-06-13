using RapNet.EntryTypes;

namespace RapNet.ValueTypes; 

public abstract class BaseRapValue<T> : IRapEntry {
    public T Value { get; set; }
    public abstract string ToConfigFormat();
}