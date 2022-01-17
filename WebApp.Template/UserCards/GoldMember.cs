using System.Text;

namespace WebApp.Template.UserCards
{
    public class GoldMember : UserCardTemplate
    {
        protected override string SetFooter()
        {
            var sb = new StringBuilder();
            sb.Append("<a href='#' class='btn btn-primary'>Send DM</a>");
            sb.Append("<a href='#' class='btn btn-warning'>Details</a>");

            return sb.ToString();
        }

        protected override string SetImage()
        {
            return $"<img class='card-img- top' src='{AppUser.ImageUrl}'>";
        }
    }
}