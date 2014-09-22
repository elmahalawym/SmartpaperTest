using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Facebook;
using SmartNewspaper;
using System.Web.Script.Serialization;
using System.Web.Services;
using SmartNewspaper.FeatureFactory;
using System.Diagnostics;
using Newtonsoft.Json;

namespace SmartNewspaper
{
    public partial class Default : System.Web.UI.Page
    {
        private static iEntities db = new iEntities();

        public string storyCount;

        List<SmartNewspaper.Item> loadedLatestNews = new List<SmartNewspaper.Item>();
        List<SmartNewspaper.Cluster> loadedRecommendedNews = new List<SmartNewspaper.Cluster>();

        protected void Page_Load(object sender, EventArgs e)
        {
            long userID;

            #region UserLoggedinOrNotLoggedIn

            if (HttpContext.Current.Request.Cookies["UserInfo"] == null)
            {
                //Label_LoggedInUserInformation.Text = "You're not logged in. ";
                //HyperLink_LogIn.Visible = true;
                Session["UserIsLoggedIn"] = false;
                Session["userID"] = null;
                userID = -1;

                Panel_UserNotLoggedIn.Visible = true;
                Panel_UserLoggedIn.Visible = false;
                //userPicture.BackImageUrl = new Uri("")

                label_loggedinindicator.Text = Session["UserIsLoggedIn"].ToString();
                Label_Welcome.Visible = false;

                counter.Visible = false;
            }
            else
            {
                User loggedinUser = HelperClass.checkAuthorizedUser(HttpContext.Current.Request.Cookies["UserInfo"]);
                //Label_LoggedInUserInformation.Text = "Welcome, " + loggedinUser.UserName + ".";
                //HyperLink_LogOut.Visible = true;

                Session["UserIsLoggedIn"] = true;
                Session["userID"] = loggedinUser.UserID;
                userID = loggedinUser.UserID;
                Panel_UserNotLoggedIn.Visible = false;
                Panel_UserLoggedIn.Visible = true;
                label_loggedinindicator.Text = Session["UserIsLoggedIn"].ToString();

                if (loggedinUser.UserPic == "")
                    userPicture.BackImageUrl = "images" + "/" + "anonymous_user.jpg";
                else
                    userPicture.BackImageUrl = loggedinUser.UserPic;


                Label_Welcome.Visible = true;
                HyperLink_Username.Text = loggedinUser.FirstName;

                counter.Visible = true;

            }
            #endregion

            #region first time loading the page
            if (!IsPostBack)
            {
                if (System.Web.HttpContext.Current.Session["EnableImages"] == null)
                {
                    Session["EnableImages"] = "false";
                }
                if ((string)Session["EnableImages"] == "false")
                {
                    imagesToggle.Text = "تفعيل الصور";
                }
                else if ((string)Session["EnableImages"] == "true")
                {
                    imagesToggle.Text = "إخفاء الصور";
                }

                hfEnableImages.Value = Session["EnableImages"].ToString();

                hfDateTimeNow.Value = DateTime.UtcNow.ToString();

                HelperClass.dataModel = new SmartNewspaper.iEntities();
                HelperClass.itemFactory = new SNWS.NewsItemFactory();
                HelperClass.storyFactory = new SNWS.NewsStoryFactory();
                //HelperClass.dataModel.UsePostTunneling = true;
                //HelperClass.dataModel.Format.UseJson();

                // loading the latest news
                loadedLatestNews = HelperClass.GetLatestNews(userID, 10);
                //loadedLatestNews = new List<SmartNewspaper.Item>()
                //{
                //    new SmartNewspaper.Item(){
                //        Title="test2", 
                //        URL="#", 
                //        DateOfItem= DateTime.Now, 
                //        CategoryID=1,
                //        ItemID= 123123123
                //    },

                //    new SmartNewspaper.Item(){
                //        Title="test2", 
                //        URL="#", 
                //        DateOfItem= DateTime.Now, 
                //        CategoryID=1,
                //        ItemID= 123123123
                //    },

                //    new SmartNewspaper.Item(){
                //        Title="test2", 
                //        URL="#", 
                //        DateOfItem= DateTime.Now, 
                //        CategoryID=1,
                //        ItemID= 123123123
                //    }

                //};
                Session["loadedNews"] = loadedLatestNews;

                // updating the left stream and its hidden field
                ListView_leftStream.DataSource = loadedLatestNews;
                ListView_leftStream.DataBind();
                hfFirstIDLatestNews.Value = loadedLatestNews.First().ItemID.ToString();
                //hfLastIDLatestNews.Value = loadedLatestNews.Last().ItemID.ToString();
                hfLeftStreamMode.Value = "LatestNews";


                // loading the latest stories
                //loadedRecommendedNews = new List<SmartNewspaper.Cluster>(HelperClass.GetLatestNewsStories(userID, 10));
                //if (Session["loadedNewsRecommended"] == null)
                //Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(loadedRecommendedNews);

                // updating the right stream and its hidden field
                //ListView_rightStream.DataSource = loadedRecommendedNews;
                //ListView_rightStream.DataBind();
                hfRightStreamMode.Value = "All";

            }
            #endregion

            #region if the user is logged in, update the filters section
            if (HttpContext.Current.Request.Cookies["UserInfo"] != null)
            {
                User loggedinUser = HelperClass.checkAuthorizedUser(HttpContext.Current.Request.Cookies["UserInfo"]);

                // Updating the count of Read Later items.
                counter.Text = HelperClass.getReadLaterCount((int)loggedinUser.UserID).ToString();

                // Get the user filters and show them in the list view
                var userFilters = HelperClass.getUserFilters((int)loggedinUser.UserID);
                ListView_userFilters.DataSource = userFilters;
                ListView_userFilters.DataBind();
            }
            #endregion
        }

