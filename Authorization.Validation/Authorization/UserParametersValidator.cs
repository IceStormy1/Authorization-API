using Authorization.Contracts.Authorization;
using FluentValidation;

namespace Authorization.Validation.Authorization;

public class UserParametersValidator : AbstractValidator<UserParameters>
{
    /// <param name="userBaseValidator"><see cref="UserBaseValidator"/></param>
    public UserParametersValidator(IValidator<UserBase> userBaseValidator)
    {
        Include(userBaseValidator);

        RuleFor(user => user.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(70);
    }
}