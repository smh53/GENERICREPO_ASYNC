using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Section
{
    public class Section : BaseEntities.BaseEntity
    {
        public string Name { get; set; }
        public int SectionNo { get; set; }
    }
}
