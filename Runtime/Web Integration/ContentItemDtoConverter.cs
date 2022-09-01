using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


public class ContentItemDtoConverter : JsonConverter<CollectionContentItemDto>
{
    static Dictionary<string, Type> contentTypes;

    public ContentItemDtoConverter()
    {
        if (contentTypes == null)
        {
            var ass = Assembly.GetExecutingAssembly();
            var list = ass.GetTypes().Select(i => new { Type = i, Attribute = i.GetCustomAttribute<MimeTypeAttribute>() });
            contentTypes = new Dictionary<string, Type>();
            foreach (var item in list)
            {
                if (item.Attribute != null)
                {
                    foreach (var mimeType in item.Attribute.Values)
                    {
                        contentTypes.Add(mimeType, item.Type);
                    }
                }
            }
        }
    }

    public override CollectionContentItemDto ReadJson(JsonReader reader, System.Type objectType, CollectionContentItemDto existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject item = JObject.Load(reader);
        var mimeType = (string)item["mimeType"];
        if (contentTypes.ContainsKey(mimeType))
        {
            var type = contentTypes[mimeType];
            var result = item.ToObject(type) as CollectionContentItemDto;
            return result;
        }
        else
        {
            Debug.LogError("Unrecognized mimeType when reading json: " + mimeType);
            return null;
        }
    }

    public override void WriteJson(JsonWriter writer, CollectionContentItemDto value, JsonSerializer serializer)
    {
        throw new System.NotImplementedException();
    }
}