using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoDodoApi.Services.EFService
{
    [Table("Proceses")]
    public class EFProcess()
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [Column("Capability")]
        public string Capability { get; set; } = string.Empty;

        [Column("Opportunity")]
        public string Opportunity { get; set; } = string.Empty;

        [Column("Status")]
        public Status Status { get; set; }

        [Column("SalesLead")]
        public string SalesLead { get; set; } = string.Empty;

        [Column("HourlyRate")]
        public int HourlyRate { get; set; }

        [Column("LastUpdate")]
        public DateOnly LastUpdate { get; set; }

        [Column("GenerationDate")]
        public DateOnly GenerationDate { get; set; }
    }

    public enum Status
    {
        LOST,
        ASSIGNED,
        INTERVIEW,
        PROSPECT,
        OFFERED,
    }
}
