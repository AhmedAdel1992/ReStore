using API.Entities;
using API.RequestHelpers;
using Azure;
using System.Text.Json;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, MetaData metaData)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(metaData, options));
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
            //a5er line fo2 da by2ol en ana same7 en el headers el custom elly ana 3amlha dy tro7 lel client 3ady b3ml
            //CORS modification y3ny
        }
    }
}
