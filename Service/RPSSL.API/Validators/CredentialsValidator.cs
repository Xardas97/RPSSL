using FluentValidation;

using Mmicovic.RPSSL.API.Models;

namespace Mmicovic.RPSSL.API.Validators
{
    public class CredentialsValidator : HttpValidator<CredentialsDTO>
    {
        public CredentialsValidator()
        {
            RuleFor(c => c.UserName).NotNull().WithName("name");
            RuleFor(c => c.Password).NotNull().WithName("passphrase");
        }
    }
}
