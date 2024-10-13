using FluentValidation;

using Mmicovic.RPSSL.Service;
using Mmicovic.RPSSL.API.Models;

namespace Mmicovic.RPSSL.API.Validators
{
    public class PostNewComputerGameValidator : HttpValidator<PlayCommand>
    {
        public PostNewComputerGameValidator()
        {
            RuleFor(c => c.ShapeId).NotNull()
                                   .WithMessage("Player shape is a mandatory field");
            RuleFor(c => c.ShapeId).GreaterThanOrEqualTo(GameManager.SHAPE_MIN)
                                   .WithMessage("Chosen player shape does not exist");
            RuleFor(c => c.ShapeId).LessThanOrEqualTo(GameManager.SHAPE_MAX)
                                   .WithMessage("Chosen player shape does not exist");
        }
    }
}
