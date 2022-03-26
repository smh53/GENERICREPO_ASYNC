using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Context;
using DataAccess.Entities.Product;
using DataAccess.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductService : GenericRepository<Product>, IProductService
    {
     
        public ProductService(GenericRepoContext context) : base(context)
        {
           
        }

        public async Task<IResult> CreateProduct(Product product, IFormFile[] attachments)
        {
            
            if (attachments != null && attachments.Length > 0)
            {
                var productAttachments = new List<ProductAttachment>();
                foreach (var attachment in attachments)
                {
                    string attachmentType = Path.GetExtension(attachment.FileName);

                    var attachmentName = String.Concat(DateTime.Now.Millisecond.ToString(), "-", attachment.FileName);

                    var saveAttachment = Path.Combine("Uploads/Product", attachmentName);
                    var databaseAttachment = Path.Combine("Uploads/Product", attachmentName);
                    var stream = new FileStream(saveAttachment, FileMode.Create);
                    attachment.CopyTo(stream); stream.Close();

                    productAttachments.Add(new ProductAttachment { File = databaseAttachment, FileName = attachmentName, FileType = attachmentType });
                }
                product.ProductAttachments = productAttachments;
            }
             var result = await Create(product);

            if(result.Success)
                return new SuccessResult();
            else
                return new ErrorResult();
        }

        public override IDataResult<IQueryable<Product>> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            var result =  TableNoTracking.Include(f => f.ProductAttachments);
            return new SuccessDataResult<IQueryable<Product>>(result);
        }

        public async Task<IDataResult<string>> GetFirstProductName()
        {
            var result =await Table.FirstOrDefaultAsync();
            

            return  new SuccessDataResult<string>(result.Name," ");
        }
        public override IDataResult<Product> GetById(int id)
        {

            var result = TableNoTracking.Include(f => f.ProductAttachments).FirstOrDefault(f => f.Id == id);
            return new SuccessDataResult<Product>(result);
        }


    }
}
