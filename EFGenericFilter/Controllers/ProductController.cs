using EFGenericFilter.Contexts;
using EFGenericFilter.DTOs;
using EFGenericFilter.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFGenericFilter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly DataContext _context;

    public ProductController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]ProductParameters parameters)
    {
        var products = await _context.Products.Filter(parameters).ToListAsync();
        return Ok(products);
    }
}