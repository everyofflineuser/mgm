using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.DataTypes;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Flags;
using System;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Converter
{
    internal class GuildFeaturesConverter : JsonConverter<GuildFeatures>
    {
        public static GuildFeaturesConverter Instance => new GuildFeaturesConverter();

        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override GuildFeatures ReadJson(JsonReader reader, Type objectType, GuildFeatures existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            var rawFeatureList = obj.ToObject<string[]>();

            GuildFeature features = GuildFeature.None;

            foreach (var item in rawFeatureList)
            {
                if (Enum.TryParse<GuildFeature>(string.Concat(item.Split('_')), true, out var result))
                    features |= result;
            }

            return new GuildFeatures(features, rawFeatureList);
        }

        public override void WriteJson(JsonWriter writer, GuildFeatures value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.RawFeatureList);
        }
    }
}
