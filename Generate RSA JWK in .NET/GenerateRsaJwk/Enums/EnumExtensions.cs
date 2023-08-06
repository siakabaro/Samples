using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GenerateRsaJwk.Enums
{
    internal static class EnumExtensions
    {

        internal static JsonWebAlgorithmAttribute? GetJsonWebAlgorithm(this Enum e)
        {
            Type type = e.GetType();
            FieldInfo? fieldInfo = type.GetField(e.ToString());
            JsonWebAlgorithmAttribute[]? attributes = fieldInfo?.GetCustomAttributes(typeof(JsonWebAlgorithmAttribute), false) as JsonWebAlgorithmAttribute[];
            if (attributes?.Length > 0)
            {
                return attributes[0];
            }
            return null;
        }
    }
}
