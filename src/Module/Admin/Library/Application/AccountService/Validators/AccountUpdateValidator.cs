using FluentValidation;
using YunHu.Lib.Utils.Core.Extensions;
using YunHu.Module.Admin.Application.AccountService.ViewModels;

namespace YunHu.Module.Admin.Application.AccountService.Validators
{
    public class AccountUpdateValidator : AbstractValidator<AccountUpdateModel>
    {
        public AccountUpdateValidator()
        {
            RuleFor(x => x.Email).EmailAddress().When(x => x.Email.NotNull()).WithMessage("请输入正确的邮箱地址");
        }
    }
}
