using Business.Abstract;
using DataAccess.Context;
using DataAccess.Entities.Section;
using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class SectionService : GenericRepository<Section>, ISectionService
    {
        public SectionService(GenericRepoContext context): base(context)
        {
            
        }
    }
}
