using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SqlViewer.QueryExecution.Domain.SqlQuery;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.QueryExecution.Services.Grpc;

public class QueryExecutionGrpcService(ISqlQueryManager sqlQueryService) : QueryExecutionService.QueryExecutionServiceBase
{
    public override async Task<SqlQueryResponse> Query(SqlQueryRequest request, ServerCallContext context)
    {
        List<dynamic> result = await sqlQueryService.QueryAsync(
            (VelocipedeDatabaseType)request.DatabaseType,
            request.ConnectionString,
            request.Query);

        SqlQueryResponse response = new();

        foreach (dynamic row in result)
        {
            SqlRow sqlRow = new();
            IDictionary<string, object> fields = (IDictionary<string, object>)row;
            foreach (KeyValuePair<string, object> kvp in fields)
            {
                sqlRow.Fields.Add(kvp.Key, MapToValue(kvp.Value));
            }
            response.QueryResult.Add(sqlRow);
        }
        return response;
    }

    private static Value MapToValue(object? val)
    {
        return val switch
        {
            null => Value.ForNull(),
            string s => Value.ForString(s),
            double d => Value.ForNumber(d),
            int i => Value.ForNumber(i),
            bool b => Value.ForBool(b),
            _ => Value.ForString(val.ToString() ?? string.Empty)
        };
    }
}
