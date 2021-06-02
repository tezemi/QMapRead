
namespace QMapRead
{
    public struct Plane
    {
        public Vec3 Vec1;
        public Vec3 Vec2;
        public Vec3 Vec3;

        public Plane(Vec3 vec1, Vec3 vec2, Vec3 vec3)
        {
            Vec1 = vec1;
            Vec2 = vec2;
            Vec3 = vec3;
        }

        public bool Equals(Plane other)
        {
            return Vec1.Equals(other.Vec1) && Vec2.Equals(other.Vec2) && Vec3.Equals(other.Vec3);
        }

        public override bool Equals(object obj)
        {
            return obj is Plane other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Vec1.GetHashCode();
                hashCode = (hashCode * 397) ^ Vec2.GetHashCode();
                hashCode = (hashCode * 397) ^ Vec3.GetHashCode();

                return hashCode;
            }
        }

        public static bool operator ==(Plane a, Plane b)
        {
            if (a.Vec1 == b.Vec1 && a.Vec2 == b.Vec2 && a.Vec3 == b.Vec3)
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(Plane a, Plane b)
        {
            if (a.Equals(b))
            {
                return false;
            }

            return true;
        }
    }
}
