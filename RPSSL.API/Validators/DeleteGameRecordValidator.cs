using FluentValidation;

namespace Mmicovic.RPSSL.API.Validators
{
    public class DeleteGameRecordValidator : HttpValidator<string>
    {
        public DeleteGameRecordValidator()
        {
            RuleFor(id => id).NotNull()
                             .WithMessage("ID must be provided");
            RuleFor(id => id).Custom((id, context) =>
            {
                if ((!(long.TryParse(id, out long value)) || value < 0))
                {
                    context.AddFailure($"ID must be a valid positive number");
                }
            });
        }
    }
}
