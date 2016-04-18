using System.ComponentModel.DataAnnotations;

namespace BesserLernen.Web.Models
{
    public class SurveyQuestion
    {
        public int Id { get; set; }

        public int SortPosition { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