        public static string startUpScript = "<script>loadHandlers();</script>";

        #region User Login

        protected void Button_Login_Click(object sender, EventArgs e)
        {
            if (TextBox_Password.Text.Trim().Length > 0 && TextBox_Username.Text.Trim().Length > 0)
            {
                User user = HelperClass.loginUser(TextBox_Username.Text.Trim(), TextBox_Password.Text.Trim());
                if (user != null)
                {
                    Panel_LoginFailed.Visible = false;
                    HttpCookie cookie = HelperClass.authorizeUser(user);
                    Response.Cookies.Add(cookie);
                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    Panel_LoginFailed.Visible = true;
                    return;
                }
            }
        }

        protected void Button_SignInWithFacebook_Click(object sender, EventArgs e)
        {
            FacebookClient fb = new FacebookClient();
            var loginURL = fb.GetLoginUrl(new
            {
                client_id = "631763456879630",
                redirect_uri = HelperClass.rootURL + "/Users/FacebookLogin.aspx",
                //redirect_uri = "http://interestori.com/Users/FacebookLogin.aspx",
                response_type = "code",
                scope = "read_stream,email"
            });

            Response.Redirect(loginURL.AbsoluteUri);
        }


        protected void registeration_Submit_Click(object sender, EventArgs e)
        {
            if (TextBox_Username_Register.Text.Trim().Length > 0 &&
                TextBox_Password_Register.Text.Trim().Length > 0 &&
                TextBox_FirstName_Register.Text.Trim().Length > 0 &&
                TextBox_LastName_Register.Text.Trim().Length > 0 &&
                TextBox_Email_Register.Text.Trim().Length > 0)
            {
                bool result = HelperClass.registerUser(new User()
                {
                    UserName = TextBox_Username_Register.Text.Trim(),
                    Password = TextBox_Password_Register.Text.Trim(),
                    FirstName = TextBox_FirstName_Register.Text.Trim(),
                    LastName = TextBox_LastName_Register.Text.Trim(),
                    Email = TextBox_Email_Register.Text.Trim()
                });
                if (result == false)
                {
                    registeration_failed.Visible = true;
                    return;
                }
                else
                {
                    registeration_failed.Visible = false;
                    User user = HelperClass.loginUser(TextBox_Username_Register.Text.Trim(), TextBox_Password_Register.Text.Trim());
                    HttpCookie cookie = HelperClass.authorizeUser(user);
                    Response.Cookies.Add(cookie);
                    Response.Redirect("~/Default.aspx");
                }
            }
            else
            {
                registeration_failed.Visible = true;
                return;
            }
        }

        #endregion

        #region Web Methods

        public string searchQuery
        {
            get { return searchBox.Text; }
        }

        [System.Web.Services.WebMethod()]
        public static string addPreference(string itemID, string type)
        {
            if ((bool)System.Web.HttpContext.Current.Session["UserIsLoggedIn"])
            {
                int userid = int.Parse(System.Web.HttpContext.Current.Session["userID"].ToString());
                int itemid = int.Parse(itemID);
                switch (type)
                {
                    case "read":
                        if (HelperClass.addPreference("Read", userid, itemid).Equals("Success"))
                            return "Server: success";
                        else
                            return "Server: fail";
                    case "share":
                        if (HelperClass.addPreference("Share", userid, itemid).Equals("Success"))
                            return "Server: success";
                        else
                            return "Server: fail";
                    case "readLater":
                        string response = HelperClass.addPreference("ReadLater", userid, itemid);
                        if (response.Equals("Success"))
                            return "Server: success";
                        else if (response.Equals("Duplicate"))
                            return "Server: duplicate";
                        else
                            return "Server: fail";

                    case "ignore":
                        if (HelperClass.addPreference("Ignore", userid, itemid).Equals("Success"))
                            return "Server: success";
                        else
                            return "Server: fail";
                    case "ignoreCluster":
                        if (HelperClass.addPreference("IgnoreCluster", userid, itemid).Equals("Success"))
                            return "Server: success";
                        else
                            return "Server: fail";
                    case "removeFromReadLater":
                        if (HelperClass.addPreference("removeFromReadLater", userid, itemid).Equals("Success"))
                            return "Server: success";
                        else
                            return "Server: fail";
                    default:
                        return "Server: unknown parameter";
                }
            }
            else
            {
                return "No user logged in.";
            }
        }

