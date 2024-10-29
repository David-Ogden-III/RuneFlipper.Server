using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Entities;
using System.Linq.Expressions;
using System.Security.Claims;
using Models.TradeFactory;

namespace Controllers;

[Route("[controller]"), ApiController, Authorize]
public class TradeController(RuneFlipperContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<TradeDetails>>> Get(Trade trade)
    {
        
        return Ok(new Trade());
    }
}