using System;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class Matrix
    {
        internal double M11 { get; set; }
        internal double M12 { get; set; }
        internal double M21 { get; set; }
        internal double M22 { get; set; }
        internal double OffsetX { get; set; }
        internal double OffsetY { get; set; }
        internal MatrixTypes Type { get; set; }

        internal static Matrix IdentityMatrix()
        {
            return new Matrix() { M11 = 1, M12 = 0, M21 = 0, M22 = 1, OffsetX = 0, OffsetY = 0, Type = MatrixTypes.Identity };
        }

        internal static DiagramPoint TransformPointByMatrix(Matrix matrix, DiagramPoint point)
        {
            DiagramPoint pt = MultiplyPoint(matrix, point.X, point.Y);
            return new DiagramPoint() { X = Math.Round(pt.X * 100) / 100, Y = Math.Round(pt.Y * 100) / 100 };
        }

        internal static DiagramPoint[] TransformPointsByMatrix(Matrix matrix, DiagramPoint[] points)
        {
            DiagramPoint[] transformedPoints = new DiagramPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                transformedPoints[i] = TransformPointByMatrix(matrix, points[i]);
            }
            return transformedPoints;
        }

        internal static void RotateMatrix(Matrix matrix, double angle, double centerX, double centerY)
        {
            angle %= 360.0;
            MultiplyMatrix(matrix, CreateRotationRadians(angle * 0.017453292519943295, centerX != 0 ? centerX : 0, centerY != 0 ? centerY : 0));
        }

        internal static void ScaleMatrix(Matrix matrix, double scaleX, double scaleY, double centerX = 0, double centerY = 0)
        {
            MultiplyMatrix(matrix, CreateScaling(scaleX, scaleY, centerX, centerY));
        }

        internal static void TranslateMatrix(Matrix matrix, double offsetX, double offsetY)
        {
            if ((matrix.Type & MatrixTypes.Identity) != 0)
            {
                matrix.Type = MatrixTypes.Translation;
                SetMatrix(matrix, 1.0, 0.0, 0.0, 1.0, offsetX, offsetY);
                return;
            }
            if ((matrix.Type & MatrixTypes.Unknown) != 0)
            {
                matrix.OffsetX += offsetX;
                matrix.OffsetY += offsetY;
                return;
            }
            matrix.OffsetX += offsetX;
            matrix.OffsetY += offsetY;
            matrix.Type |= MatrixTypes.Translation;
        }

        private static Matrix CreateScaling(double scaleX, double scaleY, double centerX, double centerY)
        {
            Matrix result = IdentityMatrix();
            result.Type = !(centerX != 0 || centerY != 0) ? MatrixTypes.Scaling : MatrixTypes.Scaling | MatrixTypes.Translation;
            SetMatrix(result, scaleX, 0.0, 0.0, scaleY, centerX - scaleX * centerX, centerY - scaleY * centerY);
            return result;
        }

        private static Matrix CreateRotationRadians(double angle, double centerX, double centerY)
        {
            Matrix result = IdentityMatrix();
            double num = Math.Sin(angle);
            double num2 = Math.Cos(angle);
            double offsetX = centerX * (1.0 - num2) + centerY * num;
            double offsetY = centerY * (1.0 - num2) - centerX * num;
            result.Type = MatrixTypes.Unknown;
            SetMatrix(result, num2, num, -num, num2, offsetX, offsetY);
            return result;
        }


        private static DiagramPoint MultiplyPoint(Matrix matrix, double x, double y)
        {
            switch (matrix.Type)
            {
                case MatrixTypes.Identity: break;
                case MatrixTypes.Translation:
                    x += matrix.OffsetX;
                    y += matrix.OffsetY;
                    break;
                case MatrixTypes.Scaling:
                    x *= matrix.M11;
                    y *= matrix.M22;
                    break;
                case MatrixTypes.Translation | MatrixTypes.Scaling:
                    x *= matrix.M11;
                    x += matrix.OffsetX;
                    y *= matrix.M22;
                    y += matrix.OffsetY;
                    break;
                default:
                    double num = y * matrix.M21 + matrix.OffsetX;
                    double num2 = x * matrix.M12 + matrix.OffsetY;
                    x *= matrix.M11;
                    x += num;
                    y *= matrix.M22;
                    y += num2;
                    break;
            }
            return new DiagramPoint(x, y);
        }

        internal static void MultiplyMatrix(Matrix matrix1, Matrix matrix2)
        {
            MatrixTypes type = matrix1.Type;
            MatrixTypes type2 = matrix2.Type;
            if (type2 == MatrixTypes.Identity)
            {
                return;
            }
            if (type == MatrixTypes.Identity)
            {
                AssignMatrix(matrix1, matrix2);
                matrix1.Type = matrix2.Type;
                return;
            }
            if (type2 == MatrixTypes.Translation)
            {
                matrix1.OffsetX += matrix2.OffsetX;
                matrix1.OffsetY += matrix2.OffsetY;
                if (type != MatrixTypes.Unknown)
                {
                    matrix1.Type |= MatrixTypes.Translation;
                }
                return;
            }
            if (type != MatrixTypes.Translation)
            {
                double num = (int)type << 4 | (int)type2;
                switch (num)
                {
                    case 34:
                        matrix1.M11 *= matrix2.M11;
                        matrix1.M22 *= matrix2.M22;
                        return;
                    case 35:
                        matrix1.M11 *= matrix2.M11;
                        matrix1.M22 *= matrix2.M22;
                        matrix1.OffsetX = matrix2.OffsetX;
                        matrix1.OffsetY = matrix2.OffsetY;
                        matrix1.Type = (MatrixTypes.Translation | MatrixTypes.Scaling);
                        return;
                    case 36: break;
                    default:
                        {
                            switch (num)
                            {
                                case 50:
                                    matrix1.M11 *= matrix2.M11;
                                    matrix1.M22 *= matrix2.M22;
                                    matrix1.OffsetX *= matrix2.M11;
                                    matrix1.OffsetY *= matrix2.M22;
                                    return;
                                case 51:
                                    matrix1.M11 *= matrix2.M11;
                                    matrix1.M22 *= matrix2.M22;
                                    matrix1.OffsetX = matrix2.M11 * matrix1.OffsetX + matrix2.OffsetX;
                                    matrix1.OffsetY = matrix2.M22 * matrix1.OffsetY + matrix2.OffsetY;
                                    return;
                                case 52: break;
                                default:
                                    switch (num)
                                    {
                                        case 66:
                                        case 67:
                                        case 68: break;
                                        default: return;
                                    }
                                    break;
                            }
                            break;
                        }
                }
                Matrix result = IdentityMatrix();
                double m11New = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21;
                double m12New = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22;
                double m21New = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21;
                double m22New = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22;
                double offsetXNew = matrix1.OffsetX * matrix2.M11 + matrix1.OffsetY * matrix2.M21 + matrix2.OffsetX;
                double offsetYNew = matrix1.OffsetX * matrix2.M12 + matrix1.OffsetY * matrix2.M22 + matrix2.OffsetY;
                SetMatrix(result, m11New, m12New, m21New, m22New, offsetXNew, offsetYNew);
                if (result.M21 != 0 || result.M12 != 0)
                {
                    result.Type = MatrixTypes.Unknown;
                }
                else
                {
                    if (result.M11 != 0 && result.M11 != 1.0 || result.M22 != 0 && result.M22 != 1.0)
                    {
                        result.Type = MatrixTypes.Scaling;
                    }
                    if (result.OffsetX != 0 || result.OffsetY != 0)
                    {
                        result.Type |= MatrixTypes.Translation;
                    }
                    if ((result.Type & (MatrixTypes.Translation | MatrixTypes.Scaling)) == MatrixTypes.Identity)
                    {
                        result.Type = MatrixTypes.Identity;
                    }
                    result.Type = MatrixTypes.Scaling | MatrixTypes.Translation;
                }
                AssignMatrix(matrix1, result);
                matrix1.Type = result.Type;
                return;
            }
            double offsetX = matrix1.OffsetX;
            double offsetY = matrix1.OffsetY;
            matrix1.OffsetX = offsetX * matrix2.M11 + offsetY * matrix2.M21 + matrix2.OffsetX;
            matrix1.OffsetY = offsetX * matrix2.M12 + offsetY * matrix2.M22 + matrix2.OffsetY;
            if (type2 == MatrixTypes.Unknown)
            {
                matrix1.Type = MatrixTypes.Unknown;
                return;
            }
            matrix1.Type = (MatrixTypes.Translation | MatrixTypes.Scaling);
        }

        private static void SetMatrix(Matrix mat, double m11, double m12, double m21, double m22, double x, double y)
        {
            mat.M11 = m11;
            mat.M12 = m12;
            mat.M21 = m21;
            mat.M22 = m22;
            mat.OffsetX = x;
            mat.OffsetY = y;
        }
        private static void AssignMatrix(Matrix matrix1, Matrix matrix2)
        {
            matrix1.M11 = matrix2.M11;
            matrix1.M12 = matrix2.M12;
            matrix1.M21 = matrix2.M21;
            matrix1.M22 = matrix2.M22;
            matrix1.OffsetX = matrix2.OffsetX;
            matrix1.OffsetY = matrix2.OffsetY;
            matrix1.Type = matrix2.Type;
        }

    }
}