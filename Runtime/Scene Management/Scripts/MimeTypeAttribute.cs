public class MimeTypeAttribute : System.Attribute
{
    public string[] Values { get; private set; }
    public MimeTypeAttribute(params string[] MimeTypes)
    {
        this.Values = MimeTypes;
    }
}
