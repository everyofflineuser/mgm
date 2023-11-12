using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.DataTypes;
using System;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Converter
{
    internal class GuildPermissionsConverter : JsonConverter<GuildPermissions>
    {
        public static GuildPermissionsConverter Instance => new GuildPermissionsConverter();

        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override GuildPermissions ReadJson(JsonReader reader, Type objectType, GuildPermissions existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            string rawPermission = obj.ToObject<string>();

            return new GuildPermissions(rawPermission);
        }

        public override void WriteJson(JsonWriter writer, GuildPermissions value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}
