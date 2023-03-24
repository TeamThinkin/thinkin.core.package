using Newtonsoft.Json;
using System.Collections;
using UnityEngine;

public struct DocumentMetaInformation
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("documentUrl")]
    public string DocumentUrl { get; set; }

    [JsonProperty("iconUrl")]
    public string IconUrl { get; set; }
}
