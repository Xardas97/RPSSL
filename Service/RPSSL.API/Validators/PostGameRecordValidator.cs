using FluentValidation;

using Mmicovic.RPSSL.API.Models;

namespace Mmicovic.RPSSL.API.Validators
{
    public class PostGameRecordValidator : HttpValidator<GameRecordDTO>
    {
        public PostGameRecordValidator(Func<int, bool> isValidShapeId)
        {
            RuleFor(c => c.PlayerChoice).NotNull().WithName("player");
            RuleFor(c => c.PlayerChoice).Must(shapeId => shapeId.HasValue && isValidShapeId(shapeId.Value))
                                        .WithMessage("Chosen player shape does not exist");

            RuleFor(c => c.Id).Null();
            RuleFor(c => c.Result).Null();
            RuleFor(c => c.ComputerChoice).Null().WithName("computer");
        }
    }
}
