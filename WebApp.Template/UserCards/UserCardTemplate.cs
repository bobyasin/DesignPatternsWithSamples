using System;
using System.Text;

using WebApp.Template.Models;

namespace WebApp.Template.UserCards
{
    public abstract class UserCardTemplate
    {
        public AppUser AppUser { get; set; }

        public void SetUser(AppUser appUser)
        {
            AppUser = appUser;
        }

        public string Build() // => Template Method
        {
            if (AppUser == null)
                throw new ArgumentNullException(nameof(AppUser));

            var sb = new StringBuilder();
            sb.Append("<div class='card'>");
            sb.Append(SetImage());
            sb.Append($@"<div class='card-body'>
                          <h5>{AppUser.UserName}</h5>
                          <p>{AppUser.Description}</p>");
            sb.Append(SetFooter());
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        protected abstract string SetFooter(); // => step 1

        protected abstract string SetImage(); // => step 2
    }
}