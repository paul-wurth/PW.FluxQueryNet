# PW.FluxQueryNet

Flux query builder fluent API for C#.
Forked from [this project](https://github.com/MalikRizwanBashir/FluxQuery.Net) and improved.

## Example

```csharp
QueryBuilder.From("datasource", "retention")
    .Filter(f => f.Measurement("measurementName")
        .Where("r.tagKey1 == \"tagValue1\"")
        .Where("r.tagKey2 == \"tagValue2\"")
        .Select(s => s.Fields("field1", "field2"))
    )
    .AbsoluteTimeRange(DateTime.Now.AddDays(-1), DateTime.Now)
    .ToQuery();
```
