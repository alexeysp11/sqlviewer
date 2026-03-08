namespace SqlViewer.ApiGateway.Extensions;

public static class ValueExtensions
{
    public static object? Unwrap(this Google.Protobuf.WellKnownTypes.Value value) => value.KindCase switch
    {
        Google.Protobuf.WellKnownTypes.Value.KindOneofCase.StringValue => value.StringValue,
        Google.Protobuf.WellKnownTypes.Value.KindOneofCase.NumberValue => value.NumberValue,
        Google.Protobuf.WellKnownTypes.Value.KindOneofCase.BoolValue => value.BoolValue,
        Google.Protobuf.WellKnownTypes.Value.KindOneofCase.NullValue => null,
        _ => value.ToString()
    };
}
