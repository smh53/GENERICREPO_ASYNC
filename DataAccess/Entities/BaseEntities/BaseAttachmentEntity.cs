using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.BaseEntities
{
    public class BaseAttachmentEntity : IEntity
    {
        public string File { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int Id { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public TimeSpan InsertTime { get; set; } = DateTime.Now.TimeOfDay;
    }
}
