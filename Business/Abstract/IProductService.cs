﻿using Core.Utilities.Results;
using DataAccess.Entities.Product;
using DataAccess.EntityFramework;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService : IRepository<Product>
    {
        Task<IDataResult<string>> GetFirstProductName();

        //Task<IResult> CreateProduct(Product product, IFormFile[] attachments);
        //Task<IResult> UpdateProduct(int productId, Product product, IFormFile[] attachments);
      
        

    }
}
