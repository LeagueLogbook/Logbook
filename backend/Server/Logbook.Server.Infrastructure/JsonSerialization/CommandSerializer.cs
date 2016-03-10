using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel.Collections;
using Logbook.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate.Proxy;

namespace Logbook.Server.Infrastructure.JsonSerialization
{
    public static class CommandSerializer
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new IgnoreSecuredPropertiesContractResolver(),
            Formatting = Formatting.Indented,
            Converters =
            {
                new IgnoreNHibernateProxiesConverter()
            }
        };
        
        public static string ToJson(object command)
        {
            return JsonConvert.SerializeObject(command, _serializerSettings);
        }

        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
        }

        private class IgnoreSecuredPropertiesContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);

                if (member.GetCustomAttributes<SecureAttribute>().Any())
                {
                    property.ShouldSerialize = instance => false;
                }

                return property;
            }
        }

        private class IgnoreNHibernateProxiesConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteNull();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override bool CanConvert(Type objectType)
            {
                return typeof (INHibernateProxy).IsAssignableFrom(objectType);
            }
        }
    }
}