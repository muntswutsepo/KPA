using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KPAWeb.Models
{
    public class KPI
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KPI_No { get; set; }

        public int KPA_Ref_No { get; set; }
     
        [Required]
        [DisplayName("Description")]
        public string? KPI_Description { get; set; }

        [Required]
        [Range(0, 100)]
        public int Weighting { get; set;}

        public int Total_Weight { get; set; }

        [Required]
        public string Target { get; set; }

        public KPA KPA { get; set; }

        public ICollection<KPIEvidence> KPIEvidences { get; set; }


    }
}
