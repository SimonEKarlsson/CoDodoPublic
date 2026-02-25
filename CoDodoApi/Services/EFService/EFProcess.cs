using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoDodoApi.Services.EFService
{
    [Table("Proceses")]
    public class EFProcess()
    {
        //Base properties of Process
        [Key]
        [Column("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [Column("Status")]
        public string Status { get; set; } = string.Empty;

        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        //base properties of Opportunity
        [Column("UriForAssignment")]
        public string UriForAssignment { get; set; } = string.Empty;

        [Column("Company")]
        public string Company { get; set; } = string.Empty;

        [Column("Capability")]
        public string Capability { get; set; } = string.Empty;

        [Column("NameOFSalesLead")]
        public string NameOfSalesLead { get; set; } = string.Empty;

        [Column("HourlyRateInSEK")]
        public int HourlyRateInSEK { get; set; }
    }
}
