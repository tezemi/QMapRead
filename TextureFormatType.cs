
namespace QMapRead
{
    /// <summary>
    /// The type of texture info formatting used by a map file. Quake uses
    /// a single Vec2 to represent offset. Valve has two textures, which
    /// use a Vec3 and a float to represent coordinates and offset.
    /// </summary>
    public enum TextureFormatType
    {
        Quake,
        Valve
    }
}
