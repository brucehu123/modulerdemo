using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YunHu.Module.Role.Application.RoleService.ResultModels
{
    public class RoleUpdateModel : RoleAddModel
    {
        [Required(ErrorMessage = "请选择角色")]
        public Guid Id { get; set; }
    }
}
