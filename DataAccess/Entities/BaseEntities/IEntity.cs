using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.BaseEntities
{
    public interface IEntity
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public TimeSpan InsertTime { get; set; }

    }
}
