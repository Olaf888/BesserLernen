using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace BesserLernen.Web.Models
{
    public static class SampleData
    {
        private static readonly string[] Question =
        {
                "Kannst du uns ein bisschen über dich erzählen?",
                "In welchen Bereichen siehst du deine Stärken?",
                "Was sind deine Schwächen?",
                "Wo siehst du dich selbst in fünf Jahren?",
                "Wie reagieren du auf Kritik?",
                "Was wäre für dich die ideale Situation am Arbeitsplatz?",
                "Wodurch motivieren du dich selbst?",
                "Kannst du bis 10 zählen?"
        };
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            if (!context.SurveyQuestions.Any())
            {
                int idx = 0;
                foreach (var q in Question)
                    context.SurveyQuestions.Add(new SurveyQuestion {SortPosition = ++idx, Title = q});

                context.SaveChanges();
            }
        }
    }
}
