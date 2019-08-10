using AutoMapper;
using YunHu.Lib.Mapper.AutoMapper;
using YunHu.Module.Admin.Application.MenuService.ResultModels;
using YunHu.Module.Admin.Application.MenuService.ViewModels;
using YunHu.Module.Admin.Domain.Menu;

namespace YunHu.Module.Admin.Application.MenuService
{
    public class MapperConfig : IMapperConfig
    {
        public void Bind(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<MenuAddModel, MenuEntity>();
            cfg.CreateMap<MenuEntity, MenuUpdateModel>();
            cfg.CreateMap<MenuUpdateModel, MenuEntity>();
            cfg.CreateMap<MenuEntity, MenuTreeResultModel>();
        }
    }
}
