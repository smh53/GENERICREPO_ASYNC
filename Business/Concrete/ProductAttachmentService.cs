using Business.Abstract;
using Business.Utilities.FileUpload;
using Core.Utilities.Results;
using DataAccess.Context;
using DataAccess.Entities.Product;
using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductAttachmentService : GenericRepository<ProductAttachment>, IProductAttachmentService
    {
        public ProductAttachmentService(GenericRepoContext context) : base(context)
        {

        }

        public override async Task<IResult> Update(int productAttachmentId, ProductAttachment productAttachment)
        {

            if (productAttachment.File != null && productAttachment.File.Length > 0)
            {
              
               var uploadedFile =  FileUpload.SingleAttachmentUpload(productAttachment.File, "Product");
                
                File.Delete(GetById(productAttachment.Id).Data.FilePath);

                productAttachment.FileName = uploadedFile.FileName;
                productAttachment.FilePath = uploadedFile.FilePath;
                productAttachment.FileType = uploadedFile.FileType;
         

                await base.Update(productAttachmentId, productAttachment);
                return new SuccessResult();
            }
            return new ErrorResult();

            


            
        }

    }
}
