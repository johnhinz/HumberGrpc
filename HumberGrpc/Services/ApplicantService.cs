using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HumberGrpc
{
    public class ApplicantService : Applicant.ApplicantBase
    {
        private readonly ILogger<ApplicantService> _logger;

        public ApplicantService(ILogger<ApplicantService> logger)
        {
            _logger = logger;
        }

        public override 
            Task<ApplicantProfile> GetApplicantProfile(Id request, ServerCallContext context)
        {
            ApplicantContext appContext = new ApplicantContext();

            var result = 
                appContext.
                Applicants.
                Where(a => a.Id == Guid.Parse(request.Id_)).
                FirstOrDefault();

            return new Task<ApplicantProfile>(() =>
               {
                   return new ApplicantProfile()
                   {
                       Id = result.Id.ToString(),
                       Login = result.Login.ToString(),
                       Salary = (double)result.Current_Salary,
                       Rate = (double)result.Current_Rate,
                       Currency = result.Currency,
                       Country = result.Country_Code,
                       Province = result.State_Province_Code
                   };
               });
        }

        public class ApplicantContext : DbContext
        {

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.
                    UseSqlServer
                    (@"Data Source=CSHARPHUMBER\HUMBERBRIDGING;Initial Catalog=JOB_PORTAL_DB;Integrated Security=True;");
                base.OnConfiguring(optionsBuilder);
            }

            public DbSet<ApplicantProfilePoco> Applicants { get; set; }
        }


        [Table("Applicant_Profiles")]
        public class ApplicantProfilePoco
        {
            public Guid Id { get; set; }
            public Guid Login { get; set; }
            public decimal Current_Salary  { get; set; }
            public decimal Current_Rate { get; set; }
            public string Currency { get; set; }
            public string Country_Code { get; set; }
            public string State_Province_Code { get; set; }
        }

    }
}
