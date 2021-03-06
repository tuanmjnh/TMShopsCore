﻿using System;
using System.Collections.Generic;

namespace TMShopsCore.Models
{
    public partial class Users
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public string Images { get; set; }
        public int? Orders { get; set; }
        public string Roles { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastChangePassword { get; set; }
        public int? Flag { get; set; }
        public string Extras { get; set; }
    }
}
