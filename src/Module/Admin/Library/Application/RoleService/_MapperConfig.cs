using AutoMapper;
using YunHu.Lib.Mapper.AutoMapper;
using YunHu.Module.Admin.Application.RoleService.ViewModels;
using YunHu.Module.Admin.Domain.Role;
using YunHu.Module.Admin.Domain.RoleMenuButton;

namespace YunHu.Module.Admin.Application.RoleService
{
    public class MapperConfig : IMapperConfig
    {
        public void Bind(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RoleAddModel, RoleEntity>();
            cfg.CreateMap<RoleEntity, RoleUpdateModel>();
            cfg.CreateMap<RoleUpdateModel, RoleEntity>();
            cfg.CreateMap<RoleMenuButtonBindModel, RoleMenuButtonEntity>();
        }
    }
}
