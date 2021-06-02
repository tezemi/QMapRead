
namespace QMapRead
{
    public struct Property
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Property(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}