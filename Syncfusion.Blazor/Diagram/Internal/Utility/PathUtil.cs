using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal static class PathUtil
    {
        internal static List<PathSegment> ProcessPathData(string data)
        {
            List<PathSegment> collection = new List<PathSegment>() { }; int i; int j;
            List<object> arrayCollection = ParsePathData(data);
            if (arrayCollection.Count > 0)
            {
                for (i = 0; i < arrayCollection.Count; i++)
                {
                    List<object> objectCollection = arrayCollection[i] as List<object>;
                    char character = (char)objectCollection?[0];
                    switch (character.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture))
                    {
#pragma warning disable CA1305 // Specify IFormatProvider
                        case "m":
                            for (j = 1; j < objectCollection.Count; j++)
                            {
                                collection.Add(new PathSegment() { Command = character, X = Convert.ToDouble(objectCollection[j]), Y = Convert.ToDouble(objectCollection[j + 1]) });
                                j++;
                                if (character == 'm')
                                {
                                    character = 'l';
                                }
                                else if (character == 'M')
                                {
                                    character = 'L';
                                }
                            }
                            break;
                        case "l":
                        case "t":
                            for (j = 1; j < objectCollection.Count; j++)
                            {
                                collection.Add(new PathSegment() { Command = character, X = Convert.ToDouble(objectCollection[j]), Y = Convert.ToDouble(objectCollection[j + 1]) });
                                j++;
                            }
                            break;
                        case "h":
                            for (j = 1; j < objectCollection.Count; j++)
                            {
                                collection.Add(new PathSegment() { Command = character, X = Convert.ToDouble(objectCollection[j]) });
                            }
                            break;
                        case "v":
                            for (j = 1; j < objectCollection.Count; j++)
                            {
                                collection.Add(new PathSegment() { Command = character, Y = Convert.ToDouble(objectCollection[j]) });
                            }
                            break;
                        case "z":
                            collection.Add(new PathSegment() { Command = character });
                            break;
                        case "c":
                            for (j = 1; j < objectCollection.Count; j++)
                            {
                                collection.Add(new PathSegment()
                                {
                                    Command = character,
                                    X1 = Convert.ToDouble(objectCollection[j]),
                                    Y1 = Convert.ToDouble(objectCollection[j + 1]),
                                    X2 = Convert.ToDouble(objectCollection[j + 2]),
                                    Y2 = Convert.ToDouble(objectCollection[j + 3]),
                                    X = Convert.ToDouble(objectCollection[j + 4]),
                                    Y = Convert.ToDouble(objectCollection[j + 5])
                                });
                                j += 5;
                            }
                            break;
                        case "s":
                            for (j = 1; j < objectCollection.Count; j++)
                            {
                                collection.Add(new PathSegment() { Command = character, X2 = Convert.ToDouble(objectCollection[j]), Y2 = Convert.ToDouble(objectCollection[j + 1]), X = Convert.ToDouble(objectCollection[j + 2]), Y = Convert.ToDouble(objectCollection[j + 3]) });
                                j += 3;
                            }
                            break;
                        case "q":
                            for (j = 1; j < objectCollection.Count; j++)
                            {
                                collection.Add(new PathSegment() { Command = character, X1 = Convert.ToDouble(objectCollection[j]), Y1 = Convert.ToDouble(objectCollection[j + 1]), X = Convert.ToDouble(objectCollection[j + 2]), Y = Convert.ToDouble(objectCollection[j + 3]) });
                                j += 3;
                            }
                            break;
                        case "a":
                            for (j = 1; j < objectCollection.Count; j++)
                            {
                                collection.Add(new PathSegment()
                                {
                                    Command = character,
                                    R1 = Convert.ToDouble(objectCollection[j]),
                                    R2 = Convert.ToDouble(objectCollection[j + 1]),
                                    Angle = Convert.ToDouble(objectCollection[j + 2]),
                                    LargeArc = Convert.ToBoolean(objectCollection[j + 3]),
                                    Sweep = Convert.ToBoolean(objectCollection[j + 4]),
                                    X = Convert.ToDouble(objectCollection[j + 5]),
                                    Y = Convert.ToDouble(objectCollection[j + 6])
                                });
                                j += 6;
                            }
                            break;
#pragma warning disable CA1305 // Specify IFormatProvider
                    }
                }
            }
            return collection;
        }

        internal static List<object> ParsePathData(string data)
        {
            //These letters are valid SVG commands. Whenever we find one, a new command is starting. Let's split the string there.
            string separators = @"(?=[A-Za-z])";
            IEnumerable<string> tokens = Regex.Split(data, separators).Where(t => !string.IsNullOrEmpty(t));
            List<object> commands = new List<object>() { };
            foreach (string token in tokens)
            {
                // If the path is not correct, it throw an exception
                SVGCommand c = SVGCommand.Parse(token);
                List<object> command = new List<object>
                {
                    c.Command
                };
                foreach (float arg in c.Arguments)
                {
                    command.Add(arg);
                }
                commands.Add(command);
            }
            return commands;
        }

        internal static string GetRectanglePath(double cornerRadius, double height, double width)
        {
            double x = 0;
            double y = 0;
            int i;
            int corner = 0;
            DiagramPoint[] points = new DiagramPoint[] {
                new DiagramPoint(){ X= x + cornerRadius, Y= y },
                new DiagramPoint(){ X= x + width - cornerRadius, Y= y },
                new DiagramPoint(){ X= x + width, Y= y + cornerRadius }, new DiagramPoint(){ X= x + width, Y= y + height - cornerRadius },
                new DiagramPoint() { X= x + width - cornerRadius, Y= y + height }, new DiagramPoint() { X= x + cornerRadius, Y= y + height },
                new DiagramPoint() { X= x, Y= y + height - cornerRadius }, new DiagramPoint() { X= x, Y= y + cornerRadius }
            };
            DiagramPoint[] corners = new DiagramPoint[] { new DiagramPoint() { X = x + width, Y = y }, new DiagramPoint() { X = x + width, Y = y + height }, new DiagramPoint() { X = x, Y = y + height }, new DiagramPoint() { X = x, Y = y } };

            string path = 'M' + points[0].X + " " + points[0].Y;

            for (i = 0; i < points.Length; i += 2)
            {
                DiagramPoint point2 = points[i + 1];
                path += 'L' + point2.X + ' ' + point2.Y;
                DiagramPoint next = points[i + 2] ?? points[0];
                path += 'Q' + corners[corner].X + " " + corners[corner].Y + ' ' + next.X + " " + next.Y;
                corner++;
            }
            return path;
        }

        internal static string GetPolygonPath(DiagramPoint[] collection)
        {
            int i;
            string path = "M" + collection[0].X + " " + collection[0].Y;
            for (i = 1; i < collection.Length; i++)
            {
                DiagramPoint seg = collection[i];
                path += "L" + seg.X + " " + seg.Y;
            }
            path += "Z";
            return path;
        }

        internal static string TransformPath(List<PathSegment> arr, double sX, double sY, bool s, double bX, double bY, double iX, double iY)
        {
            double? x1 = null; double? y1 = null; double? x2 = null; double? y2 = null;
            double? x; double? y; double length; int i;
            for (x = 0, y = 0, i = 0, length = arr.Count; i < length; ++i)
            {
                PathSegment obj = arr[i]; PathSegment seg = obj;
                char character = seg.Command.Value;
                if (seg.X.HasValue) { x = seg.X.Value; }
                if (seg.Y.HasValue) { y = seg.Y.Value; }
                if (seg.Y1.HasValue) { y1 = seg.Y1.Value; }
                if (seg.Y2.HasValue) { y2 = seg.Y2.Value; }
                if (seg.X1.HasValue) { x1 = seg.X1.Value; }
                if (seg.X2.HasValue) { x2 = seg.X2.Value; }
                if (s)
                {
                    if (x.HasValue)
                    {
                        x = ScalePathData(x.Value, sX, bX, iX);
                    }
                    if (y.HasValue)
                    {
                        y = ScalePathData(y.Value, sY, bY, iY);
                    }
                    if (x1.HasValue)
                    {
                        x1 = ScalePathData(x1.Value, sX, bX, iX);
                    }
                    if (y1.HasValue)
                    {
                        y1 = ScalePathData(y1.Value, sY, bY, iY);
                    }
                    if (x2.HasValue)
                    {
                        x2 = ScalePathData(x2.Value, sX, bX, iX);
                    }
                    if (y2.HasValue)
                    {
                        y2 = ScalePathData(y2.Value, sY, bY, iY);
                    }
                }
                else
                {
                    if (x.HasValue)
                    {
                        x = Math.Round((x.Value + sX) * Math.Pow(10, 2)) / Math.Pow(10, 2);
                    }
                    if (y.HasValue)
                    {
                        y = Math.Round((y.Value + sY) * Math.Pow(10, 2)) / Math.Pow(10, 2);
                    }
                    if (x1.HasValue)
                    {
                        x1 = Math.Round((x1.Value + sX) * Math.Pow(10, 2)) / Math.Pow(10, 2);
                    }
                    if (y1.HasValue)
                    {
                        y1 = Math.Round((y1.Value + sY) * Math.Pow(10, 2)) / Math.Pow(10, 2);
                    }
                    if (x2.HasValue)
                    {
                        x2 = Math.Round((x2.Value + sX) * Math.Pow(10, 2)) / Math.Pow(10, 2);
                    }
                    if (y2.HasValue)
                    {
                        y2 = Math.Round((y2.Value + sY) * Math.Pow(10, 2)) / Math.Pow(10, 2);
                    }
                }
                PathSegment scaledPath = new PathSegment() { X = x, Y = y, X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, R1 = seg.R1, R2 = seg.R2 };
                PathSegment newSeg = UpdatedSegment(seg, character, scaledPath, s, sX, sY);
                if (newSeg != null)
                {
                    arr[i] = newSeg;
                }
            }
            string pathData = GetPathString(arr);
            return pathData;
        }

        internal static PathSegment UpdatedSegment(PathSegment segment, char character, PathSegment obj, bool isScale, double sX, double sY)
        {
            switch (character.ToString())
            {
                case "M":
                case "m":
                    segment.X = obj.X;
                    segment.Y = obj.Y;
                    break;
                case "L":
                case "l":
                    segment.X = obj.X;
                    segment.Y = obj.Y;
                    break;
                case "H":
                case "h":
                    segment.X = obj.X;
                    break;
                case "V":
                case "v":
                    segment.Y = obj.Y;
                    break;
                case "C":
                case "c":
                    segment.X = obj.X; segment.Y = obj.Y;
                    segment.X1 = obj.X1; segment.Y1 = obj.Y1;
                    segment.X2 = obj.X2; segment.Y2 = obj.Y2;
                    break;
                case "S":
                case "s":
                    segment.X = obj.X; segment.Y = obj.Y;
                    segment.X2 = obj.X2; segment.Y2 = obj.Y2;
                    break;
                case "Q":
                case "q":
                    segment.X = obj.X; segment.Y = obj.Y;
                    segment.X1 = obj.X1; segment.Y1 = obj.Y1;
                    break;
                case "T":
                case "t":
                    segment.X = obj.X; segment.Y = obj.Y;
                    break;
                case "A":
                case "a":
                    double r1 = obj.R1.Value;
                    double r2 = obj.R2.Value;
                    if (isScale)
                    {
                        obj.R1 = r1 *= sX;
                        obj.R2 = r2 *= sY;
                    }
                    segment.X = obj.X; segment.Y = obj.Y;
                    segment.R1 = obj.R1; segment.R2 = obj.R2;
                    break;
                case "z":
                case "Z":
                    segment = new PathSegment() { Command = 'Z' };
                    break;
            }
            return segment;
        }

        internal static double ScalePathData(double val, double scaleFactor, double oldOffset, double newOffset)
        {
            if (!val.Equals(oldOffset))
            {
                if (!newOffset.Equals(oldOffset))
                {
                    val = ((val * scaleFactor) - (oldOffset * scaleFactor - oldOffset))
                        + (newOffset - oldOffset);
                }
                else
                {
                    val = (val * scaleFactor) - (oldOffset * scaleFactor - oldOffset);
                }
            }
            else
            {
                if (!newOffset.Equals(oldOffset))
                {
                    val = newOffset;
                }
            }
            return Math.Round(val * Math.Pow(10, 2)) / Math.Pow(10, 2);
        }

        internal static List<PathSegment> SplitArrayCollection(List<PathSegment> arrayCollection)
        {
            double x0 = 0; double y0 = 0; double x1 = 0; double y1 = 0; double x2 = 0; double y2 = 0;
            double x; double y; double length; int i;
            for (x = 0, y = 0, i = 0, length = arrayCollection.Count; i < length; ++i)
            {
                object path = arrayCollection[i];
                PathSegment seg = path as PathSegment;
                char character = seg.Command.Value;
                Regex rgx = new Regex(@"^[MLHVCSQTA]$");
                if (rgx.IsMatch(character.ToString()))
                {
                    if (seg.X.HasValue) { seg.X = x = seg.X.Value; }
                    if (seg.Y.HasValue) { seg.Y = y = seg.Y.Value; }
                }
                else
                {
                    if (seg.X1.HasValue) { seg.X1 = x1 = x + seg.X1.Value; }
                    if (seg.X2.HasValue) { seg.X2 = x2 = x + seg.X2.Value; }
                    if (seg.Y1.HasValue) { seg.Y1 = y1 = y + seg.Y1.Value; }
                    if (seg.Y2.HasValue) { seg.Y2 = y2 = y + seg.Y2.Value; }
                    if (seg.X.HasValue) { seg.X = x += seg.X.Value; }
                    if (seg.Y.HasValue) { seg.Y = y += seg.Y.Value; }
                    PathSegment newSeg = null;
                    switch (character)
                    {
                        case 'm':
                        case 'M':
                            newSeg = new PathSegment() { Command = 'M', X = x, Y = y };
                            break;
                        case 'l':
                        case 'L':
                            newSeg = new PathSegment() { Command = 'L', X = x, Y = y };
                            break;
                        case 'h':
                        case 'H':
                            newSeg = new PathSegment() { Command = 'H', X = x };
                            break;
                        case 'v':
                        case 'V':
                            newSeg = new PathSegment() { Command = 'V', Y = y };
                            break;
                        case 'c':
                        case 'C':
                            newSeg = new PathSegment() { Command = 'C', X = x, Y = y, X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };
                            break;
                        case 's':
                        case 'S':
                            newSeg = new PathSegment() { Command = 'S', X = x, Y = y, X2 = x2, Y2 = y2 };
                            break;
                        case 'q':
                        case 'Q':
                            newSeg = new PathSegment() { Command = 'Q', X = x, Y = y, X1 = x1, Y1 = y1 };
                            break;
                        case 't':
                        case 'T':
                            newSeg = new PathSegment() { Command = 'T', X = x, Y = y };
                            break;
                        case 'a':
                        case 'A':
                            newSeg = new PathSegment
                            {
                                Command = 'A',
                                X = x,
                                Y = y,
                                R1 = seg.R1,
                                R2 = seg.R2,
                                Angle = seg.Angle,
                                LargeArc = seg.LargeArc,
                                Sweep = seg.Sweep
                            };
                            break;
                        case 'z':
                        case 'Z':
                            x = x0; y = y0;
                            newSeg = arrayCollection[i];
                            break;
                    }
                    if (newSeg != null)
                    {
                        arrayCollection[i] = newSeg;
                    }
                }
                if (character == 'M' || character == 'm')
                {
                    x0 = x; y0 = y;
                }
            }
            return arrayCollection;
        }

        internal static string GetPathString(List<PathSegment> arrayCollection)
        {
            string getNewString = string.Empty;
            for (int i = 0; i < arrayCollection.Count; i++)
            {
                if (i == 0)
                {
                    getNewString += GetString(arrayCollection[i]);
                }
                else
                {
                    getNewString += " " + GetString(arrayCollection[i]);
                }
            }
            return getNewString;
        }

        /** @private */
        internal static string GetString(PathSegment obj)
        {
            string value = string.Empty;
            switch (obj.Command)
            {
                case 'Z':
                case 'z':
                    value = obj.Command.ToString();
                    break;
                case 'M':
                case 'm':
                case 'L':
                case 'l':
                    value = obj.Command + " " + obj.X + " " + obj.Y;
                    break;
                case 'C':
                case 'c':
                    value = obj.Command + " " + obj.X1 + " " + obj.Y1 + " " + obj.X2 + " " + obj.Y2 + " " + obj.X + " " + obj.Y;
                    break;
                case 'Q':
                case 'q':
                    value = obj.Command + " " + obj.X1 + " " + obj.Y1 + " " + obj.X + " " + obj.Y;
                    break;
                case 'A':
                case 'a':
                    char cmd = obj.Command.Value;
                    if (obj.Angle != null)
                    {
                        double ang = obj.Angle.Value;
                        string l = (obj.LargeArc != null && obj.LargeArc.Value ? "1" : "0");
                        string s = (obj.Sweep != null && obj.Sweep.Value ? "1" : "0");
                        value = cmd + " " + obj.R1 + " " + obj.R2 + " " + ang + " " + l + " " + s + " " + obj.X + " " + obj.Y;
                    }

                    break;
                case 'H':
                case 'h':
                    value = obj.Command + " " + obj.X;
                    break;
                case 'V':
                case 'v':
                    value = obj.Command + " " + obj.Y;
                    break;
                case 'S':
                case 's':
                    value = obj.Command + " " + obj.X2 + " " + obj.Y2 + " " + obj.X + " " + obj.Y;
                    break;
                case 'T':
                case 't':
                    value = obj.Command + " " + obj.X + " " + obj.Y;
                    break;
            }
            return value;
        }
    }

    internal class SVGCommand
    {
        internal char Command { get; private set; }

        internal double[] Arguments { get; private set; }

        internal SVGCommand(char command, params double[] arguments)
        {
            this.Command = command;
            this.Arguments = arguments;
        }

        internal static SVGCommand Parse(string svgPathString)
        {
            char command = svgPathString.Take(1).Single();
            string remainingArgs = svgPathString[1..];
            string argSeparators = @"[\s,]|(?=-)";
            //string argSeparators = @"[\s,]";
            IEnumerable<string> splitArgs = Regex.Split(remainingArgs, argSeparators).Where(t => !string.IsNullOrEmpty(t));
            string[] value = splitArgs.Select(arg => arg).ToArray();
            if (value.Length == 1)
            {
                string[] val = value[0].Split(".");
                if (val.Length > 2)
                {
                    for (int j = 0; j < val.Length; j++)
                    {
                        if (j == 0)
                        {
                            remainingArgs = val[j] + "." + val[j + 1];
                            j = 1;
                        }
                        else
                        {
                            remainingArgs += " 0." + val[j];
                        }
                    }
                    splitArgs = Regex.Split(remainingArgs, argSeparators).Where(t => !string.IsNullOrEmpty(t));
                }
            };
#pragma warning disable CA1305 // Specify IFormatProvider
            double[] floatArgs = splitArgs.Select(arg => double.Parse(arg)).ToArray();
#pragma warning restore CA1305 // Specify IFormatProvider
            return new SVGCommand(command, floatArgs);
        }
    }
}