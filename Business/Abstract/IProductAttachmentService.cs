using DataAccess.Entities.Product;
using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductAttachmentService : IRepository<ProductAttachment>
    {
    }
}
