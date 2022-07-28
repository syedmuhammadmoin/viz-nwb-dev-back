using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class FileUploadDto
    {
        public int Id { get; set; }
        public DocType DocType { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string UserName { get; set; }
        public string CreatedAt { get; set; }
    }
}