        //[System.Web.Services.WebMethod()]
        //public static string addToReadLater(int itemID)
        //{
        //    if ((bool)System.Web.HttpContext.Current.Session["UserIsLoggedIn"])
        //    {
        //        int userid = int.Parse(System.Web.HttpContext.Current.Session["userID"].ToString());
        //        string result = HelperClass.addToReadLater(userid, itemID);
        //        if (result.Equals("Success"))
        //        {
        //            return "Server: success";
        //        }
        //        else
        //        {
        //            return "Server: fail";
        //        }
        //    }
        //    else
        //    {
        //        return "No user logged in.";
        //    }
        //}

        [System.Web.Services.WebMethod()]
        public static string getReadLaterCount()
        {
            int userID = int.Parse(System.Web.HttpContext.Current.Session["userID"].ToString());
            return HelperClass.getReadLaterCount(userID).ToString();
        }

        [System.Web.Services.WebMethod()]
        public static string SearchQuery(string query)
        {
            System.Text.StringBuilder response = new System.Text.StringBuilder();
            try
            {
                var result = HelperClass.searchQuery(query);
                if (result.Count == 0)
                {
                    response.Append("<div class='searchloading search'>لا يوجد أخبار، حاول البحث باستخدام كلمات مختلفة.</div>");
                    return response.ToString();
                }
                else
                {
                    if (result.Count > 5)
                    {
                        result = result.GetRange(0, 5);
                    }
                    string searchResultLayout = "<div class=\"resultbox search\" data-Title=\"{1}\" data-Content=\"{2}\" data-Image=\"{3}\" data-ID=\"{4}\"  data-IDNewsSources=\"{5}\" data-url=\"{6}\" ><span class=\"searchImg\"><img src=\"{0}\"></span><p>{1}</p></div>";

                    foreach (var item in result)
                    {
                        string newsSourceImageURL;
                        string newsSourceName = HelperClass.resolveNewsSourceID(item.IDNewsSources);
                        newsSourceImageURL = HelperClass.resolveNewsSourceImage(newsSourceName);
                        response.Append(String.Format(searchResultLayout, "images/" + newsSourceImageURL, HttpUtility.HtmlEncode(item.Title), "", item.ImageUrl, item.ItemID, item.IDNewsSources, item.URL));
                    }

                    if (result.Count >= 5)
                    {
                        response.Append("<div id=\"resultfooter\" class=\"search\">اقرأ المزيد.</p></div>");
                    }
                    return response.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        [System.Web.Services.WebMethod()]
        public static string getItemContent(string itemID)
        {
            //string queryString = string.Format("GetItemContent()?itemID={0}", itemID);
            string query = GeneralFactory.GetItemContent(int.Parse(itemID));

            return query;
        }

        [System.Web.Services.WebMethod()]
        public static string fetchNewsItem(string itemID)
        {
            long itemID_asLong = long.Parse(itemID);
            var query = from item in db.Items
                        where item.ItemID == itemID_asLong
                        select item;
            var newsItem = query.FirstOrDefault();

            var result = new
            {
                Title = newsItem.Title,
                Image = newsItem.ImageUrl,
                ID = newsItem.ItemID,
                URL = newsItem.URL,
                Date = newsItem.DateOfItem,
                SourceID = newsItem.IDNewsSources,
                CalculatedTime = computeTimeFromDateTime(newsItem.DateOfItem)
            };
            return JsonConvert.SerializeObject(result);
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string toggleImages()
        {
            if ((string)(System.Web.HttpContext.Current.Session["EnableImages"]) == "false")
            {
                (System.Web.HttpContext.Current.Session["EnableImages"]) = "true";
            }
            else
            {
                (System.Web.HttpContext.Current.Session["EnableImages"]) = "false";
            }
            return "success";
        }
        #endregion

        #region Categories Buttons

        [System.Web.Services.WebMethod()]
        public static string LatestStories()
        {
            DateTime decayTime = DateTime.Today - TimeSpan.FromDays(120);

            //Get the latest clusters in deNotLazypending on the category
            var latestClusters = (from n in db.Clusters
                            where n.LastUpdate > decayTime
                            select n).OrderByDescending(x => x.Items.Count()).Take(10);

            List<object> lst = new List<object>();
            foreach (var cluster in latestClusters)
            {
                List<string> IDs_list = new List<string>();
                foreach (var i in cluster.Items)
                {
                    IDs_list.Add(i.ItemID.ToString());
                }

                var firstitem = cluster.Items.First();
                lst.Add(new
                {
                    nItems = cluster.Items.Count,
                    clusterId = cluster.ClusterID,
                    listOfIDs = string.Join(",", IDs_list),
                    firstItem = new
                    {
                        Title = firstitem.Title,
                        Image = firstitem.ImageUrl,
                        ID = firstitem.ItemID,
                        URL = firstitem.URL,
                        IDNewsSources = firstitem.IDNewsSources,
                        CalculatedTime = computeTimeFromDateTime(firstitem.DateOfItem)
                    }
                });
            }
            var res = JsonConvert.SerializeObject(lst);
            return res;
        }

        [System.Web.Services.WebMethod()]
        public static string PoliticsStories()
        {
            DateTime decayTime = DateTime.Today - TimeSpan.FromDays(120);

            //Get the latest clusters in deNotLazypending on the category
            var latestClusters = (from n in db.Clusters
                                  where n.LastUpdate > decayTime && n.CategoryID == 4
                                  select n).OrderByDescending(x => x.Items.Count()).Take(10);


            List<object> lst = new List<object>();
            foreach (var cluster in latestClusters)
            {
                List<string> IDs_list = new List<string>();
                foreach (var i in cluster.Items)
                {
                    IDs_list.Add(i.ItemID.ToString());
                }

                var firstitem = cluster.Items.First();
                lst.Add(new
                {
                    nItems = cluster.Items.Count,
                    clusterId = cluster.ClusterID,
                    listOfIDs = string.Join(",", IDs_list),
                    firstItem = new
                    {
                        Title = firstitem.Title,
                        Image = firstitem.ImageUrl,
                        ID = firstitem.ItemID,
                        URL = firstitem.URL,
                        IDNewsSources = firstitem.IDNewsSources,
                        CalculatedTime = computeTimeFromDateTime(firstitem.DateOfItem)
                    }
                });
            }
            var res = JsonConvert.SerializeObject(lst);
            return res;
        }

        [System.Web.Services.WebMethod()]
        public static string SportsStories()
        {

            DateTime decayTime = DateTime.Today - TimeSpan.FromDays(120);

            //Get the latest clusters in deNotLazypending on the category
            var latestClusters = (from n in db.Clusters
                                  where n.LastUpdate > decayTime && n.CategoryID == 2
                                  select n).OrderByDescending(x => x.Items.Count()).Take(10);

            List<object> lst = new List<object>();
            foreach (var cluster in latestClusters)
            {
                List<string> IDs_list = new List<string>();
                foreach (var i in cluster.Items)
                {
                    IDs_list.Add(i.ItemID.ToString());
                }

                var firstitem = cluster.Items.First();
                lst.Add(new
                {
                    nItems = cluster.Items.Count,
                    clusterId = cluster.ClusterID,
                    listOfIDs = string.Join(",", IDs_list),
                    firstItem = new
                    {
                        Title = firstitem.Title,
                        Image = firstitem.ImageUrl,
                        ID = firstitem.ItemID,
                        URL = firstitem.URL,
                        IDNewsSources = firstitem.IDNewsSources,
                        CalculatedTime = computeTimeFromDateTime(firstitem.DateOfItem)
                    }
                });
            }
            var res = JsonConvert.SerializeObject(lst);
            return res;
        }

        [System.Web.Services.WebMethod()]
        public static string TechStories()
        {

            DateTime decayTime = DateTime.Today - TimeSpan.FromDays(120);

            //Get the latest clusters in deNotLazypending on the category
            var latestClusters = (from n in db.Clusters
                                  where n.LastUpdate > decayTime && n.CategoryID == 3
                                  select n).OrderByDescending(x => x.Items.Count()).Take(10);

            List<object> lst = new List<object>();
            foreach (var cluster in latestClusters)
            {
                List<string> IDs_list = new List<string>();
                foreach (var i in cluster.Items)
                {
                    IDs_list.Add(i.ItemID.ToString());
                }

                var firstitem = cluster.Items.First();
                lst.Add(new
                {
                    nItems = cluster.Items.Count,
                    clusterId = cluster.ClusterID,
                    listOfIDs = string.Join(",", IDs_list),
                    firstItem = new
                    {
                        Title = firstitem.Title,
                        Image = firstitem.ImageUrl,
                        ID = firstitem.ItemID,
                        URL = firstitem.URL,
                        IDNewsSources = firstitem.IDNewsSources,
                        CalculatedTime = computeTimeFromDateTime(firstitem.DateOfItem)
                    }
                });
            }
            var res = JsonConvert.SerializeObject(lst);
            return res;
        }

        [System.Web.Services.WebMethod()]
        public static string GeneralStories()
        {

            DateTime decayTime = DateTime.Today - TimeSpan.FromDays(120);

            //Get the latest clusters in deNotLazypending on the category
            var latestClusters = (from n in db.Clusters
                                  where n.LastUpdate > decayTime && n.CategoryID == 1
                                  select n).OrderByDescending(x => x.Items.Count()).Take(10);

            List<object> lst = new List<object>();
            foreach (var cluster in latestClusters)
            {
                List<string> IDs_list = new List<string>();
                foreach (var i in cluster.Items)
                {
                    IDs_list.Add(i.ItemID.ToString());
                }

                var firstitem = cluster.Items.First();
                lst.Add(new
                {
                    nItems = cluster.Items.Count,
                    clusterId = cluster.ClusterID,
                    listOfIDs = string.Join(",", IDs_list),
                    firstItem = new
                    {
                        Title = firstitem.Title,
                        Image = firstitem.ImageUrl,
                        ID = firstitem.ItemID,
                        URL = firstitem.URL,
                        IDNewsSources = firstitem.IDNewsSources,
                        CalculatedTime = computeTimeFromDateTime(firstitem.DateOfItem)
                    }
                });
            }
            var res = JsonConvert.SerializeObject(lst);
            return res;
        }

        //protected void button_filter_All_Click(object sender, EventArgs e)
        //{
        //    long userID;
        //    try
        //    {
        //        userID = long.Parse(Session["userID"].ToString());
        //    }
        //    catch (Exception)
        //    {
        //        userID = -1;
        //    }

        //    var x = HelperClass.GetLatestNewsStories(userID, 10);
        //    ListView_rightStream.DataSource = x;
        //    ListView_rightStream.DataBind();
        //    hfRightStreamMode.Value = "All";

        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);
        //}

        //protected void button_filter_General_Click(object sender, EventArgs e)
        //{
        //    long userID;
        //    try
        //    {
        //        userID = long.Parse(Session["userID"].ToString());
        //    }
        //    catch (Exception)
        //    {
        //        userID = -1;
        //    }

        //    List<SmartNewspaper.Cluster> x = HelperClass.GetNewsStoriesbyCategory(userID, 1, 10);


        //    Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(x);
        //    ListView_rightStream.DataSource = x;
        //    ListView_rightStream.DataBind();
        //    hfRightStreamMode.Value = "General";

        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);
        //}

        //protected void button_filter_Sports_Click(object sender, EventArgs e)
        //{
        //    long userID;
        //    try
        //    {
        //        userID = long.Parse(Session["userID"].ToString());
        //    }
        //    catch (Exception)
        //    {
        //        userID = -1;
        //    }

        //    List<SmartNewspaper.Cluster> x = HelperClass.GetNewsStoriesbyCategory(userID, 2, 10);
        //    Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(x);
        //    ListView_rightStream.DataSource = x;

        //    Stopwatch sw1 = new Stopwatch();
        //    sw1.Start();

        //    ListView_rightStream.DataBind();

        //    sw1.Stop();
        //    Debug.WriteLine("Binding Sports List in: " + sw1.ElapsedMilliseconds);

        //    hfRightStreamMode.Value = "Sports";

        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);
        //}

        //protected void button_filter_Technology_Click(object sender, EventArgs e)
        //{
        //    long userID;
        //    try
        //    {
        //        userID = long.Parse(Session["userID"].ToString());
        //    }
        //    catch (Exception)
        //    {
        //        userID = -1;
        //    }

        //    List<SmartNewspaper.Cluster> x = HelperClass.GetNewsStoriesbyCategory(userID, 3, 10);
        //    Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(x);
        //    ListView_rightStream.DataSource = x;
        //    ListView_rightStream.DataBind();
        //    hfRightStreamMode.Value = "Technology";

        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);
        //}

