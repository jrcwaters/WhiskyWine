using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Interfaces;

namespace WhiskyWine.BottleService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BottlesController : Controller
    {
        private readonly IBottleService _bottleService;

        public BottlesController(IBottleService bottleService)
        {
            this._bottleService = bottleService;
        }

        [HttpGet("{bottleId}")]
        public async Task<ActionResult> GetBottleById(int bottleId)
        {
            return null;
        }
    }
}
