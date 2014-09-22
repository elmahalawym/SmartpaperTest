$body = $("body");
$selectedNewsItemID = "";
$mytestObject = $("body");

function MyPopUpWin(url, width, height) {
    var leftPosition, topPosition;
    //Allow for borders.
    leftPosition = (window.screen.width / 2) - ((width / 2) + 10);
    //Allow for title and status bars.
    topPosition = (window.screen.height / 2) - ((height / 2) + 50);
    //Open the window.
    window.open(url, "Window2",
    "status=no,height=" + height + ",width=" + width + ",resizable=yes,left="
    + leftPosition + ",top=" + topPosition + ",screenX=" + leftPosition + ",screenY="
    + topPosition + ",toolbar=no,menubar=no,scrollbars=no,location=no,directories=no");
}

var showLoginRegisterBlock = function () {

    $("#loginOverlay").fadeIn(
        250,
        "swing");


    $("#Button_Register").click(function (event) {
        $("#loginOverlay").fadeOut(100, "swing");
        $("#registrationOverlay").fadeIn(350, "swing");
        // disable scrolling
        //$('body').addClass('stop-scrolling')
        //$('body').css("hieght", "100%");
        //$('body').css("overflow", "hidden");
    });

    // disable scrolling
    $('body').addClass('stop-scrolling')
    $('body').css("hieght", "100%");
    $('body').css("overflow", "hidden");

};

var articlePreview = $('#articlePreview');

var articlePreviewEnscrolled = false;

function preload(arrayOfImages) {
    $(arrayOfImages).each(function () {
        $('<img/>')[0].src = this;
        // Alternatively you could use:
        // (new Image()).src = this;
    });
}


// setting up the scrolling handlers for loading more news
var enableScrollingDetectionRight = true;
var enableScrollingDetectionLeft = true;
scrollOKRight = true; //setup flag to check if it's OK to run the event handler
scrollOKLeft = true; //setup flag to check if it's OK to run the event handler

var timer = setInterval(function () {
    if (enableScrollingDetectionRight == true) {
        scrollOKRight = true;
    }
    else {
        scrollOKRight = false;
    }
    if (enableScrollingDetectionLeft == true) {
        scrollOKLeft = true;
    }
    else {
        scrollOKLeft = false;
    }
}, 100); //run this every tenth of a second



var showUpdatePanel_leftStream_updateprogress = function () {
    $("#UpdatePanel_leftStream_updateprogress").fadeIn(100);
};

var hideUpdatePanel_leftStream_updateprogress = function () {
    $("#UpdatePanel_leftStream_updateprogress").fadeOut(100);
};

var showUpdatePanel_rightStream_updateprogress = function () {
    $("#UpdatePanel_rightStream_updateprogress").fadeIn(100);
};

var hideUpdatePanel_rightStream_updateprogress = function () {
    $("#UpdatePanel_rightStream_updateprogress").fadeOut(100);
};

var currentRequest = null;

var canSendAjaxSearch = true;

var enableImages = false;

//var resultFrameShouldSlideUp = true;

function htmlEncode(value) {
    //create a in-memory div, set it's inner text(which jQuery automatically encodes)
    //then grab the encoded contents back out.  The div never exists on the page.
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}

function getNewsSourceImagefromNewsSourceID(IDNewsSources) {
    var SourceImage = "";
    if (IDNewsSources == 1) { SourceImage = "images/yoom7.png"; }
    else if (IDNewsSources == 2) { SourceImage = "images/goal.png"; }
    else if (IDNewsSources == 3) { SourceImage = "images/elmasry.png"; }
    else if (IDNewsSources == 4) { SourceImage = "images/jazera.png"; }
    else if (IDNewsSources == 5) { SourceImage = "images/moheet.png"; }
    else if (IDNewsSources == 6) { SourceImage = "images/bbc.png"; }
    else if (IDNewsSources == 7) { SourceImage = "images/onaeg.png"; }
    else if (IDNewsSources == 8) { SourceImage = "images/eldostor.png"; }
    else if (IDNewsSources == 9) { SourceImage = "images/ait.png"; }
    else if (IDNewsSources == 10) { SourceImage = "images/donia-tech.png"; }
    else if (IDNewsSources == 11) { SourceImage = "images/unlim-tech.png"; }
    else if (IDNewsSources == 12) { SourceImage = "images/skynews.png"; }
    else if (IDNewsSources == 13) { SourceImage = "images/sasapost.png"; }
    else if (IDNewsSources == 14) { SourceImage = "images/sherouk.png"; }
    else if (IDNewsSources == 15) { SourceImage = "images/echrouk.png"; }
    else if (IDNewsSources == 16) { SourceImage = "images/elarabi-elgaded.png"; }
    else if (IDNewsSources == 17) { SourceImage = "images/keef-tech.png"; }
    else if (IDNewsSources == 18) { SourceImage = "images/ardroid.png"; }
    else if (IDNewsSources == 19) { SourceImage = "images/cnn-arabic.png"; }
    else if (IDNewsSources == 20) { SourceImage = "images/elmasryon.png"; }
    return SourceImage;
}

