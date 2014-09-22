using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartNewspaper.Models
{
    public class User
    {

        public User()
        {
            this.Preferences = new HashSet<Preference>();
            this.ToReadLists = new HashSet<ToReadList>();
            this.NewsSources = new HashSet<NewsSource>();
        }

        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ssd { get; set; }
        public string FacebookProfile { get; set; }

        public string UserPic { get; set; }

        public virtual ICollection<Preference> Preferences { get; set; }
        public virtual ICollection<ToReadList> ToReadLists { get; set; }
        public virtual ICollection<NewsSource> NewsSources { get; set; }

        public override string ToString()
        {
            return string.Join(";", new string[] { UserName, Password, FirstName, LastName, Email, UserPic, UserID.ToString()});
        }
    }
}