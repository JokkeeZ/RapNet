using System.Collections.Generic;
using System.Linq;
using System.Text;
using RapNet.EntryTypes;

namespace RapNet.ValueTypes; 

public class RapArrayValue : BaseRapValue<List<IRapEntry>> {
    public int EntryCount { get; set; }
    public List<IRapEntry> Value { get; set; } = new ();
    public override string ToConfigFormat() => 
        new StringBuilder().Append('{')
            .Append(string.Join(',', Value.Select(v => v.ToConfigFormat())))
            .Append('}').ToString();
}