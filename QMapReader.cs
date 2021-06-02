using System;
using System.IO;

namespace QMapRead
{
    /// <summary>
    /// Static class used to read Quake/Valve formatted map files.
    /// </summary>
    public static class QMapReader
    {
        /// <summary>
        /// Reads the map file at the specified path, returning it as an instance
        /// of a Map object.
        /// </summary>
        /// <param name="mapPath">The path to the map file to read.</param>
        /// <returns>Details about the map file, all part of a single Map object.
        /// </returns>
        public static Map Read(string mapPath)
        {
            if (!File.Exists(mapPath))
            {
                throw new ArgumentException("There is no file at the specified path.", mapPath);
            }

            using var stream = File.Open(mapPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var mapFile = new Map(mapPath);

            int b;
            while ((b = stream.ReadByte()) != -1)
            {
                var c = (char)b;

                CheckAndSkipComment(ref c, stream);

                if (c == Constants.LEFT_BRACKET)
                {
                    mapFile.Entities.Add(ParseEntity(stream));
                }
                else if (c != Constants.SPACE && c != Constants.LF && c != Constants.CR)
                {
                    throw new MapFormatException($"Could not parse the map, encountered an unexpected symbol '{c}' at position '{stream.Position}' while looking for an entity.");
                }
            }

            return mapFile;
        }

        /// <summary>
        /// Parses the next entity found in the stream, and returns it.
        /// </summary>
        /// <param name="stream">The stream to read the entity in. The
        /// cursor is moved forward after the parse.</param>
        /// <returns>The entity parsed in the file.</returns>
        private static Entity ParseEntity(FileStream stream)
        {
            var entity = new Entity();

            int b;
            while ((b = stream.ReadByte()) != -1)
            {
                var c = (char)b;

                CheckAndSkipComment(ref c, stream);

                if (c == Constants.RIGHT_BRACKET)
                {
                    break;
                }

                if (c == Constants.QUOTE)               // If quote, read a property
                {
                    entity.Properties.Add(ParseProperty(stream));
                }
                else if (c == Constants.LEFT_BRACKET)   // If another left bracket, read a brush
                {
                    entity.Brushes.Add(ParseBrush(stream));
                }
            }

            return entity;
        }

        /// <summary>
        /// Parses the next property found in the stream, and returns it.
        /// </summary>
        /// <param name="stream">The stream to read the property in. The
        /// cursor is moved forward after the parse.</param>
        /// <returns>The property parsed in the file.</returns>
        private static Property ParseProperty(FileStream stream)
        {
            var readKey = true;
            var readValue = false;

            int b;
            var key = string.Empty;
            var value = string.Empty;
            while ((b = stream.ReadByte()) != -1)
            {
                var c = (char)b;

                if (!readKey && !readValue && c != Constants.QUOTE && c != Constants.SPACE)
                {
                    throw new MapFormatException($"Could not parse property, encountered an unexpected symbol '{c}' at position '{stream.Position}'.");
                }

                if (readKey)
                {
                    if (c == Constants.QUOTE)
                    {
                        readKey = false;
                        continue;
                    }

                    key += c;
                }
                else if (!readValue && c == Constants.QUOTE)
                {
                    readValue = true;
                }
                else if (readValue)
                {
                    if (c == Constants.QUOTE)
                    {
                        break;
                    }

                    value += c;
                }
            }

            return new Property(key, value);
        }

        /// <summary>
        /// Parses the next brush found in the stream, and returns it.
        /// </summary>
        /// <param name="stream">The stream to read the brush in. The
        /// cursor is moved forward after the parse.</param>
        /// <returns>The brush parsed in the file.</returns>
        private static Brush ParseBrush(FileStream stream)
        {
            var brush = new Brush();

            int b;
            while ((b = stream.ReadByte()) != -1)
            {
                var c = (char)b;

                CheckAndSkipComment(ref c, stream);

                if (c == Constants.LEFT_PARENTHESIS)
                {
                    brush.Faces.Add(ParseFace(stream));
                }
                else if (c == Constants.RIGHT_BRACKET)
                {
                    return brush;
                }
                else if (!char.IsWhiteSpace(c) && c != Constants.RIGHT_SQUARE_BRACKET && c != Constants.LEFT_SQUARE_BRACKET)
                {
                    throw new MapFormatException($"Could not parse a brush, encountered an unexpected symbol '{c}' at position '{stream.Position}'.");
                }
            }

            return brush;
        }

        /// <summary>
        /// Parses the next face found in the brush, and returns it.
        /// </summary>
        /// <param name="stream">The stream to read the face in. The
        /// cursor is moved forward after the parse.</param>
        /// <returns>The face parsed in the file.</returns>
        private static Face ParseFace(FileStream stream)
        {
            var face = new Face();
            var plane = new Vec3[3];

            var parsingPlane = true;
            var planeIndex = 0;

            var parsingTexture = false;
            var textureString = string.Empty;

            var quakeFormat = false;
            var valveFormat = false;

            char c;
            while ((c = (char)stream.ReadByte()) > 0)
            {
                CheckAndSkipComment(ref c, stream);

                if (parsingPlane)
                {
                    plane[planeIndex] = ParseVector3(stream);
                    planeIndex++;

                    if (planeIndex == plane.Length)
                    {
                        face.Plane = new Plane(plane[0], plane[1], plane[2]);

                        parsingPlane = false;

                        parsingTexture = true;

                        stream.ReadByte();
                    }
                }
                else
                {
                    if (c == Constants.SPACE && !string.IsNullOrEmpty(textureString))
                    {
                        face.TexturePath = textureString;

                        parsingTexture = false;
                    }

                    if (c != Constants.SPACE && parsingTexture)
                        textureString += c;
                }

                if (!parsingPlane && !parsingTexture)
                {
                    if (c == Constants.SPACE)
                    {
                        if (LookForward(stream) == Constants.LEFT_SQUARE_BRACKET)
                        {
                            valveFormat = true;
                        }
                        else if (char.IsDigit(LookForward(stream)))
                        {
                            quakeFormat = true;
                        }
                    }

                    if (quakeFormat)
                    {
                        face.TextureFormat = TextureFormatType.Quake;
                        face.Offset = ParseVector2(stream);
                        face.Rotation = ParseFloat(stream);
                        face.Scale = ParseVector2(stream);

                        while ((char)stream.ReadByte() != Constants.LF) { }

                        return face;
                    }

                    if (valveFormat)
                    {
                        face.TextureFormat = TextureFormatType.Valve;
                        face.ValveTex1 = ParseVector3(stream);
                        face.ValveOffset1 = ParseFloat(stream);
                        face.ValveTex2 = ParseVector3(stream);
                        face.ValveOffset2 = ParseFloat(stream);
                        face.Rotation = ParseFloat(stream);
                        face.Scale = ParseVector2(stream);

                        return face;
                    }
                }
            }

            return face;
        }

        /// <summary>
        /// Parses the next number, as a float in the stream, and returns it.
        /// </summary>
        /// <param name="stream">The stream to read the float in. The
        /// cursor is moved forward after the parse.</param>
        /// <returns>The float parsed in the file.</returns>
        private static float ParseFloat(FileStream stream)
        {
            int b;

            var readingNumber = false;
            var currentNumberString = string.Empty;

            while ((b = stream.ReadByte()) != -1)
            {
                var c = (char)b;

                if (!readingNumber && (c == Constants.NEGATIVE_SIGN || char.IsDigit(c)))
                {
                    readingNumber = true;
                }

                if (readingNumber)
                {
                    if (c == Constants.NEGATIVE_SIGN || char.IsDigit(c) || c == Constants.PERIOD)
                    {
                        currentNumberString += c;
                    }
                    else
                    {
                        if (float.TryParse(currentNumberString, out var f))
                        {
                            return f;
                        }

                        throw new MapFormatException($"Could not parse float, tried to parse '{currentNumberString}' at position '{stream.Position - currentNumberString.Length}'.");
                    }
                }
            }

            throw new MapFormatException($"Could not parse float, tried to parse '{currentNumberString}' at position '{stream.Position - currentNumberString.Length}'.");
        }

        /// <summary>
        /// Parses the next Vec2 found in the stream, by parsing two floats,
        /// and returns it.
        /// </summary>
        /// <param name="stream">The stream to read the vector in. The
        /// cursor is moved forward after the parse.</param>
        /// <returns>The vector parsed in the file.</returns>
        private static Vec2 ParseVector2(FileStream stream)
        {
            Vec2 result;

            try
            {
                result = new Vec2(ParseFloat(stream), ParseFloat(stream));
            }
            catch (MapFormatException e)
            {
                throw new MapFormatException($"Could not parse Vec2, encountered position at position '{stream.Position}'.", e);
            }

            return result;
        }

        /// <summary>
        /// Parses the next Vec3 found in the stream, by parsing three floats,
        /// and returns it.
        /// </summary>
        /// <param name="stream">The stream to read the vector in. The
        /// cursor is moved forward after the parse.</param>
        /// <returns>The vector parsed in the file.</returns>
        private static Vec3 ParseVector3(FileStream stream)
        {
            Vec3 result;

            try
            {
                result = new Vec3(ParseFloat(stream), ParseFloat(stream), ParseFloat(stream));
            }
            catch (MapFormatException e)
            {
                throw new MapFormatException($"Could not parse Vec3, encountered position at position '{stream.Position}'.", e);
            }

            return result;
        }

        /// <summary>
        /// Checks to see if the current character is a comment, if so, it then
        /// skips the rest of the line.
        /// </summary>
        /// <param name="current">The current character the stream is reading.</param>
        /// <param name="stream">The stream we're reading.</param>
        private static void CheckAndSkipComment(ref char current, FileStream stream)
        {
            if (current == Constants.FORWARD_SLASH && LookForward(stream) == Constants.FORWARD_SLASH)
            {
                var b = stream.ReadByte();

                do
                {
                    current = (char)b;
                } 
                while ((b = stream.ReadByte()) != -1 && current != Constants.CR && current != Constants.LF);
            }
        }

        /// <summary>
        /// Jumps forward one character and returns it. Jumps back before
        /// returning.
        /// </summary>
        /// <param name="stream">The stream to look ahead in.</param>
        /// <returns>The character on position ahead.</returns>
        private static char LookForward(FileStream stream)
        {
            var f = (char)stream.ReadByte();
            stream.Seek(stream.Position - 1, SeekOrigin.Begin);

            return f;
        }

        /// <summary>
        /// Contains character constants used in parsing.
        /// </summary>
        private static class Constants
        {
            public const char LEFT_BRACKET = '{';
            public const char RIGHT_BRACKET = '}';
            public const char SPACE = ' ';
            public const char CR = '\r';
            public const char LF = '\n';
            public const char FORWARD_SLASH = '/';
            public const char QUOTE = '"';
            public const char LEFT_PARENTHESIS = '(';
            public const char NEGATIVE_SIGN = '-';
            public const char LEFT_SQUARE_BRACKET = '[';
            public const char RIGHT_SQUARE_BRACKET = ']';
            public const char PERIOD = '.';
        }
    }
}
