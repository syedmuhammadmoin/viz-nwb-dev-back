using Domain.Base;
using Domain.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FileUpload : BaseEntity<int>
    {
        public int DocId { get; private set; }
        public DocType DocType { get; private set; }
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(100)]
        public string FileType { get; private set; }
        [MaxLength(10)]
        public string Extension { get; private set; }
        public string UserId { get; private set; }
        [ForeignKey("UserId")]
        public virtual User User { get; private set; }

        protected FileUpload()
        {

        }

        public FileUpload(int docId, DocType docType, string name, string fileType, string extension, string userId)
        {
            DocId = docId;
            DocType = docType;
            Name = name;
            FileType = fileType;
            Extension = extension;
            UserId = userId;
        }
    }
}
