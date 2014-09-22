using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using SmartNewspaper;

namespace SNWS
{
    public class NewsStoryFactory
    {

        public static iEntities de;
        
        public NewsStoryFactory()
        {
            de = new iEntities();
           
        }
       
        public IQueryable<Cluster> GetTrendingNewsStories(long userID, int noOfItems)
        {

            DateTime decayTime = DateTime.Today - TimeSpan.FromDays(365);
            List<long> ignoredItems = new List<long>();
            if (userID != -1)
            {
                ignoredItems = GetIgnoredStories(userID).ToList();
                if (ignoredItems.Count == 0)
                    ignoredItems = new List<long>();

            }

            //Get the latest clusters in deNotLazypending on the category
            var clusters = (from n in de.Clusters
                            where n.LastUpdate > decayTime && !ignoredItems.Contains(n.ClusterID)
                            select n).OrderByDescending(x => x.Items.Count()).Take(noOfItems);
            return clusters;

        }


        public IQueryable<Cluster> GetTrendingNewsStoriesByCategory(long userID, int categoryID, int noOfItems)
        {
            DateTime decayTime = DateTime.Today - TimeSpan.FromDays(365);
            List<long> ignoredItems = new List<long>();
            if (userID != -1)
            {
                ignoredItems = GetIgnoredStories(userID).ToList();
                if (ignoredItems.Count == 0)
                    ignoredItems = new List<long>();

            }

            //Get the latest clusters in deNotLazypending on the category
            var clusters = (from n in de.Clusters
                            where n.LastUpdate > decayTime && !ignoredItems.Contains(n.ClusterID) && n.CategoryID == categoryID
                            select n).OrderByDescending(x => x.Items.Count()).Take(noOfItems);
            return clusters;

        }

        public IQueryable<Cluster> GetTrendingStoriesBeforeTime(long userID, int noOfItems, string availableIDs)
        {

            DateTime decayTime = DateTime.Today - TimeSpan.FromDays(365);
            List<long> ignoredItems = new List<long>();
            if (userID != -1)
            {
                ignoredItems = GetIgnoredStories(userID).ToList();
                if (ignoredItems.Count == 0)
                    ignoredItems = new List<long>();
            }

            List<long> SentIDs = ExtractIDs(availableIDs);

            var clusters = (from c in de.Clusters
                            where (c.LastUpdate > decayTime && !ignoredItems.Contains(c.ClusterID) && !SentIDs.Contains(c.ClusterID))
                            select c).OrderByDescending(x => x.Items.Count()).Take(noOfItems);
            return clusters;

        }

        public IQueryable<Cluster> GetTrendingStoriesByCategoryBeforeTime(long userID, int categoryID , int noOfItems, string availableIDs)
        {

            DateTime decayTime = DateTime.Today - TimeSpan.FromDays(365);
            List<long> ignoredItems = new List<long>();
            if (userID != -1)
            {
                ignoredItems = GetIgnoredStories(userID).ToList();
                if (ignoredItems.Count == 0)
                    ignoredItems = new List<long>();
            }

            List<long> SentIDs = ExtractIDs(availableIDs);

            var clusters = (from c in de.Clusters
                            where (c.CategoryID == categoryID && c.LastUpdate > decayTime && !ignoredItems.Contains(c.ClusterID) && !SentIDs.Contains(c.ClusterID))
                            select c).OrderByDescending(x => x.Items.Count()).Take(noOfItems);
            return clusters;

        }


        public List<long> ExtractIDs(string ClusterIDs) {

            string [] ids = ClusterIDs.Split(',');
            List<long> idsToReturn = new List<long>();
            for (int i = 0; i < ids.Count(); i++)
            {
                idsToReturn.Add(long.Parse(ids[i]));
            }
            return idsToReturn.ToList();
        }

        public IQueryable<Cluster> GetLatestStoriesAfterTime(long userID, int noOfItems, string lastupdate)
        {
            List<long> ignoredItems = new List<long>();
            if (userID != -1)
            {
                ignoredItems = GetIgnoredStories(userID).ToList();
                if (ignoredItems.Count == 0)
                    ignoredItems = new List<long>();

            }

            DateTime updatetime = DateTime.Parse(lastupdate);
            var clusters = (from c in de.Clusters
                            where (c.LastUpdate > updatetime && !ignoredItems.Contains(c.ClusterID))
                            select c);
            return clusters;

        }

        //News Story is a custom created specifically for the website only.
        public IQueryable<Cluster> GetNewsStoriesByCategory(long userID , int categoryID, int noOfItems)
        {

            List<long> ignoredItems = new List<long>();
            if (userID != -1){
                ignoredItems = GetIgnoredStories(userID).ToList();
            if(ignoredItems.Count == 0)
                ignoredItems = new List<long>();
            
             }
    
            //index 0 => newest index n => oldest

            //Get the latest clusters in depending on the category
            var clusters = (from n in de.Clusters
                            where (n.CategoryID == categoryID && !ignoredItems.Contains(n.ClusterID) )
                            orderby n.LastUpdate descending
                            select n).Take(noOfItems);

            return clusters;
        }

        public IQueryable<Cluster> GetNewsStoriesByCategoryBeforeTime(long userID, int categoryID, int noOfItems, string lastupdate)
        {
            List<long> ignoredItems = new List<long>();
            if (userID != -1)
            {
                ignoredItems = GetIgnoredStories(userID).ToList();
                if (ignoredItems.Count == 0)
                    ignoredItems = new List<long>();

            }
            DateTime updatetime = DateTime.Parse(lastupdate);
            //index 0 => newest index n => oldest

            //Get the latest clusters in depending on the category
            var clusters = (from n in de.Clusters
                            where (n.LastUpdate < updatetime && n.CategoryID == categoryID && !ignoredItems.Contains(n.ClusterID))
                            orderby n.LastUpdate descending
                            select n).Take(noOfItems);

            return clusters;
        }

        public IQueryable<long> GetIgnoredStories(long userID)
        {
            IQueryable<long> ignoredItems = from n in de.IgnoredItems
                                            where (n.UserID == userID && n.IsCluster == true)
                                            select n.ItemID;

            return ignoredItems;
        }



    }
    
}