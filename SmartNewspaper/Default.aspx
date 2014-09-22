<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SmartNewspaper.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Interestori</title>
    <link rel="icon" href="images/favicon.ico" />
    <link rel="stylesheet" href="css/master.css" type="text/css" />
    <link rel="stylesheet" href="css/jquery-ui-1.10.4.css" type="text/css" />
    <link rel="stylesheet" href="css/anythingslider.css" />
    <link rel="stylesheet" href="css/flexslider.css" />

    <script type="text/javascript">
        var userIsLoggedInJS = '<%= Session["UserIsLoggedIn"].ToString() %>';
    </script>

    <script runat="server">
        protected string getImagefromNewsSourceID(string id)
        {
            Dictionary<string, string> sources = new Dictionary<string, string>();
            sources.Add("1", "images/yoom7.png");
            sources.Add("2", "images/goal.png");
            sources.Add("3", "images/elmasry.png");
            sources.Add("4", "images/jazera.png");
            sources.Add("5", "images/moheet.png");
            sources.Add("6", "images/bbc.png");
            sources.Add("7", "images/onaeg.png");
            sources.Add("8", "images/eldostor.png");
            sources.Add("9", "images/ait.png");
            sources.Add("10", "images/donia-tech.png");
            sources.Add("11", "images/unlim-tech.png");
            sources.Add("12", "images/skynews.png");
            sources.Add("13", "images/sasapost.png");
            sources.Add("14", "images/sherouk.png");
            sources.Add("15", "images/echrouk.png");
            sources.Add("16", "images/elarabi-elgaded.png");
            sources.Add("17", "images/keef-tech.png");
            sources.Add("18", "images/ardroid.png");
            sources.Add("19", "images/cnn-arabic.png");
            sources.Add("20", "images/elmasryon.png");
            if (sources.ContainsKey(id))
            {
                return sources[id];
            }
            else
            {
                return "";
            }
        }
        protected string getSnippetfromContent(string content)
        {
            if (content.Length > 100)
            {
                string snippet = content.Substring(0, 100);
                int spaceOccuranceInSnippet = snippet.LastIndexOf(" ");
                return content.Substring(0, spaceOccuranceInSnippet) + "...";
            }
            else
            {
                return content;
            }
        }

        static string computeTimeFromDateTime(DateTime x)
        {
            TimeSpan diff = DateTime.UtcNow - x;
            if (diff < new TimeSpan(0))
            {
                return x.ToString();
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
                return x.ToString();
            }
        }

        
    </script>

</head>


