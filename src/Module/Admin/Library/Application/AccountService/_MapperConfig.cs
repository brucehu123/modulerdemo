using AutoMapper;
using YunHu.Lib.Mapper.AutoMapper;
using YunHu.Module.Admin.Application.AccountService.ResultModels;
using YunHu.Module.Admin.Application.AccountService.ViewModels;
using YunHu.Module.Admin.Domain.Account;
using YunHu.Module.Admin.Domain.Menu;

namespace YunHu.Module.Admin.Application.AccountService
{
    public class MapperConfig : IMapperConfig
    {
        public void Bind(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<MenuEntity, AccountMenuItem>();
            cfg.CreateMap<AccountAddModel, AccountEntity>().ForMember(m => m.Roles, opt => opt.Ignore());
            cfg.CreateMap<AccountEntity, AccountUpdateModel>();
            cfg.CreateMap<AccountUpdateModel, AccountEntity>().ForMember(m => m.Roles, opt => opt.Ignore());
        }
    }
}
