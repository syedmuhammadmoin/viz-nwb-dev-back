using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateHeldAssetForDisposal
    {
        [Required]
        public int Id { get; set; }
        //[Required]
       // public DateTime HeldForDisposalDate { get; set; }

    }
}
