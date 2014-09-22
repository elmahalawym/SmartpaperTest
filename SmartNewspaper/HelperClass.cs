using SmartNewspaper.FeatureFactory;
using SNWS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SmartNewspaper
{
    public class HelperClass
    {
        //public static string rootURL = "http://localhost:7398";
        //public static string rootURL = "http://interestori.com";
        //public static string rootURL = "http://smartpaper.azurewebsites.net";
        public static string rootURL = "http://betainterestori.azurewebsites.net";
        
        public enum CustomExceptions
        {
            MoreThan1UserExistInTheDatabaseWithTheSameUsername,
            MoreThan1UserExistInTheDatabaseWithTheSameEmail,
        }

        public static SmartNewspaper.iEntities dataModel;
        public static NewsItemFactory itemFactory;
        public static NewsStoryFactory storyFactory;
        //public static SmartNewspaper.iEntities de;

        #region UserHandlingMethods

        public static bool usernameExists(string username)
        {
            var query = from user in dataModel.Users
                        where user.UserName == username
                        select user;
            int count = query.Count();
            if (count == 0)
            {
                return false;
            }
            else if (count == 1)
            {
                return true;
            }
            else
            {
                throw new Exception(CustomExceptions.MoreThan1UserExistInTheDatabaseWithTheSameUsername.ToString());
            }
        }

        public static bool emailExists(string email)
        {
            var query = from user in dataModel.Users
                        where user.Email == email
                        select user;
            int count = query.Count();
            if (count == 0)
            {
                return false;
            }
            else if (count == 1)
            {
                return true;
            }
            else
            {
                throw new Exception(CustomExceptions.MoreThan1UserExistInTheDatabaseWithTheSameEmail.ToString());
            }
        }

        public static User loginUser(string username, string password)
        {
            try
            {
                var query = from user in dataModel.Users
                            where user.UserName == username && user.Password == password
                            select user;
                int count = query.Count();
                if (count == 0)
                {
                    // User does not exist
                    return null;
                }
                else if (count == 1)
                {
                    SmartNewspaper.User loggedinUser = query.First();
                    return new User()
                    {
                        UserName = loggedinUser.UserName,
                        Password = loggedinUser.Password,
                        FirstName = loggedinUser.FirstName,
                        LastName = loggedinUser.LastName,
                        UserPic = loggedinUser.UserPic,
                        Email = loggedinUser.Email,
                        UserID = loggedinUser.UserID
                    };
                }
                else
                {
                    throw new Exception("More than 1 user with the same username.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public static bool registerUser(User user)
        {
            if (usernameExists(user.UserName) || emailExists(user.Email))
            {
                return false;
            }
            else
            {
                try
                {
                    SmartNewspaper.iEntities entities = dataModel;
                    entities.Users.Add(new SmartNewspaper.User()
                    {
                        UserName = user.UserName,
                        Password = user.Password,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        UserPic = user.UserPic,
                        FacebookProfile = user.FacebookProfile
                    });
                    entities.SaveChanges();
                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static HttpCookie authorizeUser(User user)
        {
            if (user == null)
            {
                return null;
            }
            FormsAuthenticationTicket myTicket = new FormsAuthenticationTicket(
                1,
                "UserInfo",
                DateTime.Now,
                DateTime.Now.AddDays(15),
                true,
                user.ToString()
                );
            string encryptedTicket = FormsAuthentication.Encrypt(myTicket);
            return new HttpCookie("UserInfo", encryptedTicket);
        }

        public static User checkAuthorizedUser(HttpCookie cookie)
        {
            FormsAuthenticationTicket decryptedTicket = FormsAuthentication.Decrypt(cookie.Value);
            List<string> userData = decryptedTicket.UserData.Split(new char[] { ';' }).ToList();
            return new User()
            {
                UserName = userData[0],
                Password = userData[1],
                FirstName = userData[2],
                LastName = userData[3],
                Email = userData[4],
                UserPic = userData[5],
                UserID = long.Parse(userData[6])
            };
        }

        public static User getLoggedInUser()
        {
            if (HttpContext.Current.Request.Cookies["UserInfo"] != null)
            {
                return checkAuthorizedUser(HttpContext.Current.Request.Cookies["UserInfo"]);

            }
            return null;
        }

        #endregion

        #region GettingNews

        public static List<SmartNewspaper.Item> GetLatestNews(long userID, int noOfItems)
        {
            IEnumerable<SmartNewspaper.Item> result_as_ienumerable = HelperClass.itemFactory.GetLatestNews(userID, noOfItems);
            return result_as_ienumerable.ToList<SmartNewspaper.Item>();
        }

        public static List<SmartNewspaper.Item> GetLatestNewsFromID(long userID, int startID)
        {
            IEnumerable<SmartNewspaper.Item> result_as_ienumerable = HelperClass.itemFactory.GetLatestNewsFromID(userID, startID);
            return result_as_ienumerable.ToList<SmartNewspaper.Item>();
        }


        public static List<SmartNewspaper.SNWebService.IItem> GetNewsOfCustomFilter(int userID, string filter)
        {
            SmartNewspaper.SNWebService.SNEntities entities = new SNWebService.SNEntities(new Uri("http://interestoriservice.cloudapp.net/SNDataService.svc/"));
            IEnumerable<SmartNewspaper.SNWebService.IItem> query = entities.CreateQuery<SmartNewspaper.SNWebService.IItem>("GetNewsOfCustomFilter").AddQueryOption("userID", userID).AddQueryOption("filter", '\'' + filter + '\'');
            try
            {
                return query.ToList<SmartNewspaper.SNWebService.IItem>();
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        //public static List<SmartNewspaper.Item> GetNewsOfCustomFilter(int userID, string filter)
        //{
        //    SmartNewspaper.SNWebService.SNEntities entities = new SNWebService.SNEntities(new Uri("http://interestoriservice.cloudapp.net/SNDataService.svc/"));
        //    IEnumerable<SmartNewspaper.Item> query = entities.CreateQuery<SmartNewspaper.Item>("GetNewsOfCustomFilter").AddQueryOption("userID", userID).AddQueryOption("filter", '\'' + filter + '\'');
        //    try
        //    {
        //        return query.ToList<SmartNewspaper.Item>();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        throw ex;
        //    }
        //}

        public static List<SmartNewspaper.Item> GetCustomNews(int userID, int categoryID, int sourceID, int noOfItems)
        {
            SmartNewspaper.SNWebService.ICategory x = new SNWebService.ICategory();
            var sw = new Stopwatch();
            sw.Start();
            Debug.WriteLine("Starting GetCustomNews... " + DateTime.Now);
            SmartNewspaper.iEntities entities = dataModel;
            IEnumerable<SmartNewspaper.Item> query = HelperClass.itemFactory.GetCustomNews(userID, categoryID, sourceID, noOfItems);
            try
            {
                Debug.Write("So far: ");
                Debug.WriteLine(sw.ElapsedMilliseconds);
                var result = query.ToList<SmartNewspaper.Item>();
                Debug.WriteLine("Finished...... @" + DateTime.Now);
                Debug.WriteLine("Exection Time: ");
                Debug.WriteLine(sw.ElapsedMilliseconds);
                Debug.WriteLine("****");
                return result;

            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        public static List<SmartNewspaper.SNWebService.IItem> searchQuery(string q)
        {
            SmartNewspaper.SNWebService.SNEntities entities = new SNWebService.SNEntities(new Uri("http://interestoriservice.cloudapp.net/SNDataService.svc"));
            var result = entities.CreateQuery<SmartNewspaper.SNWebService.IItem>("SearchNews").AddQueryOption("query", '\'' + q + '\'');

            return result.ToList<SmartNewspaper.SNWebService.IItem>();
        }


        public static List<SmartNewspaper.Cluster> GetLatestNewsStories(long userID, int noOfItems)
        {
            //    var query = dataModel.CreateQuery<SmartNewspaper.Cluster>("GetLatestNewsStories").AddQueryOption("userID", userID).AddQueryOption("noOfItems", noOfItems);
            //    var result = GetNewsStoriesFromClusters(query.ToList<SmartNewspaper.Cluster>());
            //    return result;
            //
            //string queryString = string.Format("GetLatestNewsStories()?userID={0} && noOfItems={1} && $expand=Items", userID, noOfItems);
            var query = storyFactory.GetLatestNewsStories(userID, noOfItems);
            return query.ToList<SmartNewspaper.Cluster>();
        }

        public static List<SmartNewspaper.Cluster> GetLatestStoriesBeforeTime(long userID, int noOfItems, string availableIDs)
        {
            //string queryString = string.Format("GetLatestStoriesBeforeTime()?userID={0} && noOfItems={1} && availableIDs ='{2}' && $expand=Items", userID, noOfItems, availableIDs);

            var query = storyFactory.GetTrendingStoriesBeforeTime(userID, noOfItems, availableIDs);
            var result = query.ToList<SmartNewspaper.Cluster>();
            return result;
        }

        public static List<SmartNewspaper.Cluster> GetNewsStoriesbyCategory(long userID, int categoryID, int noOfItems)
        {
            //string queryString = string.Format("GetNewsStoriesByCategory()?userID={0} && noOfItems={1} && categoryID ={2} && $expand=Items", userID, noOfItems, categoryID);

            //var query = dataModel.CreateQuery<SmartNewspaper.Cluster>("GetNewsStoriesByCategory").AddQueryOption("userID", userID).AddQueryOption("categoryID", categoryID).AddQueryOption("noOfItems", noOfItems);
            var query = storyFactory.GetTrendingNewsStoriesByCategory(userID, categoryID, noOfItems);
            var result = query.ToList<SmartNewspaper.Cluster>();
            return result;
        }

        public static List<SmartNewspaper.Cluster> GetNewsStoriesByCategoryBeforeTime(long userID, int categoryID, int noOfItems, string availableIDs)
        {
            string queryString = string.Format("GetNewsStoriesByCategoryBeforeTime()?userID={0} && categoryID ={1} && noOfItems={2} && availableIDs ='{3}' && $expand=Items", userID, categoryID, noOfItems, availableIDs);

            var query = storyFactory.GetTrendingStoriesByCategoryBeforeTime(userID, categoryID, noOfItems , availableIDs);
            var result = query.ToList<SmartNewspaper.Cluster>();
            return result;
        }


        public static List<SmartNewspaper.Cluster> CheckStoriesForUpdates(List<SmartNewspaper.Cluster> currentStories)
        {
            List<SmartNewspaper.Cluster> result = new List<SmartNewspaper.Cluster>();
            foreach (SmartNewspaper.Cluster story in currentStories)
            {
                int initialCount = story.Items.Count;
                SmartNewspaper.Cluster storyWithPossibleMoreNews = (from clusters in dataModel.Clusters
                                                                  where clusters.ClusterID == story.ClusterID
                                                                  select clusters).FirstOrDefault();
                int newCount = storyWithPossibleMoreNews.Items.Count;
                if (newCount > initialCount)
                {
                    result.Add(story);
                }
                else
                {
                    result.Add(story);
                }

            }
            return result;
        }

        #endregion


        public static bool DeleteCustomFilter(string userID, string filterID)
        {
            try
            {
                var filter = (from filters in dataModel.Filters
                              where filters.UserID == long.Parse(userID) && filters.FilterID == long.Parse(filterID)
                              select filters).FirstOrDefault();
                dataModel.Filters.Remove(filter);
                dataModel.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public static List<SmartNewspaper.Filter> getUserFilters(int userID)
        {
            var query = GeneralFactory.GetUserFilters(userID);
            return query.ToList<SmartNewspaper.Filter>();
        }


        public static string resolveNewsSourceID(long ID)
        {
            Dictionary<long, string> sources = new Dictionary<long, string>();
            sources.Add(1, "اليوم السابع");
            sources.Add(2, "جول");
            sources.Add(3, "المصري اليوم");
            sources.Add(4, "الجزيرة");
            sources.Add(5, "شبكة محيط");
            sources.Add(6, "BBC");
            sources.Add(7, "وكالة أنباء ONA");
            sources.Add(8, "جريدة الدستور");
            sources.Add(9, "aitnews");
            sources.Add(10, "دنيا النكنولوجيا");
            sources.Add(11, "التقنية بلا حدود");
            sources.Add(12, "sky news");
            sources.Add(13, "sasapost");
            sources.Add(14, "الشروق");
            sources.Add(15, "الشروق أون لاين");
            sources.Add(16, "العربي");
            sources.Add(17, "كيف تك");
            sources.Add(18, "أردرويد");
            sources.Add(19, "CNN");
            sources.Add(20, "المصريون");
            if (sources.ContainsKey(ID))
            {
                return sources[ID];
            }
            else
            {
                return null;
            }
        }

        public static string resolveNewsSourceImage(string newsSourceName)
        {
            switch (newsSourceName)
            {
                case "اليوم السابع":
                    return "yoom7.png";
                case "جول":
                    return "goal.png";
                case "المصري اليوم":
                    return "elmasry.png";
                case "الجزيرة":
                    return "jazera.png";
                case "شبكة محيط":
                    return "moheet.png";
                case "BBC":
                    return "bbc.png";
                case "وكالة أنباء ONA":
                    return "onaeg.png";
                case "جريدة الدستور":
                    return "eldostor.png";
                case "aitnews":
                    return "ait.png";
                case "دنيا النكنولوجيا":
                    return "donia-tech.png";
                case "التقنية بلا حدود":
                    return "unlim-tech.png";
                case "sky news":
                    return "skynews.png";
                case "sasapost":
                    return "sasapost.png";
                case "الشروق":
                    return "sherouk.png";
                case "الشروق أون لاين":
                    return "echrouk.png";
                case "العربي":
                    return "elarabi-elgaded.png";
                case "كيف تك":
                    return "keef-tech.png";
                case "أردرويد":
                    return "ardroid.png";
                case "CNN":
                    return "cnn-arabic.png";
                case "المصريون":
                    return "elmasryon.png";
                default:
                    return "";
            }
        }

        public static string addPreference(string type, int userID, int itemID)
        {
            try
            {
                if (type.Equals("removeFromReadLater"))
                {
                    try
                    {
                        var readLaterItem = (from item in dataModel.ToReadList
                                             where item.UserID == userID && item.ItemID == itemID
                                             select item).FirstOrDefault();
                        dataModel.ToReadList.Remove(readLaterItem);
                        dataModel.SaveChanges();
                        return "Success";
                    }
                    catch (Exception)
                    {
                        return "Failure";
                        throw;
                    }
                }
                else
                {
                    var newEntities = new SmartNewspaper.iEntities();
                    //newEntities.UsePostTunneling = true;
                    var result = GeneralFactory.AddPreference(userID, itemID, type);
                    string response = result;
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string addToReadLater(int userID, int itemID)
        {
            try
            {
                var query = from toreadlist in dataModel.ToReadList
                            where toreadlist.UserID == userID && toreadlist.ItemID == itemID
                            select toreadlist;
                int count = query.Count();
                if (count > 0)
                {
                    return "Fail, item already exists.";
                }
                else
                {
                    SmartNewspaper.iEntities entities = dataModel;
                    entities.ToReadList.Add(new SmartNewspaper.ToReadList()
                    {
                        UserID = userID,
                        ItemID = itemID
                    });
                    entities.SaveChanges();
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return "Fail";
                throw ex;
            }
        }

        public static int getReadLaterCount(int userID)
        {
            try
            {
                var query = from toreadlist in dataModel.ToReadList
                            where toreadlist.UserID == userID
                            select toreadlist;
                return query.Count();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }

        }

        public static List<SmartNewspaper.UserToReadList_Result> getReadLater(int userID)
        {
            Debug.WriteLine("Getting read later items for user.");
            var sw = new Stopwatch();
            sw.Start();
            var result = dataModel.UserToReadList(userID).ToList();
            Debug.WriteLine("Finished...... @" + DateTime.Now);
            Debug.WriteLine("Exection Time: " + sw.ElapsedMilliseconds.ToString());
            return result;
        }



        //public static List<SmartNewspaper.Item> getReadLater(int userID){

        //    try
        //    {
        //        var sw = new Stopwatch();
        //        sw.Start();
        //        Debug.WriteLine("getReadLater started... ");
        //        var query = from toreadlist in de.ToReadList
        //                    where toreadlist.UserID == userID
        //                    select toreadlist;
        //        List<SmartNewspaper.Item> result = new List<SmartNewspaper.Item>();
        //        foreach (var toReadListItem in query)
        //        {
        //            var x = from item in de.Items
        //                    where item.ItemID == toReadListItem.ItemID
        //                    select item;
        //            if (x.Count() > 0)
        //            {
        //                result.Add(x.First());
        //            }
        //        }
        //        Debug.WriteLine("Finished...... @" + DateTime.Now);
        //        Debug.WriteLine("Exection Time: " + sw.ElapsedMilliseconds.ToString());
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        throw ex;
        //    }
        
        //}
    }
}