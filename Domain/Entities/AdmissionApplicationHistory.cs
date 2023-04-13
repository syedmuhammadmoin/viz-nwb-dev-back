using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class AdmissionApplicationHistory : BaseEntity<int>
    {
        public DateTime HistoryDate { get; private set; }

        public int AdmissionApplicationId { get; private set; }
        [ForeignKey("AdmissionApplicationId")]
        public AdmissionApplication AdmissionApplication { get; private set; }

        public int BusinessPartnerId { get; private set; }
        [ForeignKey("BusinessPartnerId")]
        public BusinessPartner BusinessPartner { get; private set; }

        [MaxLength(200)]
        public string Remarks { get; set; }
        public ApplicationStatus Status { get; private set; }

        protected AdmissionApplicationHistory()
        {
        }

    }
}
