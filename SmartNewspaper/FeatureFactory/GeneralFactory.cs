using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SmartNewspaper.FeatureFactory
{
    public static class GeneralFactory
    {

        public static iEntities de = new iEntities();
        public static string GetItemContent(int itemID)
        {

            return (from i in de.ItemContents
                    where i.ItemID == itemID
                    select i.Content).FirstOrDefault();
        }


        public static string AddPreference(int userID, int itemID, string type)
        {

            Preference newPreference = new Preference();
            User _user = (from user in de.Users
                          where user.UserID == userID
                          select user).FirstOrDefault();


            if (type != "IgnoreCluster")
            {
                Item _item = (from item in de.Items
                              where item.ItemID == itemID
                              select item).FirstOrDefault();

                newPreference.UserID = _user.UserID;
                newPreference.ItemID = _item.ItemID;

            }

            //if (type != "Ignore") {
            //    //HANDLE NULL VALUES THAT EXIST 
            //    //Dictionary<string , double> profile = Utilities.GetUPVectorFromXML(_user.LongTermProfile);
            //    //Dictionary<string, double> item = Utilities.GetItemVectorFromXML(Utilities.GetItemVectorFromText(_item.Content));

            //}
            try
            {


                if (type == "Share")
                {
                    newPreference.Rating = GeneralFactory.PreferenceValue.ShareValue;
                }

                else if (type == "Read")
                {
                    newPreference.Rating = GeneralFactory.PreferenceValue.ReadValue;

                }

                else if (type == "Ignore")
                {
                    newPreference.Rating = GeneralFactory.PreferenceValue.IgnoreValue;
                    IQueryable<IgnoredItem> DuplicateIgnoreItem = (from t in de.IgnoredItems
                                                                   where t.ItemID == itemID && t.UserID == userID && t.IsCluster == false
                                                                   select t);

                    //Ignore the content of the cluster = ignore the features of the centroid vector
                    if (DuplicateIgnoreItem.Count() == 0)
                        de.IgnoredItems.Add(new IgnoredItem { IsCluster = false, ItemID = itemID, UserID = userID });

                    else return "Duplicate";
                }

                else if (type == "ReadLater")
                {

                    newPreference.Rating = PreferenceValue.ReadLaterValue;

                    IQueryable<ToReadList> DuplicateToReadItem = (from t in de.ToReadList
                                                                  where t.ItemID == itemID && t.UserID == userID
                                                                  select t);

                    if (DuplicateToReadItem.Count() == 0)
                        de.ToReadList.Add(new ToReadList { UserID = userID, ItemID = itemID });

                    else return "Duplicate";
                }


                else if (type == "IgnoreCluster")
                {


                    IQueryable<IgnoredItem> DuplicateIgnoreItem = (from t in de.IgnoredItems
                                                                   where t.ItemID == itemID && t.UserID == userID && t.IsCluster == true
                                                                   select t);

                    //Ignore the content of the cluster = ignore the features of the centroid vector
                    if (DuplicateIgnoreItem.Count() == 0)
                        de.IgnoredItems.Add(new IgnoredItem { IsCluster = true, ItemID = itemID, UserID = userID });

                    else return "Duplicate";



                }

                if (type != "IgnoreCluster")
                    _user.Preferences.Add(newPreference);
                de.SaveChanges();
                return "Success";
            }
            catch (Exception e)
            {
                de = new iEntities();
                //this.CurrentDataSource.ClearChanges();
                string exceptionMessage = "";
                if (e != null)
                {
                    exceptionMessage = "Exception In" + type + "\n" + e.ToString();
                    Debug.WriteLine(exceptionMessage);
                    return (exceptionMessage);
                }
                else return "Exception In" + type + "Message Couldn't be found";

            }
        }

        public static IQueryable<Filter> GetUserFilters(int userID)
        {

            var filters = from filter in de.Filters
                          where filter.UserID == userID
                          select filter;

            return filters;


        }

        public static class PreferenceValue
        {
            public static double ShareValue = 1.5;
            public static double ReadValue = 1;
            public static double IgnoreValue = -0.5;
            public static double DislikeValue = -1;
            public static double ReadLaterValue = 2;

        }

    }
}