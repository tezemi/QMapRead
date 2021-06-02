
namespace QMapRead
{
    /// <summary>
    /// A Face is part of a Brush. Faces are defined by a Plane, as well as
    /// texture information. There are two formats for texture info, the
    /// Quake format, which stores the offset of the texture as a Vec2, and
    /// the Valve format, which stores the offset as two groups of Vec3s
    /// and offsets.
    /// </summary>
    public struct Face
    {
        /// <summary>
        /// The Plane that represents the geometry of this Face.
        /// </summary>
        public Plane Plane { get; set; }
        /// <summary>
        /// The path to the texture to use for this face.
        /// </summary>
        public string TexturePath { get; set; }
        /// <summary>
        /// The rotation of the texture.
        /// </summary>
        public float Rotation { get; set; }
        /// <summary>
        /// The scale of the texture.
        /// </summary>
        public Vec2 Scale { get; set; }
        /// <summary>
        /// The type of format the texture information is in. If in Quake
        /// format, the Valve format fields will be unset, and vice-versa.
        /// </summary>
        public TextureFormatType TextureFormat { get; set; }

        // Quake Format
        /// <summary>
        /// The offset of the texture if in Quake format. Otherwise, this is
        /// unset.
        /// </summary>
        public Vec2 Offset { get; set; }

        // Valve Format
        /// <summary>
        /// The first texture coordinates if using Valve format, otherwise unset.
        /// </summary>
        public Vec3 ValveTex1 { get; set; }
        /// <summary>
        /// The first texture offset if using Valve format, otherwise unset.
        /// </summary>
        public float ValveOffset1 { get; set; }
        /// <summary>
        /// The second texture coordinates if using Valve format, otherwise unset.
        /// </summary>
        public Vec3 ValveTex2 { get; set; }
        /// <summary>
        /// The second texture offset if using Valve format, otherwise unset.
        /// </summary>
        public float ValveOffset2 { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Plane.GetHashCode();
                hashCode = (hashCode * 397) ^ (TexturePath != null ? TexturePath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Rotation.GetHashCode();
                hashCode = (hashCode * 397) ^ Scale.GetHashCode();
                hashCode = (hashCode * 397) ^ Offset.GetHashCode();
                hashCode = (hashCode * 397) ^ ValveTex1.GetHashCode();
                hashCode = (hashCode * 397) ^ ValveOffset1.GetHashCode();
                hashCode = (hashCode * 397) ^ ValveTex2.GetHashCode();
                hashCode = (hashCode * 397) ^ ValveOffset2.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(Face other)
        {
            return Plane.Equals(other.Plane) && TexturePath == other.TexturePath && Rotation.Equals(other.Rotation) && 
                   Scale.Equals(other.Scale) && TextureFormat == other.TextureFormat && Offset.Equals(other.Offset) && 
                   ValveTex1.Equals(other.ValveTex1) && ValveOffset1.Equals(other.ValveOffset1) && 
                   ValveTex2.Equals(other.ValveTex2) && ValveOffset2.Equals(other.ValveOffset2);
        }

        public override bool Equals(object obj)
        {
            return obj is Face other && Equals(other);
        }

        public static bool operator ==(Face a, Face b)
        {
            if (a.Plane == b.Plane && a.TexturePath == b.TexturePath && a.Scale == b.Scale && a.Rotation == b.Rotation)
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(Face a, Face b)
        {
            if (a.Equals(b))
            {
                return false;
            }

            return true;
        }
    }
}