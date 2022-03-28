using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Product
{
    public class Product : BaseEntities.BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        [NotMapped]
        public IFormFile[]? Files { get; set; }
        public ICollection<ProductAttachment>? ProductAttachments { get; set; }


    }
}