function makeStory(jsonStory) {
    //var story = JSON.parse(jsonStory);
    //alert(story.)
    var Title = jsonStory.firstItem.Title;
    var ImageURL = jsonStory.firstItem.Image;
    var ID = jsonStory.firstItem.ID;
    var IDNewsSources = jsonStory.firstItem.IDNewsSources;
    var URL = jsonStory.firstItem.URL;
    var CalculatedTime = jsonStory.firstItem.CalculatedTime;
    var itemIndex = 1;
    var ClusterItemCount = jsonStory.nItems;
    var SourceImage = "";
    var listOfIDs = jsonStory.listOfIDs
    var ClusterID = jsonStory.clusterId;

    SourceImage = getNewsSourceImagefromNewsSourceID(IDNewsSources)


    var template = "<li class=\"newsStory\" data-clusterid=\"{{ClusterID}}\" data-ClusterItemCount=\"{{ClusterItemCount}}\" data-listofids=\"{{ListOfIDs}}\"><div id=\"ListView_rightStream_Inner\"><div id=\"asp_article_wrapper\" class=\"articleWrapper\" data-Title=\'{{Title}}\' data-Content=\"{{Content}}\" data-Image=\'{{ImageURL}}\' data-itemIndex=\"{{itemIndex}}\" data-ID=\'{{ID}}\' data-URL=\'{{URL}}\'><div class=\"overlay overlayRightStream\"><\/div><table border=\"0\"><tbody><tr class=\"articleHeader\"><td><span id=\"asp_article_time\" class=\"articleDate\" title=\'\'>{{CalculatedTime}}<\/span><\/td><td><ul class=\"articleSources\"><li class=\"articleSrc\"><div class=\"triLeft\"><\/div><div class=\"triRight\"><\/div><img class=\"lazy\" src=\"{{SourceImage}}\" \/><\/li><\/ul><span class=\"newsIndexInStory\" data-fixed=\"false\">\u062E\u0628\u0631 <span class=\"itemIndex\">{{itemIndex}}</span><span> \u0645\u0646 </span><span class=\"ClusterItemCount\">{{ClusterItemCount}}</span><\/span><\/td><\/tr><tr><td style=\"height: 111px\" colspan=\"2\"><div class=\"articleFrame\"><div class=\"article lazy\"  style=\"background-image: url('{{BackgroundImageURL}}')\"><div class=\"articleOptions\"><a class=\"optionShare\" href=\"https:\/\/www.facebook.com\/sharer\/sharer.php?u={{URL}}\" onclick=\"MyPopUpWin(this.href,600,500); return false;\"><img src=\"images\/share.png\" \/><\/a><img src=\"images\/later.png\" class=\"optionLater\" \/><img src=\"images\/close.png\" class=\"optionClose\" \/><\/div><div id =\"asp_articleTitle\" class=\"articleTitle\"><span id=\"asp_article_title\">{{Title}}<\/span><\/div><\/div><\/div><\/td><\/tr><\/tbody><\/table><\/div><\/div><\/li>";

    template = template.replace(/\{\{Title\}\}/g, Title);
    template = template.replace(/\{\{Content\}\}/g, "");
    template = template.replace(/\{\{ImageURL\}\}/g, ImageURL);
    if ($('#hfEnableImages').attr('Value') == 'true') {
        template = template.replace(/\{\{BackgroundImageURL\}\}/g, ImageURL);
    }
    else {
        template = template.replace(/\{\{BackgroundImageURL\}\}/g, "");
    }
    template = template.replace(/\{\{SourceImage\}\}/g, SourceImage);
    template = template.replace(/\{\{ID\}\}/g, ID);
    template = template.replace(/\{\{URL\}\}/g, URL);
    template = template.replace(/\{\{CalculatedTime\}\}/g, CalculatedTime);
    template = template.replace(/\{\{itemIndex\}\}/g, itemIndex);
    template = template.replace(/\{\{ClusterItemCount\}\}/g, ClusterItemCount);
    template = template.replace(/\{\{ListOfIDs\}\}/g, listOfIDs);
    template = template.replace(/\{\{ClusterID\}\}/g, ClusterID);
    
    return template;
}

