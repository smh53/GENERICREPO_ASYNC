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

        public override async Task<Core.Utilities.Results.IResult> Create(Product product)
        {          

          
            if(product.Files != null && product.Files.Length > 0)
            {
                var productAttachments = new List<ProductAttachment>();
                var uploadedFileList = FileUpload.MultipleAttachmentUpload(product.Files, "Product");
                foreach (var uploadedFile in uploadedFileList)
                {
                    productAttachments.Add(new ProductAttachment { FileName = uploadedFile.FileName, FileType = uploadedFile.FileType, FilePath = uploadedFile.FilePath });
                }
                product.ProductAttachments = productAttachments;
            }
           var result = await base.Create(product);
            if(result.Success)
                return new SuccessResult();
            return new ErrorResult(result.Message);
        }

        public override async Task<Core.Utilities.Results.IResult> Update(int id, Product product)
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


           var finalResult = await base.Update(id,product);
            if (finalResult.Success)
                return new SuccessResult();
            return new ErrorResult();


        }

        public override IDataResult<IQueryable<Product>> GetAll(Expression<Func<Product, bool>>? filter = null)
        {
            var result =  TableNoTracking.Include(f => f.ProductAttachments);
            return new SuccessDataResult<IQueryable<Product>>(result);
        }

        public async Task<IDataResult<string>> GetFirstProductName()
        {
            var result =await Table.FirstOrDefaultAsync();

            if (result == null)
                return new ErrorDataResult<string>();

            return  new SuccessDataResult<string>(result.Name," ");
        }
        public override IDataResult<Product> GetById(int id)
        {

            var result = TableNoTracking.Include(f => f.ProductAttachments).FirstOrDefault(f => f.Id == id);
            if (result == null)
                return new ErrorDataResult<Product>();

            return new SuccessDataResult<Product>(result);
        }


    }
}
