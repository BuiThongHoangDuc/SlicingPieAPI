using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace SlicingPieAPI.DTOs
{
    public class SupportSelectField
    {
        public Object getByField(Object cur_object, string select_field_str)
        {
            var allPropertiesContract = new SelectiveSerializer(select_field_str);

            var allJson = JsonConvert.SerializeObject(
                cur_object,
                Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = allPropertiesContract });
            var a = JsonConvert.DeserializeObject(allJson);
            return a;
        }

        public class SelectiveSerializer : DefaultContractResolver
        {
            private readonly string[] _fields;

            public SelectiveSerializer(string fields)
            {
                string[] fieldColl = fields.Split(',');
                _fields = fieldColl.Select(f => f.ToLower().Trim()).ToArray();
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                
                var property = base.CreateProperty(member, memberSerialization);
                property.ShouldSerialize = o => _fields.Contains(member.Name.ToLower());

                return property;
            }
        }
    }
}
