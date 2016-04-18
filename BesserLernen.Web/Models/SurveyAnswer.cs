namespace BesserLernen.Web.Models
{
    public class SurveyAnswer
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public virtual SurveyQuestion Question { get; set; }

        public string UserId{ get; set; }
        public virtual ApplicationUser User { get; set; }
        
        public bool AnsweredYes { get; set; }
    }
}
