using DataAccess.Entities.BaseEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.FileUpload
{
    public static class FileUpload
    {
        public static List<BaseAttachmentEntity> MultipleAttachmentUpload (IFormFile[] files, string directoryName )
        {
            
                var attachments = new  List<BaseAttachmentEntity>();
                foreach (var attachment in files)
                {
                    string attachmentType = Path.GetExtension(attachment.FileName);

                    var attachmentName =String.Concat(Guid.NewGuid().ToString(), attachmentType);

                    var saveAttachment = Path.Combine($"Uploads/{directoryName}", attachmentName);
                    var databaseAttachment = Path.Combine($"Uploads/{directoryName}", attachmentName);
                    var stream = new FileStream(saveAttachment, FileMode.Create);
                    attachment.CopyTo(stream); stream.Close();

                     attachments.Add(new BaseAttachmentEntity { FileName = attachmentName, FileType = attachmentType, FilePath = databaseAttachment });
                }
                 return attachments;
            
            
        }

        public static BaseAttachmentEntity SingleAttachmentUpload(IFormFile file, string directoryName)
        {
                string attachmentType = Path.GetExtension(file.FileName);
                var attachmentName = String.Concat(Guid.NewGuid().ToString(), attachmentType);
                var saveAttachment = Path.Combine($"Uploads/{directoryName}", attachmentName);
                var databaseAttachment = Path.Combine($"Uploads/{directoryName}", attachmentName);
                var stream = new FileStream(saveAttachment, FileMode.Create);
                file.CopyTo(stream); stream.Close();

            var uploadedFile = new BaseAttachmentEntity()
            {
                FileName = attachmentName,
                FileType = attachmentType,
                FilePath = databaseAttachment
            };

            return uploadedFile;


        }

    }
}
