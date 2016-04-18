using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BesserLernen.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<SurveyAnswer> Answers { get; set; } = new List<SurveyAnswer>();
    }
}
