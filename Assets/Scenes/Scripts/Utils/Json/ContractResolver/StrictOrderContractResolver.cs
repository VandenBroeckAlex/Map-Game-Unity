using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

public class StrictOrderContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        // Get all properties (including base and derived)
        var props = base.CreateProperties(type, memberSerialization);

        // Sort by Order first (default = int.MaxValue), then by declaration order
        return props
            .OrderBy(p => p.Order ?? int.MaxValue)
            .ThenBy(p => p.DeclaringType?.BaseType == null ? 0 : 1) // keep base before derived
            .ToList();
    }
}
