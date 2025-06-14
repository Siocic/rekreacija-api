using System.Text.Json;
using System.Text.Json.Serialization;

public class DateTimeConverterUsingIso8601 : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.Parse(reader.GetString() ?? throw new FormatException("Invalid DateTime"));

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("o"));
}
