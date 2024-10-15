using FluentValidation;

using Mmicovic.RPSSL.API.Models;

namespace Mmicovic.RPSSL.API.Validators
{
    public class CredentialsValidator : HttpValidator<Credentials>
    {
        public CredentialsValidator()
        {
            RuleFor(c => c.UserName).NotNull().WithName("name");
            RuleFor(c => c.Password).NotNull().WithName("passphrase");
        }
    }
}
