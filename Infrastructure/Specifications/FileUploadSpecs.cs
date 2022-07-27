using Domain.Constants;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class FileUploadSpecs : BaseSpecification<FileUpload>
    {
        public FileUploadSpecs(int docId, DocType docType)
            : base(x => x.DocId == docId && x.DocType == docType)
        {
            AddInclude(i => i.User);
        }
    }
}
