using System;

namespace QMapRead
{
    /// <summary>
    /// Gets thrown when a Quake/Valve map file is not formatted correctly
    /// while parsing.
    /// </summary>
    public class MapFormatException : Exception
    {
        public MapFormatException()
        {

        }

        public MapFormatException(string message) : base(message)
        {

        }

        public MapFormatException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}