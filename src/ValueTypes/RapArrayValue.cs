using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RapNet.EntryTypes;

namespace RapNet.ValueTypes;

internal sealed class RapArrayValue : BaseRapValue<List<IRapEntry>> {
    internal int EntryCount { get; init; }
    internal new List<IRapEntry>? Value { get; set; } = new ();

    public override string ToConfigFormat() {
        var value = Value ?? throw new NullReferenceException();
        return new StringBuilder().Append('{')
            .Append(string.Join(',', Value.Select(static v => v.ToConfigFormat())))
            .Append('}').ToString().Replace("{{", "{").Replace("}}", "}");
    }
        
}