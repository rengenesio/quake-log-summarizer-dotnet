using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuakeLogSummarizer.Core.Converters
{
    /// <summary>
    /// Converts a list containing key/value pairs to JSON as a dictionary.
    /// Useful when needs serialize a dictionary containing duplicated keys.
    /// </summary>
    /// <remarks>
    /// Only the write function is implemented currently (serialization). It doesn't support deserialize a JSON string into an object.
    /// </remarks>
    public class KeyValuePairListConverter : JsonConverter<IEnumerable<(string PropertyName, int Value)>>
    {
        /// <inheritdoc />
        public override IEnumerable<(string, int)> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, IEnumerable<(string PropertyName, int Value)> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach ((string PropertyName, int Value) item in value)
            {
                writer.WritePropertyName(item.PropertyName);
                writer.WriteNumberValue(item.Value);
            }

            writer.WriteEndObject();
        }
    }
}