var loadHandlers = function () {

    $('#button_filter_All').off('click').on('click', function () {
        var resultLatestStories;
        jQuery.ajax({
            url: 'Default.aspx/LatestStories',
            type: "POST",
            data: "{ }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                resultLatestStories = result.d;
                var story = JSON.parse(result.d);
                var destination = $('#storiesStream');
                destination.html('');
                for (var i = 0; i < story.length; i++) {
                    destination.append(makeStory(story[i]));
                }
                hideUpdatePanel_rightStream_updateprogress();
                //alert(result.d);
                loadHandlers();
            },
            error: function () {
                alert(arguments[2]);
            }
        });
    });
    $('#button_filter_Politics').off('click').on('click', function () {
        var resultLatestStories;
        jQuery.ajax({
            url: 'Default.aspx/PoliticsStories',
            type: "POST",
            data: "{ }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                resultLatestStories = result.d;
                var story = JSON.parse(result.d);
                var destination = $('#storiesStream');
                destination.html('');
                for (var i = 0; i < story.length; i++) {
                    destination.append(makeStory(story[i]));
                }
                hideUpdatePanel_rightStream_updateprogress();
                //alert(result.d);
                loadHandlers();
            },
            error: function () {
                alert(arguments[2]);
            }
        });
    });
    $('#button_filter_Sports').off('click').on('click', function () {
        var resultLatestStories;
        jQuery.ajax({
            url: 'Default.aspx/SportsStories',
            type: "POST",
            data: "{ }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                resultLatestStories = result.d;
                var story = JSON.parse(result.d);
                var destination = $('#storiesStream');
                destination.html('');
                for (var i = 0; i < story.length; i++) {
                    destination.append(makeStory(story[i]));
                }
                hideUpdatePanel_rightStream_updateprogress();
                //alert(result.d);
                loadHandlers();
            },
            error: function () {
                alert(arguments[2]);
            }
        });
    });
    $('#button_filter_Technology').off('click').on('click', function () {
        var resultLatestStories;
        jQuery.ajax({
            url: 'Default.aspx/TechStories',
            type: "POST",
            data: "{ }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                resultLatestStories = result.d;
                var story = JSON.parse(result.d);
                var destination = $('#storiesStream');
                destination.html('');
                for (var i = 0; i < story.length; i++) {
                    destination.append(makeStory(story[i]));
                }
                hideUpdatePanel_rightStream_updateprogress();
                //alert(result.d);
                loadHandlers();
            },
            error: function () {
                alert(arguments[2]);
            }
        });
    });
    $('#button_filter_General').off('click').on('click', function () {
        var resultLatestStories;
        jQuery.ajax({
            url: 'Default.aspx/GeneralStories',
            type: "POST",
            data: "{ }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                resultLatestStories = result.d;
                var story = JSON.parse(result.d);
                var destination = $('#storiesStream');
                destination.html('');
                for (var i = 0; i < story.length; i++) {
                    destination.append(makeStory(story[i]));
                }
                hideUpdatePanel_rightStream_updateprogress();
                //alert(result.d);
                loadHandlers();
            },
            error: function () {
                alert(arguments[2]);
            }
        });
    });

    $('.triLeft').off('click').on('click', function () {
        var itemIndex = parseInt(parseInt($(this).parents('#asp_article_wrapper').attr('data-itemindex')));
        var clusterItemsCount = parseInt($(this).parents('.newsStory').attr('data-clusteritemcount'));

        var newsStoryParent = $(this).parents('.newsStory');

        var IDs = newsStoryParent.attr('data-listofids').split(",");

        if (itemIndex < clusterItemsCount) {
            newsStoryParent.find('.overlayRightStream').fadeIn(100);
            //alert('hit');
            var nextID = IDs[parseInt(itemIndex)];
            jQuery.ajax({
                url: 'Default.aspx/fetchNewsItem',
                type: "POST",
                data: "{ itemID: '" + nextID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    fetchedNewsItem = result.d;
                    var newsItem = JSON.parse(result.d);
                    $(newsStoryParent).find('.optionShare').attr('href', "https://www.facebook.com/sharer/sharer.php?u=" + newsItem.URL);
                    $(newsStoryParent).find('#asp_article_wrapper').attr('data-id', newsItem.ID);
                    $(newsStoryParent).find('#asp_article_wrapper').attr('data-title', newsItem.Title);
                    $(newsStoryParent).find('#asp_article_wrapper').attr('data-image', newsItem.Image);
                    if ($('#hfEnableImages').attr('Value') == 'true') {
                        $(newsStoryParent).find('.articleFrame .article').attr('style', "background-image: url('" + newsItem.Image + "')");
                    }
                    $(newsStoryParent).find('#asp_article_wrapper').attr('data-url', newsItem.URL);
                    $(newsStoryParent).find('.articleSrc img').attr('src', getNewsSourceImagefromNewsSourceID(newsItem.SourceID));
                    $(newsStoryParent).find('#asp_article_wrapper').attr('data-itemindex', itemIndex + 1);
                    $(newsStoryParent).find('.itemIndex').html(itemIndex + 1);
                    $(newsStoryParent).find('#asp_article_title').html(newsItem.Title);
                    newsStoryParent.find('.overlayRightStream').fadeOut(150);
                    loadHandlers();
                },
                error: function () {
                    alert(arguments[2]);
                }
            });
        }
    });

    $('.triRight').off('click').on('click', function () {
        var itemIndex = parseInt(parseInt($(this).parents('#asp_article_wrapper').attr('data-itemindex')));
        var clusterItemsCount = parseInt($(this).parents('.newsStory').attr('data-clusteritemcount'));

        var newsStoryParent = $(this).parents('.newsStory');

        var IDs = newsStoryParent.attr('data-listofids').split(",");

        if (itemIndex > 1) {
            newsStoryParent.find('.overlayRightStream').fadeIn(100);
            //alert('hit');
            var previousID = IDs[itemIndex - 2];
            jQuery.ajax({
                url: 'Default.aspx/fetchNewsItem',
                type: "POST",
                data: "{ itemID: '" + previousID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    fetchedNewsItem = result.d;
                    var newsItem = JSON.parse(result.d);
                    $(newsStoryParent).find('.optionShare').attr('href', "https://www.facebook.com/sharer/sharer.php?u=" + newsItem.URL);
                    $(newsStoryParent).find('#asp_article_wrapper').attr('data-id', newsItem.ID);
                    $(newsStoryParent).find('#asp_article_wrapper').attr('data-title', newsItem.Title);
                    $(newsStoryParent).find('#asp_article_wrapper').attr('data-image', newsItem.Image);
                    if ($('#hfEnableImages').attr('Value') == 'true') {
                        $(newsStoryParent).find('.articleFrame .article').attr('style', "background-image: url('" + newsItem.Image + "')");
                    }
                    $(newsStoryParent).find('#asp_article_wrapper').attr('data-url', newsItem.URL);
                    $(newsStoryParent).find('.articleSrc img').attr('src', getNewsSourceImagefromNewsSourceID(newsItem.SourceID));
                    $(newsStoryParent).find('#asp_article_wrapper').attr('data-itemindex', itemIndex - 1);
                    $(newsStoryParent).find('.itemIndex').html(itemIndex - 1);
                    $(newsStoryParent).find('#asp_article_title').html(newsItem.Title);
                    newsStoryParent.find('.overlayRightStream').fadeOut(150);
                    loadHandlers();
                },
                error: function () {
                    alert(arguments[2]);
                }
            });
        }
    });

    hideUpdatePanel_rightStream_updateprogress();
    hideUpdatePanel_leftStream_updateprogress();

    function EndRequest(sender, args) {
        if (postBackElement.hasClas('filterElmnt'))
            $get('UpdatePanel_rightStream_updateprogress').style.display = 'none';
    }

    // clicking on an article
    $(".articleFrame").off('click').on('click',
        function (event) {
            var parentOfThis = $(this).parents('.articleWrapper, .articleWrapperMini');

            // if the user is clicking the already clicked newsItem, do nothing
            if ($(parentOfThis).attr('data-id') == $selectedNewsItemID) {
                return;
            }

            // retrieve information about the newsItem from the embedded HTML attributes
            var title = $(parentOfThis).attr('data-title');
            var imageURL = $(parentOfThis).attr('data-image');
            var url = $(parentOfThis).attr('data-url');

            // update the variable selectedNewsID
            $selectedNewsItemID = $(parentOfThis).attr('data-id');

            var content = "";

            jQuery.ajax({
                url: 'Default.aspx/getItemContent',
                type: "POST",
                data: "{ itemID: '" + $selectedNewsItemID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    content = result.d;
                    // if the article preview div is visible, just change the content
                    // Else, change the content, then show it.
                    var articlePreview = $('#articlePreview');
                    if (articlePreview.hasClass('visible')) {

                        $("#articlePreviewTitle").fadeOut(75, "swing");
                        $("#articlePreviewContent").fadeOut(75, "swing");
                        $("#articlePreviewImage").fadeOut(
                            75,
                            "swing",
                            function () {
                                $("#articlePreviewTitle").text(title);
                                $("#articlePreviewLink").attr('href', url);
                                $("#articlePreviewContent").text(content);
                                $("#articlePreviewImage").attr("src", imageURL)
                            }
                        ).fadeIn(75);
                        $("#articlePreviewTitle").fadeIn(75, "swing");
                        $("#articlePreviewContent").fadeIn(75, "swing");
                    }
                    else {
                        // 
                        $("#articlePreviewTitle").text(title);
                        $("#articlePreviewLink").attr('href', url);
                        $("#articlePreviewContent").text(content);
                        $("#articlePreviewImage").attr("src", imageURL)
                        var articlePreview = $('#articlePreview');
                        articlePreview.animate({ "left": "0px" }).addClass('visible');
                    }


                },
                error: function () {
                    //alert(arguments[2]);
                }
            });



            //#region deprecated code
            // lose focus of all logos
            //$(document).find('.articleSrc img').each(function () {
            //    var sourceImageURLofItem = $(this).attr("src");
            //    sourceImageURLofItem = sourceImageURLofItem.substring(0, sourceImageURLofItem.length - "-clkd.png".length) + ".png";
            //    if ($(this).attr("src").indexOf("-clkd.png") != -1) {
            //        $(this).attr("src", sourceImageURLofItem);
            //    }
            //});

            // get the sourceImageUrl and if it's not highlighted, highlight it
            //var sourceImageURL = $(this).find(".articleSrc img").attr("src");

            //if (sourceImageURL.indexOf("-clkd.png") == -1) {
            //    $(this).find(".articleSrc img").attr("src", sourceImageURL.substring(0, sourceImageURL.length - 4) + "-clkd.png");
            //}

            // get the parent articleFrame and highlight it
            //$(".article").parent().attr("class", "articleFrame")
            //$(this).find(".articleFrame").attr("class", "articleFrameClick");
            //#endregion

            //#region if the user is logged in, addPreference: Read
            if (userIsLoggedInJS == "True") {
                var selectedNewsItemID = $(parentOfThis).attr('data-id');
                //var overlay = $(this).find(".overlay");
                //overlay.fadeIn(150);
                //alert("reading started");
                jQuery.ajax({
                    url: 'Default.aspx/addPreference',
                    type: "POST",
                    data: "{ itemID: '" + selectedNewsItemID + "', type: 'read'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.d == "Server: success") {
                            //overlay.fadeOut(100);
                        }
                        else {
                            $("#errorPopup").fadeIn(50).fadeOut(400);
                            //overlay.fadeOut(100);
                        }
                    },
                    error: function () { alert(arguments[2]); }
                });

            }
            else {

            }
            //#endregion 
        });

    // clicking on the article back button
    $('#articleButton').click(
        function (event) {
            $selectedNewsItemID = 0;
            var articlePreview = $('#articlePreview');
            articlePreview.animate({ "left": "-1000px" }).removeClass('visible');
        });

    // showing article options on hover
    $(".articleWrapper").hover(
        function (event) {
            $(this).find('.articleOptions').fadeIn(100);
        }, function (event) {
            $(this).find('.articleOptions').fadeOut(100);
        });
    $(".articleWrapperMini").hover(
        function (event) {
            $(this).find('.articleOptions').fadeIn(100);
        }, function (event) {
            $(this).find('.articleOptions').fadeOut(100);
        });

    $(".articleOptions").click(function (event) {
        event.stopPropagation();
    });

    // changing the styling of the side bar
    $('#navReadLater').hover(function () {
        $('#button_read_later').css('color', 'white')
        $(this).css('background', 'rgba(28,50,89,1)')
        $('#iconLater').attr('src', 'images/later_small_h.png');
    }, function () {
        $('#button_read_later').css('color', 'black')
        $(this).css('background', '')
        $('#iconLater').attr('src', 'images/later_small.png');
    });

    $('#navReadLater').on('click', function () {
        $('#button_read_later').click();
    });

    $('#navHome').on('click', function () {
        window.location = $('#navHome a').attr('href');
    });

    $('#navHome').hover(function () {
        $(this).css('color', 'white')
        $(this).css('background', 'rgba(28,50,89,1)')
        $('#iconHome').attr('src', 'images/home_h.png');
    }, function () {
        $(this).css('color', 'black')
        $(this).css('background', '')
        $('#iconHome').attr('src', 'images/home.png');
    });

    //#region typing in the search box
    $("#searchBox").keypress(function (e) {
        var inputSearch = $(this).val();
        if (e.keyCode == 13 && canSendAjaxSearch && inputSearch != '') {
            //alert(inputSearch);
            $("#resultframe").slideUp(200, function () {
                $("#resultframe").html("<span class='searchloading'>جاري البحث...</span>");
                $("#resultframe").slideDown(200);
            });

            if (inputSearch != '' && canSendAjaxSearch) {
                canSendAjaxSearch = false;
                jQuery.ajax({
                    url: "Default.aspx/SearchQuery",
                    type: "POST",
                    data: "{ query: '" + inputSearch + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (html) {
                        $("#resultframe").slideUp(200, function () {
                            $("#resultframe").html(html.d);

                            $(".resultbox").off('click').on('click', function (event) {

                                // update the variable selectedNewsID
                                $selectedNewsItemID = $(this).attr('data-id');

                                var title = $(this).attr('data-title');
                                var imageURL = $(this).attr('data-image');
                                var url = $(this).attr('data-url');

                                jQuery.ajax({
                                    url: 'Default.aspx/getItemContent',
                                    type: "POST",
                                    data: "{ itemID: '" + $selectedNewsItemID + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (result) {

                                        var content = result.d;

                                        //#region change the content of the Article Preview div
                                        $("#articlePreview").fadeOut(
                                            75,
                                            "swing",
                                            function () {
                                                $("#articlePreviewTitle").text(title);
                                                $("#articlePreviewLink").attr('href', url);
                                                $("#articlePreviewContent").text(content);
                                                $("#articlePreviewImage").attr("src", imageURL)
                                            }
                                        ).fadeIn(75);
                                        //#endregion

                                        var articlePreview = $('#articlePreview');

                                        if (!articlePreview.hasClass('visible')) {
                                            articlePreview.animate({ "left": "0px" }).addClass('visible');
                                        }
                                    },
                                    error: function () {
                                        alert(arguments[2]);
                                    }
                                });



                                //#region addPreference: Read
                                var selectedNewsItemIDToAddToPreference = $(this).attr('data-id');
                                //var overlay = $(this).find(".overlay");
                                //overlay.fadeIn(150);
                                if (userIsLoggedInJS == "True") {
                                    jQuery.ajax({
                                        url: 'Default.aspx/addPreference',
                                        type: "POST",
                                        data: "{ itemID: '" + selectedNewsItemIDToAddToPreference + "', type: 'read'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (result) {
                                            if (result.d == "Server: success") {
                                                overlay.fadeOut(100);
                                            }
                                            else {
                                                $("#errorPopup").fadeIn(50).fadeOut(400);
                                                overlay.fadeOut(100);
                                            }
                                        },
                                        error: function () { alert(arguments[2]); }
                                    });
                                }
                                //#endregion
                            });
                            $("#resultfooter").off('click').on('click', function (event) {
                                var articlePreview = $('#articlePreview');
                                if (articlePreview.hasClass('visible')) {
                                    articlePreview.animate({ "left": "-1000px" }).removeClass('visible');
                                }
                                $("#UpdatePanel_leftStream_updateprogress").fadeIn(75);
                                $('#button_seeMore').click();
                            });

                        });
                        $("#resultframe").slideDown(200);

                        canSendAjaxSearch = true;

                    },
                    error: function () { alert(arguments[2]); }
                }).done(function () { }).fail(function () { });
            }
        }
        else if (e.keyCode == 13) {
            e.preventDefault();
            $("#searchBox").focus();
        }
    });
    //#endregion

    //#region handling clicking events inside and outside the search box
    // clicking outside the search box
    $(document).on("click", function (e) {
        var $clicked = $(e.target);
        if (!$clicked.hasClass("search")) {
            $("#resultframe").slideUp();
        }
    });

    // clicking inside the search box
    $("#searchBox").click(function () {
        $("#resultframe").slideDown();
    });
    //#endregion

    //#region article options handlers

    $('.optionShare').off('click').on('click', function (event) {
        if (userIsLoggedInJS == "True") {
            var selectedNewsItemID = $(this).parents(".articleWrapper,.articleWrapperMini").attr('data-id');
            var overlay = $(this).parents(".articleWrapper,.articleWrapperMini").find(".overlay");
            overlay.fadeIn(150);
            //alert("sharing started");
            jQuery.ajax({
                url: 'Default.aspx/addPreference',
                type: "POST",
                data: "{ itemID: '" + selectedNewsItemID + "', type: 'share'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d == "Server: success") {
                        overlay.fadeOut(100);
                    }
                    else {
                        overlay.fadeOut(100);
                        $("#errorPopup").fadeIn(50).fadeOut(400);
                    }
                },
                error: function () { alert(arguments[2]); }
            });
        }

        else {
        }
    });

    $('.optionLater').off('click').on('click', function (event) {

        if (userIsLoggedInJS == "True") {
            var selectedNewsItemID = $(this).parents(".articleWrapper,.articleWrapperMini").attr('data-id');
            var overlay = $(this).parents(".articleWrapper,.articleWrapperMini").find(".overlay");
            overlay.fadeIn(150);
            // adding preference
            jQuery.ajax({
                url: 'Default.aspx/addPreference',
                type: "POST",
                data: "{ itemID: '" + selectedNewsItemID + "', type: 'readLater'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d == "Server: success") {
                        overlay.fadeOut(100);
                        jQuery.ajax({
                            url: 'Default.aspx/getReadLaterCount',
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                $('#counter').html(result.d);
                            }
                        });
                    }
                    else if (result.d == "Server: duplicate") {
                        overlay.fadeOut(100);
                    }
                    else {
                        overlay.fadeOut(100);
                        $("#errorPopup").fadeIn(50).fadeOut(400);
                    }
                },
                error: function () { alert(arguments[2]); }
            });
            //overlay.fadeIn();
            // adding to read later
            //jQuery.ajax({
            //    url: 'Default.aspx/addToReadLater',
            //    type: "POST",
            //    data: "{ itemID: '" + selectedNewsItemID + "', type: 'x'}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (result) {
            //        if (result.d == "Server: success") {
            //            $('#counter').text(parseInt($('#counter').text()) + 1);
            //            overlay.fadeOut(100);
            //        }
            //        else if (result.d == "Server: fail") {
            //            $("#errorPopup").fadeIn(50).fadeOut(400);
            //            overlay.fadeOut(100);
            //        }
            //    },
            //    error: function () { alert(arguments[2]); }
            //});
        }
        else {
            showLoginRegisterBlock();
        }

    });

    $('.optionClose').off('click').on('click', function (event) {
        var this_optionClose = this;
        // if the user is logged in
        if (userIsLoggedInJS == "True") {

            if (($('#hfLeftStreamMode').attr('value') == 'ReadLater') && ($(this).parents('#leftStream').length > 0)) {
                //#region if it's an item in the read later list
                var selectedNewsItemID = $(this).parents(".articleWrapper,.articleWrapperMini").attr('data-id');
                var overlay = $(this).parents(".articleWrapper,.articleWrapperMini").find(".overlay");
                overlay.show();
                //alert("closing started");
                jQuery.ajax({
                    url: 'Default.aspx/addPreference',
                    type: "POST",
                    data: "{ itemID: '" + selectedNewsItemID + "', type: 'removeFromReadLater'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.d == "Server: success") {
                            overlay.fadeOut(100);
                            $(this_optionClose).parents(".articleWrapper, .articleWrapperMini").slideUp(550);
                            jQuery.ajax({
                                url: 'Default.aspx/getReadLaterCount',
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (result) {
                                    $('#counter').html(result.d);
                                }
                            });
                        }
                        else {
                            overlay.fadeOut(100);
                            $(this_optionClose).parents(".articleWrapper, .articleWrapperMini").slideUp(550);
                            $("#errorPopup").fadeIn(50).fadeOut(400);
                        }
                    },
                    error: function () { alert(arguments[2]); }
                });
                //#endregion
            }
            else {
                if ($(this_optionClose).parents(".newsStory").length > 0) {
                    //#region if it's a cluster
                    var selectedClusterID = $(this).parents(".newsStory").attr('data-clusterid');
                    //alert(selectedClusterID);
                    var overlay = $(this).parents(".articleWrapper,.articleWrapperMini").find(".overlay");
                    overlay.show();
                    jQuery.ajax({
                        url: 'Default.aspx/addPreference',
                        type: "POST",
                        data: "{ itemID: '" + selectedClusterID + "', type: 'ignoreCluster'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.d == "Server: success") {
                                overlay.hide();
                                $(this_optionClose).parents(".newsStory").slideUp(550);
                            }
                            else {
                                overlay.hide();
                                $(this_optionClose).parents(".newsStory").slideUp(550);
                                $("#errorPopup").fadeIn(50).fadeOut(400);
                            }
                        },
                        error: function () { alert(arguments[2]); }
                    });
                    //#endregion
                }
                else {
                    //#region if it's an item
                    var selectedNewsItemID = $(this).parents(".articleWrapper,.articleWrapperMini").attr('data-id');
                    var overlay = $(this).parents(".articleWrapper,.articleWrapperMini").find(".overlay");
                    console.log("overlay...");
                    console.log(overlay.length);
                    overlay.show();
                    //alert("closing started");
                    jQuery.ajax({
                        url: 'Default.aspx/addPreference',
                        type: "POST",
                        data: "{ itemID: '" + selectedNewsItemID + "', type: 'ignore'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.d == "Server: success") {
                                overlay.fadeOut(100);
                                $(this_optionClose).parents(".articleWrapper, .articleWrapperMini").slideUp(550);
                            }
                            else {
                                overlay.fadeOut(100);
                                $(this_optionClose).parents(".articleWrapper, .articleWrapperMini").slideUp(550);
                                $("#errorPopup").fadeIn(50).fadeOut(400);
                            }
                        },
                        error: function () { alert(arguments[2]); }
                    });
                    //#endregion
                }
            }

        }
        else {
            showLoginRegisterBlock();
        }
    });

    $('.optionShare').hover(function () {
        $(this).find('img').attr("src", "images/share_hover.png")
    }, function () {
        $(this).find('img').attr("src", "images/share.png")
    });

    $('.optionLater').hover(function () {
        $(this).attr("src", "images/later_hover.png")
    }, function () {
        $(this).attr("src", "images/later.png")
    });

    $('.optionClose').hover(function () {
        $(this).attr("src", "images/close_hover.png")
    }, function () {
        $(this).attr("src", "images/close.png")
    });

    //#endregion

    // login and register overlay handlers

    $(".link_login_register").click(
        function (event) {
            showLoginRegisterBlock();
        });


    $(".xbutton").click(
        function (event) {
            $(this).parent().fadeOut(
                250,
                "swing");
            // enable scrolling
            $('body').css("height", "auto");
            $('body').css("overflow", "scroll");
        });
    $("#xbutton_addFilter").click(function () {
        $('#addFilterOverlayBG').hide('slide');

    });


    //#region filters handlers
    $('.filterElmnt').not('.customFilterElmnt').click(function (event) {
        showUpdatePanel_rightStream_updateprogress();
        $('.filterElmnt').not('.customFilterElmnt').removeClass('selected');
        $(this).addClass("selected");
    });

    $('.customFilterElmnt').click(function (event) {
        showUpdatePanel_leftStream_updateprogress();
        $('#hfCustomFilter').attr("value", $(this).attr('data-filterContent'));
        $('.customFilterElmnt').removeClass('selected');
        $(this).addClass('selected');
        $('#button_customFilter').click();
    });

    $('#button_loadMoreNews').click(function (event) {
        showUpdatePanel_leftStream_updateprogress();

    });
    $('#button_reloadRightStream').click(function (event) {
        showUpdatePanel_rightStream_updateprogress();

    });

    $('.customFilterElmntDelete').click(function (event) {
        $('#UpdateProgress_userFilters').fadeIn(300);
        $('#hfCustomFilter').attr("value", $(this).attr('data-filterID'));
        $('#button_customFilterDelete').click();
    });
    //#endregion

    $('#button_read_later').click(function (e) {
        if (userIsLoggedInJS == "True") {
            showUpdatePanel_leftStream_updateprogress();
        }
        else {
            showLoginRegisterBlock();
            e.preventDefault();
            return false;
        }
    });

    $('#button_addFilter').click(function (event) {
        if (userIsLoggedInJS == "False") {
            showLoginRegisterBlock();
        }
        else {
            $('#addFilterOverlayBG').show('slide');
        }
    });

    $('#addFilterButton').click(function (event) {
        showUpdatePanel_rightStream_updateprogress();
    });

    $('#button_loadMoreNews_Recommended').on('click', function () {
        $('.articleTitle').tooltip('close');
    });

    // setting up the tooltips
    //$('.articleTitle').tooltip({
    //    content: function () {
    //        return $(this).attr('snippet');
    //    },
    //    items: "[snippet]",
    //    tooltipClassType: "tooltip-custom-styling",
    //    track: true,
    //    hide: { effect: "blind", duration: 150 },
    //    position: { my: "left top+15", at: "left bottom", collision: "flipfit" }
    //});

    // deleting all open tooltips
    $("[role='tooltip']").remove();

    var customFilterElmntDeleteWidth = $('.customFilterElmntDelete').outerWidth();

    $('.customFilterElmntContainer').hover(
        function () {
            $(this).find('.customFilterElmntDelete').animate({ "left": "0px" }, 150);
        },
        function () {
            $(this).find('.customFilterElmntDelete').animate({ "left": '-' + customFilterElmntDeleteWidth + 'px' }, 150);
        });


    enableScrollingDetection = true;
    enableScrollingDetectionRight = true;
    enableScrollingDetectionLeft = true;
    scrollOKRight = true;
    scrollOKLeft = true;

    // auto scrolling detection
    //$('#rightStream').bind('scroll', function () {
    //    //check if it's OK to run code
    //    if (scrollOKRight) {

    //        //set flag so the interval has to reset it to run this event handler again
    //        scrollOKRight = false;

    //        //check if the user has scrolled within 100px of the bottom of the page
    //        if ($(this).scrollTop() + $(this).innerHeight() >= (this.scrollHeight - 400)) {
    //            if (false) {
    //                //alert("HEY");
    //                $('#button_loadMoreNews_Recommended').click();
    //                enableScrollingDetectionRight = false;
    //            }
    //        }
    //    }
    //});
    //$('#leftStream').bind('scroll', function () {
    //    //check if it's OK to run code
    //    if (scrollOKLeft) {

    //        //set flag so the interval has to reset it to run this event handler again
    //        scrollOKLeft = false;

    //        //check if the user has scrolled within 100px of the bottom of the page
    //        if ($(this).scrollTop() + $(this).innerHeight() >= (this.scrollHeight - 400)) {
    //            if (false) {
    //                if ($('#hfLeftStreamMode').attr('value') == 'LatestNews') {
    //                    alert("updating left stream...\nAdding 10 items starting from ID: " + $('#hflastIDLatestNews').attr('value'));
    //                    $('#button_loadMoreNews').click();
    //                    enableScrollingDetectionLeft = false;
    //                }
    //            }
    //        }
    //    }
    //});

    //$('.newsStory').anythingSlider({
    //    enableStartStop: false,
    //    enableKeyboard: false,
    //    buildNavigation: false,
    //    buildStartStop: false,
    //    buildArrows: false,
    //    hashTags: false,
    //    resizeContents: true,
    //    animationTime: 300,
    //    onInitialized: function (e, slider) {
    //        $('.triLeft').click(function () {
    //            $(this).parents('.newsStory').data('AnythingSlider').goForward();
    //        });
    //        $('.triRight').click(function () {
    //            $(this).parents('.newsStory').data('AnythingSlider').goBack();
    //        });
    //    }
    //});


    //#region Custom Scrolling

    $('.enscroll-track').parent().remove()
    $('#leftStream').removeAttr('style');
    $('#rightStream').removeAttr('style');

    var scrollingSettings1 = {
        showOnHover: true,
        verticalTrackClass: 'track3',
        verticalHandleClass: 'handle3',
        scrollIncrement: 80,
        easingDuration: 350
    };
    var scrollingSettings2 = {
        showOnHover: true,
        verticalTrackClass: 'track4',
        verticalHandleClass: 'handle3',
        scrollIncrement: 80,
        easingDuration: 350
    };

    $('#leftStream').enscroll(scrollingSettings1);
    $('#rightStream').enscroll(scrollingSettings1);

    $('#articlePreview').enscroll(scrollingSettings2);
    $('.track4').parent().css('z-index', '14')
    $('#articlePreview').css('padding-right', '20px');
    $('#articlePreview').css('width', '404px');

    //#endregion

    $('.styleSelectFix').on('click', function (event) {
        $($(this).parent())
    });

    // fixing the count of news in each story
    //$('.newsIndexInStory').each(function () {
    //    if ($(this).data("fixed") == false) {
    //        $(this).html($(this).html() + $(this).parents('.newsStory').data('storiescount'));
    //        $(this).data("fixed", true);
    //        console.log("ok");
    //    }
    //});

    if ($('#hfLeftStreamMode').attr('value') == 'LatestNews') {
        $('#button_loadMoreNews').css('display', 'block');
    }
    else {
        $('#button_loadMoreNews').css('display', 'none');
    }

    $('#button_loadMoreNews_Recommended').hover(function () {
        var goal = $('#rightStream')[0].scrollHeight;
        $('#rightStream').animate({
            scrollTop: String(goal)
        }, {
            duration: 200
        });
    }, function () {

    });

    $('#button_customNewsApply').on('click', function (event) {
        showUpdatePanel_leftStream_updateprogress();
    });

    $('.help').on('click', function (event) {

    });

    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            $selectedNewsItemID = 0;
            var articlePreview = $('#articlePreview');
            articlePreview.animate({ "left": "-1000px" }).removeClass('visible');
        }
    });

};

