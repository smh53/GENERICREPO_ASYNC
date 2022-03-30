using Business.Abstract;
using Business.Utilities.FileUpload;
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

        public override async Task<IResult> Create(Product product)
        {          

          
            if(product.Files != null && product.Files.Length > 0)
            {
                var productAttachments = new List<ProductAttachment>();
                var uploadedfileList = FileUpload.MultipleAttachmentUpload(product.Files, "Product");
                foreach (var uploadedFile in uploadedfileList)
                {
                    productAttachments.Add(new ProductAttachment { FileName = uploadedFile.FileName, FileType = uploadedFile.FileType, FilePath = uploadedFile.FilePath });
                }
                product.ProductAttachments = productAttachments;
            }
            await base.Create(product);
            
            return new SuccessResult();
        }

        public override async Task<IResult> Update(int productId, Product product)
        {

            if (product.Files != null && product.Files.Length > 0)
            {
                var productAttachments = new List<ProductAttachment>();
                var uploadedfileList = FileUpload.MultipleAttachmentUpload(product.Files, "Product");
                foreach (var uploadedFile in uploadedfileList)
                {
                    productAttachments.Add(new ProductAttachment { FileName = uploadedFile.FileName, FileType = uploadedFile.FileType, FilePath = uploadedFile.FilePath });
                }
                product.ProductAttachments = productAttachments;
            }


            await base.Update(productId,product);

           
            return new SuccessResult();
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
