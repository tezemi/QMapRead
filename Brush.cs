using System.Collections.Generic;

namespace QMapRead
{
    /// <summary>
    /// Represents a brush from a Quake map file, which is just a
    /// collection of Faces.
    /// </summary>
    public class Brush
    {
        /// <summary>
        /// The Faces that are a part of this Brush.
        /// </summary>
        public readonly List<Face> Faces = new List<Face>();

        public IEnumerable<Vec3> ConstructVertices()
        {
            var hashSet = new HashSet<Vec3>();

            foreach (var face in Faces)
            {
                if (!hashSet.Contains(face.Plane.Vec1))
                {
                    hashSet.Add(face.Plane.Vec1);
                }

                if (!hashSet.Contains(face.Plane.Vec2))
                {
                    hashSet.Add(face.Plane.Vec2);
                }

                if (!hashSet.Contains(face.Plane.Vec1))
                {
                    hashSet.Add(face.Plane.Vec3);
                }
            }

            return hashSet;
        }
    }
}