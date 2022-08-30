using System;
using Thinkin.Web;

public struct AssetUrl
{
    public Uri Parsed;
    public string CatalogUrl;
    public string AssetPath;

    public AssetUrl(string url)
    {
        Parsed = new Uri(url.Replace("{PLATFORM}", Config.PlatformKey));
        CatalogUrl = $"{Parsed.Scheme}://{Parsed.Authority}{Parsed.PathAndQuery}";
        //AssetPath = WebSocketSharp.Net.HttpUtility.UrlDecode(uri.Fragment.Substring(1));
        if (Parsed.Fragment != null && Parsed.Fragment.Length > 1)
            AssetPath = HttpUtility.UrlDecode(Parsed.Fragment.Substring(1));
        else
            AssetPath = "";
    }
}
