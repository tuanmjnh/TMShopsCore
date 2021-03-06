﻿using System;
using System.Collections.Generic;

namespace TMShopsCore.Models
{
    public partial class Roles
    {
        public Roles()
        {
            SubRoles = new HashSet<SubRoles>();
        }

        public Guid Id { get; set; }
        public string AppKey { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Orders { get; set; }
        public string Details { get; set; }
        public string Modules { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public int Flag { get; set; }

        public virtual ICollection<SubRoles> SubRoles { get; set; }
    }
}
