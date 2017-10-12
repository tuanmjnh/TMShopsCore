using System;
using System.Collections.Generic;

namespace TMShopsCore.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupItem = new HashSet<GroupItem>();
        }

        public long Id { get; set; }
        public string AppKey { get; set; }
        public string IdKey { get; set; }
        public string Title { get; set; }
        public string ParentId { get; set; }
        public string ParentsId { get; set; }
        public int? Levels { get; set; }
        public string Details { get; set; }
        public int? Orders { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public int? Flag { get; set; }
        public string Extras { get; set; }

        public virtual ICollection<GroupItem> GroupItem { get; set; }
    }
}
