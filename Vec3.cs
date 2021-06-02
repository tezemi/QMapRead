
namespace QMapRead
{
    public struct Vec3
    {
        public float X;
        public float Y;
        public float Z;

        public static readonly Vec3 Forward = new Vec3(0f, 0f, 1f);
        public static readonly Vec3 Back = new Vec3(0f, 0f, -1f);
        public static readonly Vec3 Down = new Vec3(0f, -1f, 0f);
        public static readonly Vec3 Left = new Vec3(-1f, 0f, 0f);
        public static readonly Vec3 Right = new Vec3(1f, 0f, 0f);
        public static readonly Vec3 Up = new Vec3(0f, 1f, 0f);

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = z;
            Z = y;
        }

        public float Dot(Vec3 other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        public static float Dot(Vec3 a, Vec3 b)
        {
            return a.Dot(b);
        }

        public Vec3 Cross(Vec3 other)
        {
            var x = Y * other.Z - other.Y * Z;
            var y = Z * other.X - other.Z * X;
            var z = X * other.Y - other.X * Y;

            return new Vec3(x, y, z);
        }

        public bool Equals(Vec3 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            return obj is Vec3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();

                return hashCode;
            }
        }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3 operator *(Vec3 a, float b)
        {
            return new Vec3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vec3 operator *(float a, Vec3 b)
        {
            return new Vec3(b.X * a, b.Y * a, b.Z * a);
        }

        public static Vec3 operator /(Vec3 a, float b)
        {
            return new Vec3(a.X / b, a.Y / b, a.Z / b);
        }

        public static bool operator ==(Vec3 a, Vec3 b)
        {
            if (a.X == b.X && a.Y == b.Y && a.Z == b.Z)
            {
                return true;
            }

            return false;
        }


        public static bool operator !=(Vec3 a, Vec3 b)
        {
            if (a.Equals(b))
            {
                return false;
            }

            return true;
        }
    }
}
