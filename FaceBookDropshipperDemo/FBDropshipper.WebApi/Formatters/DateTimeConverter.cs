using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FBDropshipper.Common.Extensions;

namespace FBDropshipper.WebApi.Formatters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var data = reader.GetString();
            return data == null ? DateTime.MinValue : DateTime.Parse(data);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToGeneralDateTime());
        }
    }

    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var data = reader.GetString();
            return data == null ? DateOnly.MinValue : DateOnly.Parse(data);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToGeneralDate());
        }
    }
}