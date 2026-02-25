using CoDodoApi.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoDodoApi.Services.EFService
{
    [Table("Processes")]
    public class EFProcess()
    {
        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [Column("Status")]
        public string Status { get; set; } = string.Empty;

        [Column("UpdatedDate")]
        public DateTimeOffset UpdatedDate { get; set; }

        [Column("CreatedDate")]
        public DateTimeOffset CreatedDate { get; set; }

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

    public static class CreateEFProcessExtensions
    {
        public static EFProcess ToEFProcess(this Process process)
        {
            return new EFProcess
            {
                Name = process.Name,
                Status = process.Status,
                UpdatedDate = process.UpdatedDate,
                CreatedDate = process.CreatedDate,
                UriForAssignment = process.Opportunity.UriForAssignment,
                Company = process.Opportunity.Company,
                Capability = process.Opportunity.Capability,
                NameOfSalesLead = process.Opportunity.NameOfSalesLead,
                HourlyRateInSEK = process.Opportunity.HourlyRateInSEK,
            };
        }

        public static Process ToProcess(this EFProcess efProcess, TimeProvider provider)
        {
            Opportunity opportunity = new(efProcess.UriForAssignment,
                efProcess.Company,
                efProcess.Capability,
                efProcess.NameOfSalesLead,
                efProcess.HourlyRateInSEK);
            return new Process(efProcess.Name,
                opportunity,
                efProcess.Status,
                efProcess.CreatedDate,
                efProcess.UpdatedDate,
                provider);
        }
    }
}
