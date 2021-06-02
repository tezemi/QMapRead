using System.Collections.Generic;

namespace QMapRead
{
    /// <summary>
    /// An Entity from a Quake map file. Entities can have two things,
    /// key-value properties, or brushes.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// A collection of all the properties associated with this entity.
        /// </summary>
        public readonly List<Property> Properties = new List<Property>();
        /// <summary>
        /// A collection of every brush this entity has.
        /// </summary>
        public readonly List<Brush> Brushes = new List<Brush>();
    }
}