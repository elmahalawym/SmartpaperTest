using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartNewspaper
{
    public partial class NewsItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string q = Request.Params[0];
        }
    }
}