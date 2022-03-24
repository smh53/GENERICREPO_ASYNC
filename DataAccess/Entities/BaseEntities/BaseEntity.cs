using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.BaseEntities
{
    public class BaseEntity : IEntity
    {
        public int Id { get; set ; }
        public DateTime InsertDate { get ; set; } = DateTime.Now;
        public TimeSpan InsertTime { get; set; } = DateTime.Now.TimeOfDay;
    }
}
