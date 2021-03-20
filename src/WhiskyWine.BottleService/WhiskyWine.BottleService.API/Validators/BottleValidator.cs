using FluentValidation;
using System;
using WhiskyWine.BottleService.API.Models;
using WhiskyWine.BottleService.Domain.Enums;

namespace WhiskyWine.BottleService.API.Validators
{
    /// <summary>
    /// Class to perform validation on Bottles that are passed in to the API via the BottlesController.
    /// </summary>
    public class BottleValidator : AbstractValidator<BottleApiModel>
    {
        /// <summary>
        /// Constructs an instance of the BottleValidator.
        /// </summary>
        public BottleValidator()
        {
            //Specify the validation rules for the BottleValidator.
            RuleFor(bottle => bottle.Name).NotNull().NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(bottle => bottle.Region).NotNull().NotEmpty().WithMessage("Region cannot be empty.");
            RuleFor(bottle => bottle.AlcoholCategory).Must(BeAValidAlcoholCategory).WithMessage("Please specify a valid AlcoholCategory.");
        }

        /// <summary>
        /// Specifies a custom validation rule for the AlcoholCategory property.
        /// </summary>
        /// <param name="alcoholCategory">String to be validated against the AlcoholCategory enum.</param>
        /// <returns></returns>
        private bool BeAValidAlcoholCategory(string alcoholCategory)
        {
            //If string can be parsed into AlcoholCategory enum, the validation succeeds. Else it fails.
            if (Enum.TryParse(alcoholCategory, true, out AlcoholCategory _)) return true;
            return false;
        }
    }
}
