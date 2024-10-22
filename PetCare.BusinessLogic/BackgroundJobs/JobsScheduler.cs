using Hangfire;
using PetCare.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.BusinessLogic.BackgroundJobs
{
    public static class JobsScheduler
    {
        public static void RegisterRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IUpcomingVaccinesService>("vaccine-reminder-job", (s) => s.SendVaccineReminder(), Cron.Daily(1));
        }
    }
}
