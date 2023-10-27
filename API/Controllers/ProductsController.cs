using API.Data;
using API.Entities;
using API.Extensions;
using API.RequestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProductsAsync([FromQuery]ProductParams productParams)
        {
            var query = _context.Products
                .Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();

            var products = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

            Response.AddPaginationHeader(products.MetaData);
            //Response.Headers.Add("Pagination", JsonSerializer.Serialize(products.MetaData)); //da lw m3mltsh extension method
            //fo2 hna zwdt 3ala el headers elly htrg3ly mn el response el Pagination Info elly hgbha mn el metadata 
            //elly gowa el products l2n el products dy dlw2ty mn no3 PagedList wel PagedList ana 3amlha prop MetaData
            //ana 3mlt extension method lel HttpResponse esmha AddPaginationHeader w est5dmtha bdl el line elly howa da
            //Response.Headers.Add("Pagination", JsonSerializer.Serialize(products.MetaData));

            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            return product;
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

            return Ok(new { brands, types });
        }
    }
}
