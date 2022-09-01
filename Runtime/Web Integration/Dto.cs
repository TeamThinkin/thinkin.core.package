using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMapDestinationDto
{
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("environment")]
    public EnvironmentContentItemDto Environment { get; set; }

    [JsonProperty("placement")]
    public PlacementDto Placement { get; set; }
}

public class RegisterDeviceRequestDto
{
    [JsonProperty("uid")]
    public string Uid { get; set; }

    [JsonProperty("mac")]
    public string Mac { get; set; }
}

public class RegisterDeviceResultDto : UserDto
{
    //[JsonProperty("domains")]
    //public DomainDto[] Domains { get; set; }
}

public class UserDto
{ 
    [JsonProperty("_id")]
    public string Id { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("hasPassword")]
    public bool HasPassword { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("avatarUrl")]
    public string AvatarUrl { get; set; }

    [JsonProperty("homeRoomUrl")]
    public string HomeRoomUrl { get; set; }

    [JsonProperty("currentRoomUrl")]
    public string CurrentRoomUrl { get; set; }
}


public class CollectionContentItemDto
{
    [JsonProperty("_id")]
    public string Id { get; set; }

    [JsonProperty("mimeType")]
    public string MimeType { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("tags")]
    public string[] Tags { get; set; }

    // -- Additional Properties not present in JSON --
    public string CollectionUrl { get; set; }
    /// ///////////////////////

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(DisplayName))
            return DisplayName + " (" + MimeType + ": " + Id + ")";
        else
            return base.ToString();
    }
}

public class FileContentItemDto : CollectionContentItemDto
{
    [JsonProperty("url")]
    public string Url { get; set; }

    public override bool Equals(object obj)
    {
        var other = obj as FileContentItemDto;
        if (other == null) return false;
        return this.Url == other.Url;
    }

    public override int GetHashCode()
    {
        return Url != null ? Url.GetHashCode() : base.GetHashCode();
    }
}

public class RegistryEntryDto
{
    [JsonProperty("_id")]
    public string Id { get; set; }

    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("type")]
    public string CollectionType { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
}

[MimeType("environment/unity.assetbundle")]
public class EnvironmentContentItemDto : FileContentItemDto
{
}

[MimeType("environment/local")]
public class LocalSceneContentItemDto : CollectionContentItemDto
{
    [JsonProperty("path")]
    public string Path { get; set; }
}

public class PlacementDto
{
    [JsonProperty("position")]
    public Vector3Dto Position { get; set; }
    
    [JsonProperty("scale")]
    public float Scale { get; set; }

    [JsonProperty("size")]
    public Vector3Dto Size { get; set; }

    [JsonProperty("rotation")]
    public Vector4Dto Rotation { get; set; }

    public Vector3 ScaleSized
    {
        get
        {
            if (Size != null)
                return (Vector3)Size * Scale;
            else
                return Vector3.one * Scale;
        }
    }
}

[MimeType("image/jpg", "image/jpeg", "image/png")]
public class ImageContentItemDto : FileContentItemDto
{
    [JsonProperty("corsFriendly")]
    public bool IsCorsFriendly { get; set; }
}

[MimeType("link/destination")]
public class DestinationLinkContentItemDto : FileContentItemDto
{
    [JsonProperty("placement")]
    public PlacementDto Placement { get; set; }
}

[MimeType("link/room")]
public class RoomLinkContentItemDto : FileContentItemDto
{
    [JsonProperty("placement")]
    public PlacementDto Placement { get; set; }

    [JsonProperty("displayType")]
    public RoomDisplayTypeEnum DisplayType { get; set; }
}

public enum RoomDisplayTypeEnum
{
    Default,
    Hidden
}

[MimeType("layout/3d")]
public class Layout3dContentItemDto : FileContentItemDto
{
}

[MimeType("layout-data/absolute")]
public class AbsoluteLayoutDataContentItemDto : CollectionContentItemDto
{
    [JsonProperty("parentKey")]
    public string ParentKey { get; set; }

    [JsonProperty("itemKey")]
    public string ItemKey { get; set; }

    [JsonProperty("placement")]
    public PlacementDto Placement { get; set; }
}

[MimeType("link/collection")]
public class CollectionLinkContentItemDto : FileContentItemDto
{
}

[MimeType("item-info/transform")]
public class ItemTransformDto : CollectionContentItemDto
{
    [JsonProperty("itemId")]
    public string ItemId { get; set; }

    [JsonProperty("position")]
    public Vector3Dto Position { get; set; }

    [JsonProperty("rotation")]
    public Vector4Dto Rotation{ get; set; }

    [JsonProperty("scale")]
    public Vector3Dto Scale { get; set; }
}

public class Vector3Dto
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public override string ToString()
    {
        return x.ToString("0.00") + ", " + y.ToString("0.00") + ", " + z.ToString("0.00");
    }

    public static implicit operator Vector3(Vector3Dto dto)
    {
        return dto != null ? new Vector3(dto.x, dto.y, dto.z) : Vector3.zero;
    }

    public static implicit operator Vector3Dto(Vector3 vector)
    {
        return new Vector3Dto() { x = vector.x, y = vector.y, z = vector.z };
    }
}
public class Vector4Dto
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public float w { get; set; }

    public override string ToString()
    {
        return x.ToString("0.00") + ", " + y.ToString("0.00") + ", " + z.ToString("0.00") + ", " + w.ToString("0.00");
    }

    public static implicit operator Quaternion(Vector4Dto dto)
    {
        return dto != null ? new Quaternion(dto.x, dto.y, dto.z, dto.w) : Quaternion.identity;
    }

    public static implicit operator Vector4Dto(Quaternion q)
    {
        return new Vector4Dto() { x = q.x, y = q.y, z = q.z, w = q.w };
    }
}
