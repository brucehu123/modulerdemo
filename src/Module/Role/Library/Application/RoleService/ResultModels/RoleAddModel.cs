using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YunHu.Module.Role.Application.RoleService.ResultModels
{
    public class RoleAddModel
    {
        [Required(ErrorMessage = "请输入角色名称")]
        public string Name { get; set; }

        public string Remarks { get; set; }

        public int Sort { get; set; }
    }
}
