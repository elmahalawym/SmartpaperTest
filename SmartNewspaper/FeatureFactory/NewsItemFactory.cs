using SmartNewspaper;
//using LuceneService;
using System.Collections.Generic;
using System.Linq;


namespace SNWS
{

    public class NewsItemFactory
    {
        public static iEntities de;
        public long UserID { get; set; }
        public long ItemID { get; set; }

        public NewsItemFactory (){
        
            UserID = -1;
            de = new iEntities();
        }

        public IQueryable<Item> GetLatestNews(long userID, int noOfItems)
        {
            List<long> ignoredItems = new List<long>();
            if (userID != -1)
            {
                ignoredItems = GetIgnoredItems(userID).ToList();
                if (ignoredItems.Count == 0)
                    ignoredItems = new List<long>();

            }

            var latestitems = (from n in de.Items
                               where !ignoredItems.Contains(n.ItemID)
                               select n).OrderByDescending(x => x.ItemID).Take(noOfItems);

            return latestitems;
        
        }

        public IQueryable<Item> GetLatestNewsFromID(long userID,int startId)
        {
            List<long> ignoredItems = new List<long>();
            if (userID != -1)
            {
                ignoredItems = GetIgnoredItems(userID).ToList();
                if (ignoredItems.Count == 0)
                    ignoredItems = new List<long>();

            }

            //Note : Take would handle the bad parameter 30 without problems
            var latestitems = (from n in de.Items
                               where (n.ItemID > startId && !ignoredItems.Contains(n.ItemID))
                               select n 
                               ).OrderByDescending(x => x.ItemID).Take(30);

            return latestitems;

        }

        public IQueryable<Item> GetCustomNews(long userID, int categoryID, int sourceID, int noOfItems)
        {

            int ALLSources = -1;
            int AllCategories = -1;
            IQueryable<Item> items;

            List<long> ignoredItems = new List<long>();
            if (userID != -1)
            {
                ignoredItems = GetIgnoredItems(userID).ToList();
                if (ignoredItems.Count == 0)
                    ignoredItems = new List<long>();

            }


            if (sourceID == ALLSources)
            {
                items = (from n in de.Items
                         where (n.CategoryID == categoryID && !ignoredItems.Contains(n.ItemID))
                         select n).OrderByDescending(x => x.DateOfItem).Take(noOfItems);
            }

            else if (categoryID == AllCategories)
            {
                items = (from n in de.Items
                         where (n.IDNewsSources == sourceID && !ignoredItems.Contains(n.ItemID))
                         select n).OrderByDescending(x => x.DateOfItem).Take(noOfItems);
            }


            else if(categoryID == AllCategories && sourceID == ALLSources){

                items = (from n in de.Items
                         select n).OrderByDescending(x => x.DateOfItem).Take(noOfItems);
            }
            else
            {

                items = (from n in de.Items
                         where ((!ignoredItems.Contains(n.ItemID) && n.IDNewsSources == sourceID && n.CategoryID == categoryID))
                         select n).OrderByDescending(x => x.DateOfItem).Take(noOfItems);

            }
            return items;
        }

        internal IQueryable<Item> GetCustomNewsFromID(int userID, int categoryID, int sourceID, int noOfItems, int startId)
        {

            int ALLSources = -1;
            int AllCategories = -1;
            IQueryable<Item> items;

            List<long> ignoredItems = new List<long>();
            if (userID != -1)
            {
                ignoredItems = GetIgnoredItems(userID).ToList();
                if (ignoredItems.Count == 0)
                    ignoredItems = new List<long>();

            }

            if (sourceID == ALLSources)
            {
                items = (from n in de.Items
                         where (n.ItemID > startId && !ignoredItems.Contains(n.ItemID) && n.CategoryID == categoryID)
                         select n).OrderByDescending(x => x.DateOfItem).Take(noOfItems);
            }

            else if (categoryID == AllCategories)
            {
                items = (from n in de.Items
                         where (n.ItemID > startId && !ignoredItems.Contains(n.ItemID) && n.IDNewsSources == sourceID)
                         select n).OrderByDescending(x => x.DateOfItem).Take(noOfItems);
            }


            //Note : Take would handle the bad parameter 30 without problems
                items = (from n in de.Items
                         where (n.ItemID > startId && !ignoredItems.Contains(n.ItemID) && n.IDNewsSources == sourceID && n.CategoryID == categoryID)
                         select n).OrderByDescending(x => x.ItemID).Take(30);

                return items;
        }


        //public IQueryable<Item> GetNewsFromCustomFilter(long userID, string filter)
        //{
        //    List<long> ignoredItems = new List<long>();
        //    if (userID != -1)
        //    {
        //        ignoredItems = GetIgnoredItems(userID).ToList();
        //        if (ignoredItems.Count == 0)
        //            ignoredItems = new List<long>();

        //    }

        //    //Always return top 20 Item
        //    //Threshold rule should be implemented
        //    var resultsIDs = Searcher.FilterByQuery(filter, Indexer.IndexDirectory, 20);

        //    var results = from n in de.Items
        //                  where (!ignoredItems.Contains(n.ItemID) && resultsIDs.Contains(n.ItemID))
        //                  select n;

        //    return results;
        //}
        private IQueryable<long> GetIgnoredItems(long UserID)
        {
            IQueryable<long> ignoredItems = from n in de.IgnoredItems
                                            where (n.UserID == UserID && n.IsCluster == false)
                                            select n.ItemID;
            return ignoredItems;
        }

    }
}