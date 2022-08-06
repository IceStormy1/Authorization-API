using Authorization.Contracts.Authorization;
using FluentValidation;

namespace Authorization.Validation.Authorization
{
    public class AuthenticateParametersValidator : AbstractValidator<AuthenticateParameters>
    {
        /// <param name="userBaseValidator"><see cref="UserBaseValidator"/></param>
        public AuthenticateParametersValidator(IValidator<UserBase> userBaseValidator)
        {
            Include(userBaseValidator);
        }
    }
}
