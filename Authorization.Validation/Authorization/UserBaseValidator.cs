using Authorization.Contracts.Authorization;
using FluentValidation;

namespace Authorization.Validation.Authorization
{
    public class UserBaseValidator : AbstractValidator<UserBase>
    {
        public UserBaseValidator()
        {
            RuleFor(user => user.UserName)
                .NotEmpty()
                .MaximumLength(40);
            RuleFor(user => user.Password)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