        //protected void button_filter_Politics_Click(object sender, EventArgs e)
        //{
        //    long userID;
        //    try
        //    {
        //        userID = long.Parse(Session["userID"].ToString());
        //    }
        //    catch (Exception)
        //    {
        //        userID = -1;
        //    }

        //    List<SmartNewspaper.Cluster> x = HelperClass.GetNewsStoriesbyCategory(userID, 4, 10);
        //    Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(x);
        //    ListView_rightStream.DataSource = x;
        //    ListView_rightStream.DataBind();
        //    hfRightStreamMode.Value = "Politics";

        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);
        //}

        //protected void button_reloadRightStream_Click(object sender, EventArgs e)
        //{
        //    long userID;
        //    try
        //    {
        //        userID = long.Parse(Session["userID"].ToString());
        //    }
        //    catch (Exception)
        //    {
        //        userID = -1;
        //    }

        //    List<Cluster> result;

        //    switch (hfRightStreamMode.Value)
        //    {
        //        case "All":
        //            result = HelperClass.GetLatestNewsStories(userID, 10);
        //            Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(result);
        //            ListView_rightStream.DataSource = result;
        //            ListView_rightStream.DataBind();
        //            break;
        //        case "General":
        //            result = HelperClass.GetNewsStoriesbyCategory(userID, 1, 10);
        //            Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(result);
        //            ListView_rightStream.DataSource = result;
        //            ListView_rightStream.DataBind();
        //            break;
        //        case "Sports":
        //            result = HelperClass.GetNewsStoriesbyCategory(userID, 2, 10);
        //            Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(result);
        //            ListView_rightStream.DataSource = result;
        //            ListView_rightStream.DataBind();
        //            break;
        //        case "Technology":
        //            result = HelperClass.GetNewsStoriesbyCategory(userID, 3, 10);
        //            Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(result);
        //            ListView_rightStream.DataSource = result;
        //            ListView_rightStream.DataBind();
        //            break;
        //        case "Politics":
        //            result = HelperClass.GetNewsStoriesbyCategory(userID, 4, 10);
        //            Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(result);
        //            ListView_rightStream.DataSource = result;
        //            ListView_rightStream.DataBind();
        //            break;
        //        default:
        //            break;
        //    }

        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);

        //}


        #endregion

        #region Filters

        protected void addFilterButton_Click(object sender, EventArgs e)
        {
            long userID;
            try
            {
                userID = long.Parse(Session["userID"].ToString());
            }
            catch (Exception)
            {
                userID = -1;
            }

            string filterName = addFilterName.Text;
            if (filterName.Length > 0 && Session["userID"] != null)
            {
                var entities = HelperClass.dataModel;
                Filter filterExists = (from filters in entities.Filters
                                       where filters.Content == filterName && filters.UserID == userID
                                       select filters).SingleOrDefault();
                if (filterExists != null)
                {
                    return;
                }
                entities.Filters.Add(new SmartNewspaper.Filter()
                {
                    Content = filterName,
                    UserID = long.Parse(Session["userID"].ToString())

                });
                entities.SaveChanges();
                ListView_userFilters.DataSource = HelperClass.getUserFilters(int.Parse(Session["userID"].ToString()));
                ListView_userFilters.DataBind();

                string s = "<script>loadHandlers();$('#addFilterOverlayBG').hide('slide');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", s, false);

            }
            else return;
        }

        protected void button_customFilterDelete_Click(object sender, EventArgs e)
        {
            string filterID = hfCustomFilter.Value.ToString();
            string userid = System.Web.HttpContext.Current.Session["userID"].ToString();
            HelperClass.DeleteCustomFilter(userid, filterID);

            ListView_userFilters.DataSource = HelperClass.getUserFilters(int.Parse(Session["userID"].ToString()));
            ListView_userFilters.DataBind();

            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);
        }

        protected void button_customFilter_Click(object sender, EventArgs e)
        {
            int userid = int.Parse(System.Web.HttpContext.Current.Session["userID"].ToString());
            string filter = hfCustomFilter.Value.ToString();
            ListView_leftStream.DataSource = HelperClass.GetNewsOfCustomFilter(userid, filter);
            ListView_leftStream.DataBind();
            hfLeftStreamMode.Value = "CustomFilter";

            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);

        }

        #endregion

        #region Search

        protected void button_seeMore_Click(object sender, EventArgs e)
        {
            string q = ((TextBox)Page.FindControl("searchBox")).Text;
            ListView_leftStream.DataSource = HelperClass.searchQuery(q);
            ListView_leftStream.DataBind();
            hfLeftStreamMode.Value = "Search";


            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);

        }


        #endregion

        protected void button_read_later_Click(object sender, EventArgs e)
        {

            if (Session["userID"] != null)
            {
                ListView_leftStream.DataSource = HelperClass.getReadLater(int.Parse(Session["userID"].ToString()));
                ListView_leftStream.DataBind();
                hfLeftStreamMode.Value = "ReadLater";

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);
            }

        }

        protected void button_loadMoreNews_Click(object sender, EventArgs e)
        {
            long userID;
            try
            {
                userID = long.Parse(Session["userID"].ToString());
            }
            catch (Exception)
            {
                userID = -1;
            }

            string mode = hfLeftStreamMode.Value;
            switch (mode)
            {
                case "LatestNews":

                    loadedLatestNews = new List<SmartNewspaper.Item>((List<SmartNewspaper.Item>)Session["loadedNews"]);

                    List<SmartNewspaper.Item> moreNews = HelperClass.GetLatestNewsFromID(userID, int.Parse(loadedLatestNews.First().ItemID.ToString()));

                    List<SmartNewspaper.Item> result = new List<SmartNewspaper.Item>();
                    result.AddRange(new List<SmartNewspaper.Item>(moreNews));
                    result.AddRange(new List<SmartNewspaper.Item>(loadedLatestNews));

                    loadedLatestNews = new List<SmartNewspaper.Item>(result);

                    Session["loadedNews"] = new List<SmartNewspaper.Item>(loadedLatestNews);

                    ListView_leftStream.DataSource = loadedLatestNews;
                    ListView_leftStream.DataBind();

                    break;
                case "CustomFilter":
                    break;
                case "CustomNews":
                    break;
                case "Search":
                    break;
                case "ReadLater":
                    break;
                default:
                    break;
            }



            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);
        }

        //protected void button_loadMoreNews_Recommended_Click(object sender, EventArgs e)
        //{
        //    long userID;
        //    try
        //    {
        //        userID = long.Parse(Session["userID"].ToString());
        //    }
        //    catch (Exception)
        //    {
        //        userID = -1;
        //    }
        //    List<SmartNewspaper.Cluster> moreNews;
        //    switch (hfRightStreamMode.Value)
        //    {
        //        case "All":
        //            loadedRecommendedNews = new List<SmartNewspaper.Cluster>((List<SmartNewspaper.Cluster>)Session["loadedNewsRecommended"]);


        //            List<string> availableIDsAll = new List<string>(); ;
        //            foreach (var story in loadedRecommendedNews)
        //            {
        //                availableIDsAll.Add(story.ClusterID.ToString());
        //            }

        //            moreNews = HelperClass.GetLatestStoriesBeforeTime(userID, 10, String.Join(",", availableIDsAll));

        //            loadedRecommendedNews.AddRange(moreNews);

        //            //loadedRecommendedNews = new List<Cluster>(HelperClass.CheckStoriesForUpdates(loadedRecommendedNews));

        //            #region error check
        //            //List<DateTime> order = new List<DateTime>();
        //            //for (int i = 0; i < loadedRecommendedNews.Count - 1; i++)
        //            //{
        //            //    order.Add(loadedRecommendedNews[i].LastUpdate);
        //            //    if (loadedRecommendedNews[i].LastUpdate < loadedRecommendedNews[i + 1].LastUpdate)
        //            //    {
        //            //        throw new Exception("Stories not in the correct order. " + i.ToString() + ", " + (i + 1).ToString());
        //            //    }
        //            //}
        //            #endregion

        //            // saving the current news stories in the session
        //            Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(loadedRecommendedNews);

        //            // updating the right stream and its hidden field
        //            ListView_rightStream.DataSource = loadedRecommendedNews;
        //            ListView_rightStream.DataBind();
        //            break;
        //        case "General":
        //            loadedRecommendedNews = new List<SmartNewspaper.Cluster>((List<SmartNewspaper.Cluster>)Session["loadedNewsRecommended"]);

        //            List<string> availableIDsGeneral = new List<string>();
        //            foreach (var story in loadedRecommendedNews)
        //                availableIDsGeneral.Add(story.ClusterID.ToString());


        //            moreNews = HelperClass.GetNewsStoriesByCategoryBeforeTime(userID, 1, 10, String.Join(",", availableIDsGeneral));
        //            loadedRecommendedNews.AddRange(moreNews);
        //            Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(loadedRecommendedNews);
        //            ListView_rightStream.DataSource = loadedRecommendedNews;
        //            ListView_rightStream.DataBind();
        //            break;
        //        case "Sports":
        //            loadedRecommendedNews = new List<SmartNewspaper.Cluster>((List<SmartNewspaper.Cluster>)Session["loadedNewsRecommended"]);

        //            List<string> availableIDsSports = new List<string>();
        //            foreach (var story in loadedRecommendedNews)
        //                availableIDsSports.Add(story.ClusterID.ToString());

        //            moreNews = HelperClass.GetNewsStoriesByCategoryBeforeTime(userID, 1, 10, String.Join(",", availableIDsSports));
        //            loadedRecommendedNews.AddRange(moreNews);
        //            Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(loadedRecommendedNews);
        //            ListView_rightStream.DataSource = loadedRecommendedNews;
        //            ListView_rightStream.DataBind();
        //            break;
        //        case "Technology":
        //            loadedRecommendedNews = new List<SmartNewspaper.Cluster>((List<SmartNewspaper.Cluster>)Session["loadedNewsRecommended"]);

        //            List<string> availableIDsTechnology = new List<string>();
        //            foreach (var story in loadedRecommendedNews)
        //                availableIDsTechnology.Add(story.ClusterID.ToString());

        //            moreNews = HelperClass.GetNewsStoriesByCategoryBeforeTime(userID, 3, 10, String.Join(",", availableIDsTechnology));
        //            loadedRecommendedNews.AddRange(moreNews);
        //            Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(loadedRecommendedNews);
        //            ListView_rightStream.DataSource = loadedRecommendedNews;
        //            ListView_rightStream.DataBind();
        //            break;
        //        case "Politics":
        //            loadedRecommendedNews = new List<SmartNewspaper.Cluster>((List<SmartNewspaper.Cluster>)Session["loadedNewsRecommended"]);

        //            List<string> availableIDsPolitics = new List<string>();
        //            foreach (var story in loadedRecommendedNews)
        //                availableIDsPolitics.Add(story.ClusterID.ToString());

        //            moreNews = HelperClass.GetNewsStoriesByCategoryBeforeTime(userID, 4, 10, String.Join(",", availableIDsPolitics));
        //            loadedRecommendedNews.AddRange(moreNews);
        //            Session["loadedNewsRecommended"] = new List<SmartNewspaper.Cluster>(loadedRecommendedNews);
        //            ListView_rightStream.DataSource = loadedRecommendedNews;
        //            ListView_rightStream.DataBind();
        //            break;
        //        default:
        //            break;
        //    }

        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);

        //}

        protected void button_customNewsApply_Click(object sender, EventArgs e)
        {
            int categoryID = int.Parse(dropDownList_Category.SelectedValue);
            int sourceID = int.Parse(dropDownList_Source.SelectedValue);

            int userID;
            try
            {
                userID = int.Parse(Session["userID"].ToString());
            }
            catch (Exception)
            {
                userID = -1;
            }

            if (categoryID == -1 && sourceID == -1)
            {
                List<SmartNewspaper.Item> result = HelperClass.GetLatestNews(userID, 20);
                ListView_leftStream.DataSource = result;
                ListView_leftStream.DataBind();
                hfLeftStreamMode.Value = "LatestNews";
            }
            else
            {
                Stopwatch sw1 = new Stopwatch();

                sw1.Start();
                List<SmartNewspaper.Item> result = HelperClass.GetCustomNews(userID, categoryID, sourceID, 20);
                sw1.Stop();
                Debug.WriteLine("GetCustomNews finished in: " + sw1.ElapsedMilliseconds);


                ListView_leftStream.DataSource = result;

                sw1.Start();
                ListView_leftStream.DataBind();
                sw1.Stop();
                Debug.WriteLine("Binding finished in: " + sw1.ElapsedMilliseconds);

                hfLeftStreamMode.Value = "CustomNews";
            }

            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", startUpScript, false);

        }

        static string computeTimeFromDateTime(DateTime x)
        {
            TimeSpan diff = DateTime.UtcNow - x;
            if (diff < new TimeSpan(0))
            {
                return "حالاً";
            }
            if (diff.Days > DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
            {
                return "منذ أكثر من شهر";
            }

            else if (diff.Days > 21)
            {
                return "منذ أكثر من 3 أسابيع";
            }
            else if (diff.Days > 14)
            {
                return "منذ أكثر من أسبوعين";
            }
            else if (diff.Days > 7)
            {
                return "منذ أكثر من أسبوع";
            }
            else if (diff.Days == 7)
            {
                return "منذ أسبوع";
            }

            else if (diff.Days <= 6 && diff.Days >= 3)
            {
                return "منذ " + diff.Days + " أيام";
            }
            else if (diff.Days == 2)
            {
                return "منذ يومين";
            }
            else if (diff.Days == 1)
            {
                return "منذ يوم";
            }

            else if (diff.Hours >= 11)
            {
                return "منذ " + diff.Hours + " ساعة";
            }
            else if (diff.Hours <= 10 && diff.Hours >= 3)
            {
                return "منذ " + diff.Hours + " ساعات";
            }
            else if (diff.Hours == 2)
            {
                return "منذ ساعتين";
            }
            else if (diff.Hours == 1)
            {
                return "منذ ساعة";
            }

            else if (diff.Minutes >= 11)
            {
                return "منذ " + diff.Minutes + " دقيقة";
            }
            else if (diff.Minutes >= 3)
            {
                return "منذ " + diff.Minutes + " دقائق";
            }
            else if (diff.Minutes == 2)
            {
                return "منذ دقيقتين";
            }
            else if (diff.Minutes == 1)
            {
                return "منذ دقيقة";
            }

            else if (diff.Seconds >= 11)
            {
                return "منذ " + diff.Seconds + " ثانية";
            }
            else if (diff.Seconds >= 3)
            {
                return "منذ " + diff.Seconds + " ثواني";
            }
            else if (diff.Seconds > 1)
            {
                return "منذ ثانيتين";
            }
            else if (diff.Seconds == 1)
            {
                return "حالاً";
            }
            else
            {
                return "حالاً";
            }
        }
    }
}