$(document).ready(function ($) {

    enableImages = '<%=Session["enableImages"]%>';
    $('#imagesToggle').off('click').on('click', function () {
        jQuery.ajax({
            url: 'Default.aspx/toggleImages',
            type: "POST",
            data: "{ }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d=="success") {
                    window.location.reload();
                }
            },
            error: function () {
                alert(arguments[2]);
            }
        });
    });

    preload([
        '../images/up-grey-arrow.png',
        '../images/up-grey-arrow_hover.png',
        '../images/down-grey-arrow.png',
        '../images/down-grey-arrow_hover.png',
        '../images/logo.png',
        '../images/share.png',
        '../images/share_hover.png',
        '../images/close.png',
        '../images/close_hover.png',
        '../images/later_small.png',
        '../images/later_small_h.png',
        '../images/later_hover.png',
        '../images/home.png',
        '../images/home_h.png',
        '../images/help/01.jpg',
        '../images/help/02.jpg',
        '../images/help/03.jpg'
    ]);

    $('#helpSlideshow').flexslider({
        animation: "fade",
        slideshow: false,
        directionNav: false,
        contolNav: false
    });

    $('#nav_prev').on('click', function () {
        $('#helpSlideshow').flexslider('prev');
        return false;
    });
    $('#nav_next').on('click', function () {
        $('#helpSlideshow').flexslider('next');
        return false;
    });


    $('#xbuttonSlideShow').on('click', function (event) {
        $('#helpOverlay').fadeOut();
    });

    $('.help').on('click', function (event) {
        $('#helpOverlay').fadeIn();
    });
    var resultLatestStories;
    jQuery.ajax({
        url: 'Default.aspx/LatestStories',
        type: "POST",
        data: "{ }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            resultLatestStories = result.d;
            var story = JSON.parse(result.d);
            var destination = $('#storiesStream');
            destination.html('');
            for (var i = 0; i < story.length; i++) {
                destination.append(makeStory(story[i]));
            }
            hideUpdatePanel_rightStream_updateprogress();
            //alert(result.d);
            $('#button_loadMoreNews_Recommended').show();
            loadHandlers();
        },
        error: function () {
            alert(arguments[2]);
        }
    });
});

