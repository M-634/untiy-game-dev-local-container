using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pj_server.Api.Domain.Item;
using pj_server.Api.Infrastructure.Db;

namespace pj_server.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly AppDb _appDb;
    
    public SampleController(AppDb appDb)
    {
        _appDb = appDb;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAllItems()
    {
        var items = await _appDb.Items.Include(i => i.ItemLocalize).ToListAsync();
        return CreatedAtAction(nameof(GetAllItems), items);
    }

    [HttpPost]
    public Task<ActionResult<Item>> GetItem(int itemId)
    {
        var targetItem = _appDb.Items.FirstOrDefault(item => item.ItemId == itemId);
        if (targetItem == null)
        {
            return Task.FromResult<ActionResult<Item>>(new NotFoundResult());
        }
        return Task.FromResult<ActionResult<Item>>(targetItem);
    }
    
}