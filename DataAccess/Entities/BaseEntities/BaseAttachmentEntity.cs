using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.BaseEntities
{
    public class BaseAttachmentEntity : IEntity
    {
        public int Id { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; }

        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public TimeSpan InsertTime { get; set; } = DateTime.Now.TimeOfDay;
    }
}
