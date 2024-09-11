using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class AppPackgeLine : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Description { get; private set; }

        public int AppPackgeId { get; private set; }
        [ForeignKey("AppPackgeId")]
        public AppPackge AppPackge { get; private set; }

        public int AppUnitId { get; private set; }
        [ForeignKey("AppUnitId")]
        public AppUnit AppUnit { get; private set; }

        protected AppPackgeLine()
        {
        }
    }
}
