using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WhiskyWine.BottleService.API.Models;
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
        /// The validator to ensure Bottles passed into the controller meet required standards.
        /// </summary>
        private readonly IValidator<BottleApiModel> _bottleValidator;
        private readonly IMapper<BottleDomainModel, BottleApiModel> _toApiModelMapper;
        private readonly IMapper<BottleApiModel, BottleDomainModel> _toDomainModelMapper;

        /// <summary>
        /// Constructs an instance of the BottlesController.
        /// </summary>
        /// <param name="bottleService">An instance of class implementing the IBottleService interface.</param>
        public BottlesController(IBottleService bottleService, 
            IValidator<BottleApiModel> bottleValidator, 
            IMapper<BottleDomainModel, BottleApiModel> toApiModelMapper, 
            IMapper<BottleApiModel, BottleDomainModel> toDomainModelMapper)
        {
            this._bottleService = bottleService;
            this._bottleValidator = bottleValidator;
            this._toApiModelMapper = toApiModelMapper;
            this._toDomainModelMapper = toDomainModelMapper;
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
            var domainBottle = await this._bottleService.GetBottleAsync(bottleId);

            var apiModelBottle = _toApiModelMapper.MapOne(domainBottle);

            //If service returns null, no Bottle has been found matching the passed id, so return a 404.
            return apiModelBottle == null ? (IActionResult)NotFound(bottleId) : Ok(apiModelBottle);
        }

        /// <summary>
        /// Gets all Bottle entities.
        /// </summary>
        /// <returns>Ok result containing all Bottle data returned by service.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBottlesAsync()
        {
            //If no bottles are found, the service will return an empty list, so we can still return a 200 containing this.
            var domainBottleList = await this._bottleService.GetAllBottlesAsync();

            var apiBottleList = _toApiModelMapper.MapMany(domainBottleList);
            return Ok(apiBottleList);
        }

        /// <summary>
        /// Creates a new Bottle through the service.
        /// </summary>
        /// <param name="bottle">The Bottle entity to create.</param>
        /// <returns>Created result containing the Bottle if successful. BadRequest result if Bottle already exists with given Id.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostBottleAsync(BottleApiModel apiBottle)
        {
            //If the bottle data passed into the call is valid, map it to a domain Bottle model so it can be passed into the domain service.
            await _bottleValidator.ValidateAndThrowAsync(apiBottle);
            var domainBottle = _toDomainModelMapper.MapOne(apiBottle);

            var servicePostResult = await this._bottleService.PostBottleAsync(domainBottle);

            //If service returned null, the Bottle object has not been successfully created, so return a 400.
            if (servicePostResult != null)
            {
                //Map the result that was returned from the service back to an API model, so it can be sent back out of the API in the response.
                var apiBottleToReturn = _toApiModelMapper.MapOne(servicePostResult);
                return Created($"api/bottles/{apiBottleToReturn.BottleId}", apiBottleToReturn);
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
        public async Task<IActionResult> UpdateBottleAsync(string bottleId, BottleApiModel apiBottle)
        {
            apiBottle.BottleId = bottleId;
            await _bottleValidator.ValidateAndThrowAsync(apiBottle);
            var domainBottle = _toDomainModelMapper.MapOne(apiBottle);
            

            //If service returns null, no Bottle has been found matching the passed id, so return a 404.
            if (await this._bottleService.GetBottleAsync(bottleId) == null) return NotFound(bottleId);

            await this._bottleService.UpdateBottleAsync(bottleId, domainBottle);

            //Return API Bottle that was passed
            return Ok(apiBottle);

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
            var wasDeleted = await this._bottleService.DeleteBottleAsync(bottleId);

            //If service returns false, no Bottle has been found matching the passed id, so return a 404.
            return wasDeleted == false ? NotFound(bottleId) : NoContent();
        }
    }
}
