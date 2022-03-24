using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Context;
using DataAccess.Entities.Product;
using DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductService : GenericRepository<Product>, IProductService
    {
        public ProductService(GenericRepoContext context) : base(context)
        {

        }
        public async Task<IDataResult<string>> GetFirstProductName()
        {
            var result =await Table.FirstOrDefaultAsync();
            

            return  new SuccessDataResult<string>(result.Name," ");
        }
    }
}
