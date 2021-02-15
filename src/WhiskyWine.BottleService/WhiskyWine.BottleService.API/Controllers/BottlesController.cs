using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBottleAsync(string bottleId)
        {
            try
            {
                var result = await this._bottleService.GetBottleAsync(bottleId);
                return result == null ? NotFound(bottleId) : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBottlesAsync()
        {
            try 
            {
                var result = await this._bottleService.GetAllBottlesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostBottleAsync(Bottle bottle)
        {

            try
            {
                var result = await this._bottleService.PostBottleAsync(bottle);
                if (result != null)
                {
                    return Created($"api/bottles/{bottle.BottleId}", result);
                }
                return BadRequest("Cannot insert duplicate record.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{bottleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBottleAsync(string bottleId, Bottle bottle)
        {
            bottle.BottleId = bottleId;
            try
            {
                if (await this._bottleService.GetBottleAsync(bottleId) == null) return NotFound(bottleId);
                
                await this._bottleService.UpdateBottleAsync(bottleId, bottle);
                return Ok(bottle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
