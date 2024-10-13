using FluentValidation;
using FluentValidation.Results;

using Mmicovic.RPSSL.API.Exceptions;

namespace Mmicovic.RPSSL.API.Validators
{
    public abstract class HttpValidator<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var result = base.Validate(context);
            if (!result.IsValid)
                throw new HttpArgumentException(result.ToString());

            return result;
        }
    }
}
