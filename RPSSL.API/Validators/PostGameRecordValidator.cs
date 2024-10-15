using FluentValidation;

using Mmicovic.RPSSL.API.Models;

namespace Mmicovic.RPSSL.API.Validators
{
    public class PostGameRecordValidator : HttpValidator<GameRecord>
    {
        public PostGameRecordValidator(Func<int, bool> isValidShapeId)
        {
            RuleFor(c => c.PlayerChoice).NotNull()
                                        .WithMessage("Player shape is a mandatory field");
            RuleFor(c => c.PlayerChoice).Must(shapeId => shapeId.HasValue && isValidShapeId(shapeId.Value))
                                        .WithMessage("Chosen player shape does not exist");

            RuleFor(c => c.Id).Null()
                                          .WithMessage("ID field must be empty, ID is generated");

            RuleFor(c => c.Result).Null()
                                  .WithMessage("Results field must be empty, results are calculated");

            RuleFor(c => c.ComputerChoice).Null()
                                          .WithMessage("Computer field must be empty, computer choice is randomized");
        }
    }
}
