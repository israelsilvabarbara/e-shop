
namespace Logger.API.DTOs
{
    public record ServiceSelectorRequest
    (
        string Service
    )
    {
        internal static ServiceSelectorRequest FromQuery(IQueryCollection query)
        {
            return new ServiceSelectorRequest(
                Service: query.ContainsKey("Service") && !string.IsNullOrWhiteSpace(query["Service"]) ? query["Service"].ToString() : ""
            );
        }
    }

}