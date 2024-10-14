using FluentValidation;

using Mmicovic.RPSSL.API.Models;

namespace Mmicovic.RPSSL.API.Validators
{
    public class PostNewComputerGameValidator : HttpValidator<PlayCommand>
    {
        public PostNewComputerGameValidator(Func<int, bool> isValidShapeId)
        {
            RuleFor(c => c.ShapeId).NotNull()
                                   .WithMessage("Player shape is a mandatory field");
            RuleFor(c => c.ShapeId).Must(shapeId => shapeId.HasValue && isValidShapeId(shapeId.Value))
                                   .WithMessage("Chosen player shape does not exist");
        }
    }
}
