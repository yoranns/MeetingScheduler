using MeetingScheduler.Domain.Extensions;
using MeetingScheduler.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeetingScheduler.Domain.Providers
{
    public class MeetingStatusJsonConverter : JsonConverter<MeetingStatus>
    {
        public override MeetingStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException();

            var statusString = reader.GetString().ToLower();
            return MeetingStatusExtensions.FromDescriptionString(statusString);
        }

        public override void Write(Utf8JsonWriter writer, MeetingStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToDescriptionString());
        }
    }
}
