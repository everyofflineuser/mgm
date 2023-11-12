using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Converter
{
    public class HexToColorConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, "#" + ColorUtility.ToHtmlStringRGB(value));
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return Color.white;

            string hexColor = reader.Value.ToString();

            if (!ColorUtility.TryParseHtmlString(hexColor, out Color color))
                throw new Exception("Cant convert to Color");

            return color;
        }
    }
}
