using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using YunHu.Lib.Mapper.AutoMapper;
using YunHu.Module.Role.Application.RoleService.ResultModels;
using YunHu.Module.Role.Domain.Role;

namespace YunHu.Module.Role.Application.RoleService
{
    public class MapperConfig : IMapperConfig
    {
        public void Bind(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RoleAddModel, RoleEntity>();
            cfg.CreateMap<RoleEntity, RoleUpdateModel>();
            cfg.CreateMap<RoleUpdateModel, RoleEntity>();
        }
    }
}
