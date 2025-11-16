using System.Text.Json.Serialization;
using System.Text.Json;

public class DecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions options)
        => reader.GetDecimal();

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        var rounded = Math.Round(value, 6);
        var formatted = rounded.ToString("0.######");
        writer.WriteRawValue(formatted);
    }
}
