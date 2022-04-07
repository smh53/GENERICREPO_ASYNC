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

        public override async Task<IResult> Update(int id, ProductAttachment productAttachment)
        {

            if (productAttachment.File != null && productAttachment.File.Length > 0)
            {
              
               var uploadedFile =  FileUpload.SingleAttachmentUpload(productAttachment.File, "Product");

                var oldFile = GetById(productAttachment.Id).Data.FilePath;
                if (oldFile != null)
                    File.Delete(oldFile);
                else
                    return new ErrorResult();



                productAttachment.FileName = uploadedFile.FileName;
                productAttachment.FilePath = uploadedFile.FilePath;
                productAttachment.FileType = uploadedFile.FileType;
         

                await base.Update(id, productAttachment);
                return new SuccessResult();
            }
            return new ErrorResult();

            


            
        }

    }
}
