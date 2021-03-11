using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.API.Controllers
{

    /// <summary>
    /// The main API controller for the BottleService.
    /// Handles all incoming requests to the REST service.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BottlesController : Controller
    {
        /// <summary>
        /// The domain service to which API calls will be passed.
        /// </summary>
        private readonly IBottleService _bottleService;

        /// <summary>
        /// Constructs an instance of the BottlesController.
        /// </summary>
        /// <param name="bottleService">An instance of class implementing the IBottleService interface.</param>
        public BottlesController(IBottleService bottleService)
        {
            this._bottleService = bottleService;
        }

        /// <summary>
        /// Gets a single Bottle entity asynchronously, using its Id.
        /// </summary>
        /// <param name="bottleId">The Id of the Bottle to get.</param>
        /// <returns>Ok result containing Bottle if Bottle returned by service. NotFound result containing Id passed otherwise.</returns>
        [HttpGet("{bottleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBottleAsync(string bottleId)
        {
            var result = await this._bottleService.GetBottleAsync(bottleId);
            
            //If service returns null, no Bottle has been found matching the passed id.
            return result == null ? (IActionResult)NotFound(bottleId) : Ok(result);
        }

        /// <summary>
        /// Gets all Bottle entities.
        /// </summary>
        /// <returns>Ok result containing all Bottle data returned by service.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBottlesAsync()
        {
            var result = await this._bottleService.GetAllBottlesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Creates a new Bottle through the service.
        /// </summary>
        /// <param name="bottle">The Bottle entity to create.</param>
        /// <returns>Created result containing the Bottle if successful. BadRequest result if Bottle already exists with given Id.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostBottleAsync(Bottle bottle)
        {
            var result = await this._bottleService.PostBottleAsync(bottle);
            
            //If service returns null, the Bottle object has not been successfully created.
            if (result != null)
            {
                return Created($"api/bottles/{bottle.BottleId}", result);
            }
            return BadRequest("Cannot insert duplicate record.");
        }

        /// <summary>
        /// Update-Replaces an existing Bottle entity.
        /// </summary>
        /// <param name="bottleId">The Id of the Bottle entity to update.</param>
        /// <param name="bottle">The Bottle entity to associate with the passed Id.</param>
        /// <returns>Ok result containing the Bottle if successful. NotFound result containing passed Id if Bottle not found.</returns>
        [HttpPut("{bottleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBottleAsync(string bottleId, Bottle bottle)
        {
            bottle.BottleId = bottleId;

            //If service returns null, no Bottle has been found matching the passed id.
            if (await this._bottleService.GetBottleAsync(bottleId) == null) return NotFound(bottleId);

            await this._bottleService.UpdateBottleAsync(bottleId, bottle);
            return Ok(bottle);

        }

        /// <summary>
        /// Deletes a Bottle through the bottle service.
        /// </summary>
        /// <param name="bottleId">The Id of the Bottle entity to delete.</param>
        /// <returns>NoContent result if the operation is successful. NotFound result containing the passed Id if Bottle not found.</returns>
        [HttpDelete("{bottleId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBottleAsync(string bottleId)
        {
            var result = await this._bottleService.DeleteBottleAsync(bottleId);

            //If service returns false, no Bottle has been found matching the passed id.
            return result == false ? NotFound(bottleId) : NoContent();
        }
    }
}
