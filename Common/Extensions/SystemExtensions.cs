
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NPOI.SS.Formula;
using NPOI.SS.UserModel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace System
{
    public static class SystemExtensions
    {
        public static string GetUrl(this HttpContext context, params string[] urls)
        {
            var result = $"{context.Request.PathBase}";

            foreach (var url in urls)
            {
                result += $"/{url}";
            }

            return result;
        }

        /// <summary>
        /// Devuelve el valor de un Enum
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TResult GetValue<TResult>(this IConvertible source)
            where TResult : IConvertible
        {
            return (TResult)source;
        }

        /// <summary>
        /// Devuelve el valor del enum
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static short GetValue(this IConvertible source)
        {
            return (short)source;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="soure"></param>
        /// <returns></returns>
        public static int Count(this Enum soure)
        {
            return Enum.GetNames(soure.GetType()).Length;
        }

        public static string GetName(this Enum value, bool? lowercase = null)
        {
            var result = Convert.ToString(value);

            if (lowercase.HasValue)
            {
                if (lowercase.Value)
                {
                    result = result?.ToLower();
                }
                else
                {
                    result = result?.ToUpper();
                }
            }

            return result;
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }

        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        public static string GeneratePassword(this object me, int length = 8)
        {
            me?.ToString();

            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%*";
            var random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }

            return new string(chars);
        }

        public static string DecodeToken(this string token)
        {
            try
            {
                var hash = token.FromBase64String();
                var code = hash.Substring(32);
                code = new string(code.Reverse().ToArray());

                return code;
            }
            catch
            {
                return default;
            }
        }

        public static string EncodeToken(this object data)
        {
            try
            {
                var code = new string($"{data}".Reverse().ToArray());
                return ($"{Guid.NewGuid()}".Replace("-", "") + code).ToBase64String();
            }
            catch
            {
                return default;
            }
        }

        public static string ToBase64String(this object data)
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes($"{data}");
                return Convert.ToBase64String(plainTextBytes);
            }
            catch
            {
                return default;
            }
        }

        //Decode
        public static string FromBase64String(this string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return default;
            }

        }

        public static Attachment ToAttachment(this byte[] file, string name)
        {
            if (file != null && file.Length > 0)
            {
                return ToAttachment(new MemoryStream(file), name);
            }

            return default;
        }

        public static Attachment ToAttachment(this Stream file, string name)
        {
            return new Attachment(file, name);
        }

        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static string[] DecodeBasicToken(this StringValues token)
        {
            try
            {
                var apptoken = AuthenticationHeaderValue.Parse(token)?.Parameter;

                var credentialBytes = Convert.FromBase64String(apptoken);

                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);

                return credentials;
            }
            catch (Exception ex)
            {
                throw new Exception("El token de seguridad no es el correcto", ex);
            }
        }

        public static string GetApplicationToken(this HttpRequest request)
        {
            try
            {
                return ((string)request.Headers["AppId"]) ?? request.Headers["ApiKey"];
            }
            catch (Exception ex)
            {
                throw new Exception("El token de la aplicación no es el correcto", ex);
            }
        }

        public static string GetCompanyToken(this HttpRequest request)
        {
            try
            {
                return ((string)request.Headers["CompanyId"]) ?? request.Headers["Company"];
            }
            catch (Exception ex)
            {
                throw new Exception("El token de la aplicación no es el correcto", ex);
            }
        }

        public static string GetBearerToken(this HttpRequest request)
        {
            try
            {
                return request.Headers["Authorization"].FirstOrDefault()?.Split(" ")?.Last();
            }
            catch (Exception ex)
            {
                throw new Exception("El token de seguridad no es el correcto", ex);
            }
        }

        public static object GetValue(this ICell cell)
        {
            object result = null;

            try
            {
                if (cell != null)
                {
                    if (cell.CellType == CellType.Formula)
                    {
                        var wb = cell.Sheet.Workbook;

                        if (wb != null)
                        {
                            BaseFormulaEvaluator.EvaluateAllFormulaCells(wb);
                        }
                    }

                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            result = DateUtil.IsCellDateFormatted(cell) ? cell.DateCellValue : cell.NumericCellValue;
                            break;
                        case CellType.String:
                            result = $"{cell.StringCellValue}";
                            break;
                        case CellType.Blank:
                            result = string.Empty;
                            break;
                        case CellType.Boolean:
                            result = cell.BooleanCellValue;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}