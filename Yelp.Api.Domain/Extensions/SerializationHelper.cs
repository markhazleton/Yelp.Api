using System.Runtime.Serialization.Json;
using System.Text;

namespace Yelp.Api.Domain.Extensions;


public static class SerializationHelper
{
    /// <summary> Deserialized Json string of type T. </summary>
    public static T DeserializeJsonString<T>(string jsonString)
    {
        T tempObject = default(T);

        using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            tempObject = (T)serializer.ReadObject(memoryStream);
        }

        return tempObject;
    }
}