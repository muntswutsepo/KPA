using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KPAWeb.Models
{
    public class KPA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KPA_No { get; set; }
        [Required]
        [DisplayName("Description")]
        public string? KPA_Description { get; set; }
        [Required]
        [Range(0,100)]
        public int Weighting { get; set; }

        public ICollection<KPI> KPIs { get; set; }
        



    }
}