<body runat="server" id="body">
    <form runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <div id="loginOverlay">
            <span class="xbutton">
                <img src="images/x.png" />
            </span>
            <div id="loginBox">
                <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanel_Login" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="loginOverlay_loading" class="modal" style="display: block; z-index: 1005;">
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanel_Login" runat="server" UpdateMode="Conditional">

                    <ContentTemplate>
                        <asp:TextBox ID="TextBox_Username" CssClass="textbox_login_register" runat="server" placeholder="Username"></asp:TextBox>
                        <asp:TextBox ID="TextBox_Password" CssClass="textbox_login_register" ClientIDMode="Static" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="loginControl" ID="RequiredFieldValidator_Username" runat="server" ErrorMessage="Username" ControlToValidate="TextBox_Username" Text="" Display="None"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ValidationGroup="loginControl" ID="RequiredFieldValidator_Password" runat="server" ErrorMessage="Password" ControlToValidate="TextBox_Password" Text="" Display="None"></asp:RequiredFieldValidator>

                        <asp:ValidationSummary ID="ValidationSummary_Login" ValidationGroup="loginControl" runat="server" CssClass="validationSpace" HeaderText="Required fields: " EnableClientScript="true" DisplayMode="BulletList" />

                        <asp:Panel ID="Panel_LoginFailed" ClientIDMode="Static" runat="server" CssClass="login_failed" Visible="false">Login failed. Please try again.</asp:Panel>

                        <div style="display: block">
                            <asp:Button ID="Button_Login" CssClass="button_login_register search" ClientIDMode="Static" runat="server" Text="Login" OnClick="Button_Login_Click" />
                            <asp:Button ID="Button_Register" CssClass="button_login_register" ClientIDMode="Static" runat="server" Text="Register" />
                        </div>
                        <asp:Button ID="Button_SignInWithFacebook" CssClass="button_facebook" Text="Sign in with Facebook" ClientIDMode="Static" runat="server" OnClick="Button_SignInWithFacebook_Click" ValidationGroup="myValidation" />
                        <%--<asp:Button ID="Button_SignInWithTwitter" CssClass="button_twitter" Text="Sign in with Twitter" ClientIDMode="Static" runat="server" OnClick="Button_SignInWithTwitter_Click" />--%>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Button_Login" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>



        <div id="registrationOverlay">
            <span class="xbutton">
                <img src="images/x.png" />
            </span>
            <div id="registrationBox">
                <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanel_Registartion">
                    <ProgressTemplate>
                        <div id="registerationOverlay_loading" class="modal" style="display: block; z-index: 1005;">
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanel_Registartion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <table style="margin: auto;">
                            <tr>
                                <td>
                                    <asp:TextBox ID="TextBox_Username_Register" ClientIDMode="Static" CssClass="textbox_login_register" runat="server" placeholder="Username"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ValidationGroup="registerationGroup" ErrorMessage="*" ControlToValidate="TextBox_Username_Register" runat="server" CssClass="validationStar" Display="Static" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="TextBox_Password_Register" ClientIDMode="Static" CssClass="textbox_login_register" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ValidationGroup="registerationGroup" ErrorMessage="*" ControlToValidate="TextBox_Password_Register" runat="server" CssClass="validationStar" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="TextBox_Email_Register" ClientIDMode="Static" CssClass="textbox_login_register" runat="server" placeholder="Email"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ValidationGroup="registerationGroup" ErrorMessage="*" ControlToValidate="TextBox_Email_Register" runat="server" CssClass="validationStar" />
                                    <%--<asp:RegularExpressionValidator runat="server" ErrorMessage="*" ControlToValidate="TextBox_Email_Register" CssClass="validationStar"/>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="TextBox_FirstName_Register" ClientIDMode="Static" CssClass="textbox_login_register" runat="server" placeholder="First Name"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ValidationGroup="registerationGroup" ErrorMessage="*" ControlToValidate="TextBox_FirstName_Register" runat="server" CssClass="validationStar" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="TextBox_LastName_Register" ClientIDMode="Static" CssClass="textbox_login_register" runat="server" placeholder="Last Name"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ValidationGroup="registerationGroup" ErrorMessage="*" ControlToValidate="TextBox_LastName_Register" runat="server" CssClass="validationStar" />
                                </td>
                            </tr>
                        </table>

                        <asp:Panel ID="registeration_failed" ClientIDMode="Static" runat="server" CssClass="login_failed" Visible="false">Registration failed. Please try again.</asp:Panel>

                        <asp:Button runat="server" ID="registeration_Submit" Text="Submit" ClientIDMode="Static" OnClick="registeration_Submit_Click" CssClass="button_login_register" /></td>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="registeration_Submit" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <div id="helpOverlay">
            <span id="xbuttonSlideShow">
                <img src="images/x.png" />
            </span>
            <div id="helpSlideshowOuter">
                <div id="nav_next"></div>
                <div id="nav_prev"></div>
                <div id="helpSlideshow">
                    <ul class="slides">
                        <li>
                            <img src="images/help/help1.png" /></li>
                        <li>
                            <img src="images/help/help2.png" /></li>
                        <li>
                            <img src="images/help/help3.png" /></li>
                    </ul>
                </div>

            </div>
        </div>

        <div id="errorPopup">An error occurred.</div>


        <div id="header" class="header row">
            <asp:Panel ID="Panel_UserLoggedIn" ClientIDMode="Static" runat="server" Visible="false">
                <div id="loggedIn">
                    <asp:Panel ID="userPicture" ClientIDMode="Static" runat="server"></asp:Panel>
                    <div id="userControls">
                        <table style="width: 100%" border="0">
                            <tbody>
                                <tr>
                                    <td style="font-weight: bold">
                                        <asp:HyperLink ID="HyperLink_Username" CssClass="usernameText" Text="Username" Target="_blank" runat="server" />
                                        <asp:Label ID="Label_Welcome" Text=" مرحباً " runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td><a>Settings</a></td>--%>
                                    <td><a href="Users/Logout.aspx" class="userControlsLabels">خروج</a>
                                        <span class="help userControlsLabels">مساعدة ؟</span>
                                        <asp:Label runat="server" ID="imagesToggle" CssClass="userControlsLabels" ClientIDMode="Static" Text="" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                </div>
            </asp:Panel>

            <asp:Panel ID="Panel_UserNotLoggedIn" ClientIDMode="Static" runat="server" Visible="true">
                <div id="login">
                    <a class="link_login_register">التسجيل - الدخول</a>
                    <span class="help" style="float: left;">مساعدة ؟</span>
                </div>

            </asp:Panel>

            <div id="logo">
                <a href="Default.aspx">
                    <img src="images/logo.png" alt="S-paper" />
                </a>
            </div>

            <div id="search" class="search">
                <asp:TextBox ID="searchBox" ClientIDMode="Static" CssClass="search" runat="server" autocomplete="off" />
                <div id="resultframe" class="search">
                </div>
            </div>

        </div>

        <div id="wrapper" class="body row scroll-x scroll-y">
            <div id="leftStream">

                <div id="articlePreview">

                    <div id="articleButton">
                        <img src="images/back-button2.png" />
                    </div>

                    <asp:Image runat="server" ID="articlePreviewImage" ClientIDMode="Static" ImageUrl="" />

                    <a id="articlePreviewLink" href="#" target="_blank">
                        <div id="articlePreviewTitle">
                            <asp:Label runat="server" ID="asp_selected_article_Title" Text=""></asp:Label>
                        </div>
                    </a>

                    <div id="articlePreviewContent">
                        <asp:Label runat="server" ID="asp_selected_article_content" Text="">
                        </asp:Label>
                    </div>
                </div>

                <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanel_leftStream" DisplayAfter="0" ID="UpdatePanel_leftStream_updateprogress" ClientIDMode="Static">
                    <ProgressTemplate>
                        <div class="modal" style="display: block; z-index: 1008;">
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div id="leftStreamContainer">

                    <asp:Button ID="button_loadMoreNews" ClientIDMode="Static" OnClick="button_loadMoreNews_Click" runat="server" type="button" />
                    <div id="button_settings">
                        <asp:Button ID="button_customNewsApply" ClientIDMode="Static" Text="تطبيق" OnClick="button_customNewsApply_Click" runat="server" type="button" />
                        <div class="styleSelect">
                            <asp:DropDownList runat="server" ID="dropDownList_Source" ClientIDMode="Static">
                                <asp:ListItem Text="كل المصادر" Value="-1" />
                                <asp:ListItem Text="اليوم السابع" Value="1" />
                                <asp:ListItem Text="جول" Value="2" />
                                <asp:ListItem Text="المصري اليوم" Value="3" />
                                <asp:ListItem Text="الجزيرة" Value="4" />
                                <asp:ListItem Text="شبكة محيط" Value="5" />
                                <asp:ListItem Text="بي بي سي" Value="6" />
                                <asp:ListItem Text="أونا" Value="7" />
                                <asp:ListItem Text="جريدة الدستور" Value="8" />
                                <asp:ListItem Text="البوابة العربية للأخبار التقنية" Value="9" />
                                <asp:ListItem Text="دنيا التكنولوجيا" Value="10" />
                                <asp:ListItem Text="التقنية بلا حدود" Value="11" />
                                <asp:ListItem Text="سكاي نيوز" Value="12" />
                                <asp:ListItem Text="ساسة بوست" Value="13" />
                                <asp:ListItem Text="الشروق" Value="14" />
                                <asp:ListItem Text="الشروق أون لاين" Value="15" />
                                <asp:ListItem Text="العربي" Value="16" />
                                <asp:ListItem Text="كيف تك" Value="17" />
                                <asp:ListItem Text="أردرويد" Value="18" />
                                <asp:ListItem Text="سي ان ان" Value="19" />
                                <asp:ListItem Text="المصريون" Value="20" />
                            </asp:DropDownList>
                        </div>
                        <div class="styleSelect">
                            <asp:DropDownList runat="server" ID="dropDownList_Category" ClientIDMode="Static">
                                <asp:ListItem Text="كل التصنيفات" Value="-1" />
                                <asp:ListItem Text="السياسة" Value="4" />
                                <asp:ListItem Text="الرياضة" Value="2" />
                                <asp:ListItem Text="التكنولوجيا" Value="3" />
                                <asp:ListItem Text="عام" Value="1" />
                            </asp:DropDownList>
                        </div>
                        <img src="images/settings.png" />
                    </div>

                    <asp:UpdatePanel runat="server" ID="UpdatePanel_leftStream" ClientIDMode="Static" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ListView ID="ListView_leftStream" ClientIDMode="Static" runat="server">

                                <LayoutTemplate>
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                </LayoutTemplate>

                                <ItemTemplate>

                                    <asp:Panel runat="server" ID="asp_article_wrapper" class="articleWrapperMini" data-Title='<%# Eval("Title") %>' data-Content="" data-Image='<%# Eval("ImageURL") %>' data-ID='<%# Eval("ItemID") %>' data-URL='<%# Eval("URL") %>'>
                                        <div class="overlay overlayLeftStream">
                                        </div>
                                        <div class="articleFrame">
                                            <%--next line could have this  style="background-image: url('<%# Eval("ImageURL") %>')"--%>
                                            <div class="article lazy" style="background-image: url('<%# hfEnableImages.Value=="true" ? Eval("ImageURL") : "" %>')">
                                                <div class="articleOptions" style="display: none;">
                                                    <a class="optionShare" href='<%# "https://www.facebook.com/sharer/sharer.php?u=" + Eval("URL") %>' onclick="MyPopUpWin(this.href,600,500); return false;">
                                                        <img src="images/share.png" />
                                                    </a>
                                                    <img src="images/later.png" class="optionLater" runat="server" />
                                                    <img src="images/close.png" class="optionClose" runat="server" />
                                                </div>
                                                <div id="asp_articleTitle" class="articleTitle articleTitleMini" <%--snippet='<%# getSnippetfromContent(Eval("Content").ToString()) %>'--%>>

                                                    <asp:Label ID="asp_article_title" runat="server" Text='<%# Eval("Title") %>' /><br>
                                                    <asp:Label ID="asp_article_time" runat="server" CssClass="articleDateMini" Text='<%# computeTimeFromDateTime((DateTime)Eval("DateOfItem")) %>' title='<%# Eval("DateOfItem") %>'></asp:Label>
                                                    <img class="lazy" src="<%# getImagefromNewsSourceID(Eval("IDNewsSources").ToString()) %>" />
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                </ItemTemplate>

                            </asp:ListView>

                            <asp:HiddenField runat="server" ID="hfLeftStreamMode" ClientIDMode="Static" Value="LatestNews" />
                            <asp:HiddenField runat="server" ID="hfFirstIDLatestNews" ClientIDMode="Static" Value="" />
                            <asp:HiddenField runat="server" ID="hfLastIDLatestNews" ClientIDMode="Static" Value="" />

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="button_loadMoreNews" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="button_seeMore" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="button_customFilter" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="button_read_later" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="button_customNewsApply" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>

            </div>

            <div id="rightStream">
                <div id="rightStreamContainer">
                    <input type="button" id="button_reloadRightStream" data-onclick="button_reloadRightStream_Click" />
                    <div id="filters">
                        <div id="plus">
                            <div id="addFilterOverlayBG">
                                <span id="xbutton_addFilter">
                                    <img src="images/x.png" />
                                </span>
                                <asp:Panel runat="server" ID="addFilterOverlay" ClientIDMode="Static" DefaultButton="addFilterButton">
                                    <span>كلمة البحث:</span>
                                    <asp:TextBox runat="server" ID="addFilterName" ClientIDMode="Static" />
                                    <asp:Button Text="إضافة" ID="addFilterButton" ClientIDMode="Static" runat="server" OnClick="addFilterButton_Click" />
                                </asp:Panel>
                            </div>

                        </div>

                        <div id="filterElmntWrapper">
                            <input type="button" value="آخر الأحداث" id="button_filter_All" data-onclick="button_filter_All_Click" class="filterElmnt selected" />
                            <input type="button" value="سياسة" id="button_filter_Politics" data-onclick="button_filter_Politics_Click" class="filterElmnt" />
                            <input type="button" value="رياضة" id="button_filter_Sports" data-onclick="button_filter_Sports_Click" class="filterElmnt" />
                            <input type="button" value="علوم و تكنولوجيا" id="button_filter_Technology" data-onclick="button_filter_Technology_Click" class="filterElmnt" />
                            <input type="button" value="عام" id="button_filter_General" data-onclick="button_filter_General_Click" class="filterElmnt" />
                            <%--<asp:Button Text="آخر الأحداث" runat="server" ID="button1" OnClick="button_filter_All_Click" CssClass="filterElmnt selected" />--%>
                            <%--                            <asp:Button Text="سياسة" runat="server" ID="button_filter_Politics" OnClick="button_filter_Politics_Click" CssClass="filterElmnt" />
                            <asp:Button Text="رياضة" runat="server" ID="button_filter_Sports" OnClick="button_filter_Sports_Click" CssClass="filterElmnt" />
                            <asp:Button Text="علوم و تكنولوجيا" runat="server" ID="button_filter_Technology" OnClick="button_filter_Technology_Click" CssClass="filterElmnt" />
                            <asp:Button Text="عام" runat="server" ID="button_filter_General" OnClick="button_filter_General_Click" CssClass="filterElmnt" />--%>
                        </div>
                    </div>

                    <div id="UpdatePanel_rightStream_updateprogress">
                        <div class="modal" style="display: block; z-index: 1008;">
                        </div>
                    </div>

                    <div id="UpdatePanel_rightStream">

                        <div id="ListView_rightStream">
                            <ul id="storiesStream">
                                <%--                                    <li class="newsStory">
                                        <div id="ListView_rightStream_Inner">
                                                    <div id="asp_article_wrapper" class="articleWrapper" data-Title='' data-Content="" data-Image='' data-ID='' data-URL=''>
                                                        <div class="overlay overlayRightStream">
                                                        </div>
                                                        <table border="0">
                                                            <tbody>
                                                                <tr class="articleHeader">
                                                                    <td>
                                                                        <span id="asp_article_time" class="articleDate" title=''>منذ أكثر من شهر و كده</span>
                                                                    </td>
                                                                    <td>
                                                                        <ul class="articleSources">
                                                                            <li class="articleSrc">
                                                                                <div class="triLeft"></div>
                                                                                <div class="triRight"></div>
                                                                                <img class="lazy" src="" /></li>
                                                                        </ul>
                                                                        <span class="newsIndexInStory" data-fixed="false">خبر كده من كده</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 111px" colspan="2" runat="server">
                                                                        <div class="articleFrame">
                                                                            <div class="article lazy" >
                                                                                <div class="articleOptions">
                                                                                    <a class="optionShare" href="https://www.facebook.com/sharer/sharer.php?u=" onclick="MyPopUpWin(this.href,600,500); return false;">
                                                                                        <img src="images/share.png" />
                                                                                    </a>
                                                                                    <img src="images/later.png" class="optionLater" runat="server" />
                                                                                    <img src="images/close.png" class="optionClose" runat="server" />
                                                                                </div>
                                                                                <div id ="asp_articleTitle" class="articleTitle">
                                                                                    <span id="asp_article_title">العنوان و كده</span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                        </div>
                                    </li>--%>
                            </ul>

                        </div>

                        <input type="button" id="button_loadMoreNews_Recommended" data-onclick="button_loadMoreNews_Recommended_Click" />

                        <%--<asp:HiddenField runat="server" ID="hfRightStreamLastUpdate" ClientIDMode="Static" Value="" />--%>
                        <asp:HiddenField runat="server" ID="hfRightStreamMode" ClientIDMode="Static" Value="All" />

                    </div>
                </div>
            </div>

            <div id="sidebar">
                <ul>
                    <li class="nav" id="navHome">
                        <a href="Default.aspx">الرئيسية</a>
                        <img id="iconHome" src="images/home.png" />
                    </li>
                    <li class="nav" id="navReadLater">
                        <asp:Label Text="" ID="counter" ClientIDMode="Static" runat="server" />
                        <asp:Button Text="للقراءة لاحقاً" ID="button_read_later" ClientIDMode="Static" OnClick="button_read_later_Click" runat="server" />
                        <img id="iconLater" src="images/later_small.png" />
                    </li>
                    <asp:UpdateProgress ID="UpdateProgress_userFilters" runat="server" AssociatedUpdatePanelID="UpdatePanel_userFilters" ClientIDMode="Static">
                        <ProgressTemplate>
                            <div class="modal" style="display: block; z-index: 1008;">
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel_userFilters" ClientIDMode="Static" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ListView runat="server" ID="ListView_userFilters">
                                <LayoutTemplate>
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <asp:Panel runat="server" Style="display: block;" CssClass="customFilterElmntContainer">
                                        <asp:Label runat="server" Text='<%# Eval("Content") %>' CssClass="filterElmnt customFilterElmnt" data-filterID='<%# Eval("FilterID") %>' data-filterContent='<%# Eval("Content") %>' />
                                        <asp:Label Text="x" CssClass="customFilterElmntDelete" data-filterID='<%# Eval("FilterID") %>' runat="server" />
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:ListView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="addFilterButton" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="button_customFilterDelete" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ul>
                <span id="button_addFilter" class="plus_addFilter">مرشح جديد</span>
            </div>

        </div>

        <asp:Button Text="SEEMORE" runat="server" ID="button_seeMore" OnClick="button_seeMore_Click" ClientIDMode="Static" Style="display: none;" />
        <asp:Button Text="CUSTOMFILTER" runat="server" ID="button_customFilter" OnClick="button_customFilter_Click" ClientIDMode="Static" Style="display: none;" data-filter="" />
        <asp:Button Text="CUSTOMFILTERDELETE" runat="server" ID="button_customFilterDelete" OnClick="button_customFilterDelete_Click" ClientIDMode="Static" Style="display: none;" data-filter="" />
        <asp:HiddenField runat="server" ID="hfCustomFilter" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hfDateTimeNow" ClientIDMode="Static" Value="" />
        <asp:HiddenField runat="server" ID="hfEnableImages" ClientIDMode="Static" Value="true" />

        <div id="footer" style="display: none;">
            <asp:Label runat="server" ID="label_loggedinindicator"></asp:Label>
        </div>


    </form>

    <script type="text/javascript" src="js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.js"></script>
    <script type="text/javascript" src="js/enscroll-0.6.0.min.js"></script>
    <script type="text/javascript" src="js/jquery.anythingslider.js"></script>
    <script type="text/javascript" src="js/jquery.waitforimages.min.js"></script>
    <script type="text/javascript" src="js/jquery.flexslider-min.js"></script>
    <script type="text/javascript" src="js/default.js"></script>
</body>
</html>
