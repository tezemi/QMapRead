namespace QMapRead
{
    public struct Vec2
    {
        public float X;
        public float Y;

        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Vec2 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Vec2 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();

                return hashCode;
            }
        }

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X - b.X, a.Y - b.Y);
        }

        public static bool operator ==(Vec2 a, Vec2 b)
        {
            if (a.X == b.X && a.Y == b.Y)
            {
                return true;
            }

            return false;
        }


        public static bool operator !=(Vec2 a, Vec2 b)
        {
            if (a.Equals(b))
            {
                return false;
            }

            return true;
        }
    }
}