using FSH.Core.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace FSH.Infrastructure.Serialization;

public class NewtonSoftSerializationService : ISerializationService
{
    public T Deserialize<T>(string text)
    {
        return JsonConvert.DeserializeObject<T>(text);
    }

    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter() {NamingStrategy =  new CamelCaseNamingStrategy()}
            }
        });
    }

    public string Serialize<T>(T obj, Type type)
    {
        return JsonConvert.SerializeObject(obj, type, new());
    }
}
