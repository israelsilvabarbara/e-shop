namespace Inventory.API.DTOs
{
    public record GetItemsRequest(
        List<Guid> ProductIds
    )
    {
        public static GetItemsRequest? FromQuery(IQueryCollection query)
        {
            try
            {
                var ids = query["ProductIds"].ToString()
                                            .Split(',')
                                            .ToList();


                                            Console.WriteLine("DEBUG: " + ids);

                if (ids.Any(id => string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out _)))
                {
                    return null;
                }

                var guids = ids.Select(id => Guid.Parse(id)).ToList();

                Console.WriteLine("DEBUG: " + guids);
                
                return new GetItemsRequest(ProductIds: guids);
            }
            catch(Exception ex)
            {
                Console.WriteLine("DEBUG ERROR: " + ex.Message);
                return null;
            }
        }
    }
}