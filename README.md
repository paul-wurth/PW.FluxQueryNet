# PW.FluxQueryNet

Flux query builder fluent API for .NET.  
Forked from [this project](https://github.com/MalikRizwanBashir/FluxQuery.Net) and significantly improved.

## Example

```csharp
var fluxQuery = FluxQueryBuilder.From("bucketName")
    .Filter(f => f.Measurement("measurementName")
        .Tag("tagKey1", "tagValue1")
        .Tag("tagKey2", "tagValue2")
        .Fields("field1", "field2")
        .Where("r._value > 0")
    )
    .Range(new DateTime(2023, 01, 02, 03, 04, 05, DateTimeKind.Utc), TimeSpan.FromDays(2.5))
    .Limit(50)
    .ToQuery();
```

The code above generates the following Flux query:
```flux
from(bucket: "bucketName")
|> filter(fn: (r) => r._measurement == "measurementName" and r["tagKey1"] == "tagValue1" and r["tagKey2"] == "tagValue2" and (r._field == "field1" or r._field == "field2") and r._value > 0)
|> range(start: 2023-01-02T03:04:05.0000000Z, stop: 2d12h)
|> limit(n: 50)
```