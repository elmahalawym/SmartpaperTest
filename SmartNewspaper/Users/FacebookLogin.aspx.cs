using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using SmartNewspaper.SNWebService;
using Newtonsoft.Json;
using System.Diagnostics;


namespace SmartNewspaper.Users
{
    public partial class FacebookLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != null)
            {
                string accessCode = Request.QueryString["code"].ToString();

                var fb = new FacebookClient();

                // throws OAuthException 
                dynamic result = fb.Post("oauth/access_token", new
                {

                    client_id = "631763456879630",

                    client_secret = "0377de47eb135e4453dba95ff6331219",

                    redirect_uri = HelperClass.rootURL + "/Users/FacebookLogin.aspx",

                    code = accessCode

                });

                var accessToken = result.access_token;
                var expires = result.expires;

                Session["accessToken"] = accessToken;
                Session["expries"] = expires;

                fb.AccessToken = accessToken;

                // Retrieving basic information about the user

                dynamic info = fb.Get("me");
                dynamic info_pic = fb.Get("me/picture?redirect=0&type=large");
                // Extracting the data
                SmartNewspaper.User fbUser = new SmartNewspaper.User()
                {
                    FirstName = info.first_name,
                    LastName = info.last_name,
                    Password = "",
                    Email = info.email,
                    UserName = info.email,
                    UserPic = info_pic.data.url
                };

                // If the user is already registered, log him in
                if (HelperClass.usernameExists(fbUser.UserName))
                {
                    User loggedInUser = HelperClass.loginUser(fbUser.UserName, fbUser.Password);
                    HttpCookie cookie = HelperClass.authorizeUser(loggedInUser);
                    Response.Cookies.Add(cookie);
                    Response.Redirect(HelperClass.rootURL);
                    return;
                }
                else
                {
                    // else, extract the needed data, register him, then log him in


                    #region Retrieving Liked Pages

                    //dynamic likedPages_JSON = fb.Get("me/likes?limit=1000&fields=name,category&&locale=ar_AR");

                    //List<FacebookPage> listOfFacebookPages = new List<FacebookPage>();

                    //foreach (var page in likedPages_JSON.data)
                    //{
                    //    listOfFacebookPages.Add(new FacebookPage()
                    //    {
                    //        Name = page.name,
                    //        Category = page.category
                    //    });
                    //}

                    #endregion

                    #region Retrieving Last 20 Posts

                    //dynamic last20Posts = fb.Get("me/posts?limit=50");

                    //List<FacebookPost> listOfFacebookPosts = new List<FacebookPost>();

                    //foreach (var post in last20Posts.data)
                    //{
                    //    if (post.story == null)
                    //    {
                    //        listOfFacebookPosts.Add(new FacebookPost()
                    //        {
                    //            message = post.message,
                    //            link = post.link,
                    //            status_type = post.status_type,
                    //            type = post.type
                    //        });
                    //    }
                    //}
                    #endregion

                    #region making the User_Profile object and adding it to the User object
                    //FacebookData fbData_Csharp = new FacebookData()
                    //{
                    //    likedPages = listOfFacebookPages,
                    //    recentPosts = listOfFacebookPosts
                    //};

                    //string fbData_JSON = JsonConvert.SerializeObject(fbData_Csharp).ToString();
                    
                    //fbUser.FacebookProfile = fbData_JSON;
                    #endregion

                    #region Registering the user
                    try
                    {
                        fbUser.UserName = fbUser.UserName.ToLower();
                        iEntities entity = HelperClass.dataModel;
                        entity.Users.Add(fbUser);
                        entity.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        throw ex;
                    }
                    #endregion

                    #region Logging the user in

                    User loggedInUser = HelperClass.loginUser(fbUser.UserName, fbUser.Password);
                    HttpCookie cookie = HelperClass.authorizeUser(loggedInUser);
                    Response.Cookies.Add(cookie);
                    Response.Redirect(HelperClass.rootURL);

                    #endregion
                }

            }
        }
    }

    public class FacebookPage
    {
        public string Category;
        public string Name;
    }

    public class FacebookPost
    {
        public string message;
        public string link;
        public string status_type;
        public string type;
    }

    public class FacebookData
    {
        public List<FacebookPage> likedPages { get; set; }
        public List<FacebookPost> recentPosts { get; set; }
    }

}