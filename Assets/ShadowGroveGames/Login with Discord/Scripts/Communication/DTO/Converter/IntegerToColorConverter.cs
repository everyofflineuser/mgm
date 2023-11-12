using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Converter
{
    public class IntegerToColorConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, long.Parse(ColorUtility.ToHtmlStringRGB(value), System.Globalization.NumberStyles.HexNumber));
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return Color.white;

            int intColor = int.Parse(reader.Value.ToString());
            string hexColor = String.Format("#{0:X6}", 0xFFFFFF & intColor).ToUpper();

            if (!ColorUtility.TryParseHtmlString(hexColor, out Color color))
                throw new Exception("Cant convert to Color");

            return color;
        }
    }
}
