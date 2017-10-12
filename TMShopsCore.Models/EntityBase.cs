using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMShopsCore.Models
{
    public interface IEntityBase
    {
        //int Id { get; set; }
        string CreatedBy { get; set; }
        DateTime? CreatedAt { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
        string DeleteBy { get; set; }
        DateTime? DeleteAt { get; set; }
    }
    public class EntityBase
    {
        //int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}
