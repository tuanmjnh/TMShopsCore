using System;
using System.Collections.Generic;

namespace TMShopsCore.Models
{
    public partial class SubRoles
    {
        public long Id { get; set; }
        public Guid RolesId { get; set; }
        public long ModulesId { get; set; }
        public int IsSelect { get; set; }
        public int IsInsert { get; set; }
        public int IsUpdate { get; set; }
        public int IsDelete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public int? Flag { get; set; }

        public virtual Modules Modules { get; set; }
        public virtual Roles Roles { get; set; }
    }
}
