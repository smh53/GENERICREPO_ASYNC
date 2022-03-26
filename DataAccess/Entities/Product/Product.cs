﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Product
{
    public class Product : BaseEntities.BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<ProductAttachment>? ProductAttachments { get; set; }


    }
}
