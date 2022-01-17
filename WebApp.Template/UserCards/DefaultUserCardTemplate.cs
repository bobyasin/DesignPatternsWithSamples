using WebApp.Template.Models;

namespace WebApp.Template.UserCards
{
    public class DefaultUserCardTemplate : UserCardTemplate
    {
        protected override string SetFooter()
        {
            return string.Empty;
        }

        protected override string SetImage()
        {
            return "<img class='card-img- top' src='/Images/unknown_person.jpg'>";
        }
    }
}