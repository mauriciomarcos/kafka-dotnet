using Confluent.Kafka;
using System.IO.Compression;
using System.Text.Json;

namespace kafka.Serializations;

internal class KafkaDesserializer<T> : IDeserializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        using var memoryStream = new MemoryStream(data.ToArray());
        using var unzipStream = new GZipStream(memoryStream, CompressionMode.Decompress, true);

        return JsonSerializer.Deserialize<T>(unzipStream)!;
    }
}