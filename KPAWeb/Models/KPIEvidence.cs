using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KPAWeb.Models
{
    public class KPIEvidence
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int Evidence_No { get; set; }

       public int KPI_Ref_No { get; set; }
        [Required]
       public string Months { get; set; }
        [Required]
       public DateTime Start_Date { get; set; }
        [Required]
        public DateTime End_Date { get; set; }
       [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
       public int No_Of_Days { get; set; }

       public int Own_Score { get; set; }

       public int Line_Manager_Score { get; set; }
      
       public int Weighting { get; set; }

       [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
       public double Final_Score { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public KPI KPI { get; set; }

    }
}
