using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace WebCrawlerIV
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string RequestBody;
            try
            {
                RequestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation($"Received request body: {RequestBody}");
            }
            catch (Exception ex)
            {
                log.LogError($"Error reading request body: {ex.Message}");
                return new BadRequestObjectResult("Failed to read request body.");
            }

            string[] TheResult;

            try
            {
                TheResult = await CrawlBlog(RandomInt(11), RequestBody);
            }
            catch (Exception ex)
            {
                log.LogError($"Error in CrawlBlog: {ex.Message}");
                return new BadRequestObjectResult("An error occurred while processing the blog.");
            }

            if (TheResult == null || TheResult.Length < 6)
            {
                return new BadRequestObjectResult("Invalid result data.");
            }


            string FstResult = TheResult[0]; string SndResult = TheResult[1];
            string TrdResult = TheResult[2]; string FthResult = TheResult[3];
            string FifthResult = TheResult[4];
            string SixthResult = TheResult[5];

            return new OkObjectResult(new
            {
                FstResult,
                SndResult,
                TrdResult,
                FthResult,
                FifthResult,
                SixthResult
            });

        }

        private static readonly Dictionary<int, string> blogUrls = new()
        {
            { 0, "https://turbo360.com/blog" },
            { 1, "https://css-tricks.com/" },
            { 2, "https://azure.microsoft.com/en-in/blog/?utm_source=devglan" },
            { 3, "https://unity.com/blog?filter=editor" }, //bad
            { 4, "https://www.unrealengine.com/en-US/feed/blog" }, //403 error
            { 5, "https://medium.com/better-programming" },
            { 6, "https://reliance.systems/blog/" }, //not suing
            { 7, "https://www.upgrad.com/blog/software-development/" }, //not using
            { 8, "https://www.workfall.com/learning/blog/category/backend-development/?utm_source=feedspot" },
            { 9, "https://iximiuz.com/en/" },
            { 10, "https://careersatdoordash.com/engineering-blog/?utm_source=feedspot" },
            { 11, "https://medium.com/coders-camp/230-machine-learning-projects-with-python-5d0c7abf8265" } //not using
        };

        /// <summary>
        /// persoanl random number 
        /// return Intenger
        /// </summary>
        /// <param name="Mxn"></param>
        /// <returns></returns>
        private static int RandomInt(int Mxn)
        {
            Random randy = new Random();

            int RandNum = randy.Next(0, Mxn);

            return RandNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Picker">The intger sent from random function</param>
        /// <param name="req">Message from logic apps will be the day of the week</param>
        /// <returns></returns>
        private static async Task<string[]> CrawlBlog(int Picker, string req)
        {
            int BlogPicker = Picker;

            string[] TheResult = new string[6];

            TheResult[0] = req; //will be overwrriten

            TheResult[5] = req;
            bool Running = true;
            bool ItsFailed = false;
            //Get The url we want to go
            var Url = "";

            //convert to integer from string
            int DoW = int.Parse(TheResult[5]); //get the intenger

            int NewPick = -100;
            NewPick = RandomInt(3);

            if (DoW > 0)
            {
                //Now will be using the day of the week to edit
                //if value is greater zero
                //will overide the random number pass in the function
                //i.e blogpicker will be overwrriten if a case is valid
               
                switch (DoW)
                {
                    case 0:
                        {
                            if (NewPick == 1)
                            {
                                BlogPicker = 10; //doordash
                            }
                            else if (NewPick == 2)
                            {
                                BlogPicker = 7; //fullstack
                            }
                            else
                            {
                                BlogPicker = 2; //azure
                            }

                            break;
                        }


                    case 1:
                        {
                            if (NewPick == 0)
                            {
                                BlogPicker = 1; //serverless
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 8; //azure
                            }
                            else
                            {
                                BlogPicker = 10; //doordash fullstack
                            }

                            break;
                        }

                    case 2:
                        {
                            if (NewPick == 0)
                            {
                                BlogPicker = 7; //up-grad fullstack
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 1; //CSS Tricks
                            }
                            else
                            {
                                BlogPicker = 10; //DoorDash
                            }
                            break;
                        }

                    case 3:
                        {

                            if (NewPick == 0)
                            {
                                BlogPicker = 5; //meduim
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 8; //back-end
                            }
                            else
                            {
                                BlogPicker = 11; //machine learning
                            }
                            break;
                        }

                    case 4:
                        {
                            if (NewPick == 0)
                            {
                                BlogPicker = 1; //CSS Tricks
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 7; //full-stack
                            }
                            else
                            {
                                BlogPicker = 2; //azure
                            }
                            break;
                        }

                    case 5:
                        {
                            if (NewPick == 0)
                            {
                                BlogPicker = 7; //serverless
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 5; //meduim
                            }
                            else
                            {
                                BlogPicker = 10; //doordash
                            }
                            break;
                        }

                    case 6:
                        {
                            if (NewPick == 1)
                            {
                                BlogPicker = 1; //azure
                            }
                            else
                            {
                                BlogPicker = 8; //backend
                            }
                            break;
                        }

                    case 7:
                        {
                            if (NewPick == 0)
                            {
                                BlogPicker = 11; //meduim
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 1; //CSS Tricks
                            }
                            else
                            {
                                BlogPicker = 7; //fullstack
                            }
                            break;
                        }

                    default:
                        {
                            TheResult[5] += " Case wasnt Passed ";
                            break;
                        }
                }
            }

            do


            {
                if (blogUrls.ContainsKey(BlogPicker))
                {
                    Url = blogUrls[BlogPicker];
                }
                else
                {
                    ItsFailed = true;
                }

                if (BlogPicker == 4)
                {
                    // Running = true;
                    //to prevent this blogs from being picked
                    BlogPicker = 2;// RandomInt(12); //get a new picker

                }
                else if (ItsFailed)
                {
                    Running = false;
                }
                else
                {
                    Running = false;
                }

            } while (Running);



            if (ItsFailed)
            {
                TheResult[0] = "Failure";
                TheResult[1] = "We got an issue";
                TheResult[2] = "Fix this BlogPicker " + BlogPicker;
                TheResult[3] = ":D";
                TheResult[4] = "No Image";
                TheResult[5] = "-1";
                return TheResult;
            }


            using (HttpClient httpClient = new HttpClient())
            {
                // Set a custom user-agent to avoid getting blocked
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

                try
                {
                    var html = await httpClient.GetStringAsync(Url);
                    var htmlDocument = new HtmlDocument();

                    htmlDocument.LoadHtml(html);



                    //a list to add all availabel blogs we found
                    var Blog = new List<BlogStat>();


                    static string ExtractImageUrl(string styleAttribute)
                    {

                        if (string.IsNullOrWhiteSpace(styleAttribute))
                        {
                            Console.WriteLine("Empty string or whitespace");
                            return "https://th.bing.com/th/id/OIP.fAiNZID9Y4I5wwe3rMSY6AHaEo?w=244&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7";
                        }

                        // Use regular expression to find the URL pattern within the style attribute
                        Match match = Regex.Match(styleAttribute, @"url\(&quot;([^&]+)&quot;\)");

                        // Check if a match is found
                        if (match.Success && match.Groups.Count > 1)
                        {
                            // Get the URL from the first capturing group
                            string url = match.Groups[1].Value;

                            // Replace HTML entities like &quot; with actual characters
                            url = System.Net.WebUtility.HtmlDecode(url);

                            return url;
                        }

                        Console.WriteLine("No match found");
                        return "https://th.bing.com/th/id/OIP.fAiNZID9Y4I5wwe3rMSY6AHaEo?w=244&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7";
                    }

                    static string GetBackupImageUrl()
                    {
                        List<string> backupImageUrls = new List<string>
            {
                "https://th.bing.com/th/id/R.d31fcfb02bb041c614d1344fa2c25327?rik=AaLXxZ3BD7cn0A&pid=ImgRaw&r=0",
                "https://e0.pxfuel.com/wallpapers/764/807/desktop-wallpaper-u-alienware-logo-blue.jpg",
                "https://th.bing.com/th/id/OIP.1Q_sPzE0Nc13qLFD80Xp2QHaEK?rs=1&pid=ImgDetMain",
                "https://th.bing.com/th/id/R.af9174e9cf95411694da5b929711286c?rik=Rz%2bdTDZJaLUZoQ&pid=ImgRaw&r=0",
                "https://th.bing.com/th/id/R.54012efd936e516cb24f7e8a9ba8afb2?rik=r%2biA2t8h62fxzA&riu=http%3a%2f%2fwww.quytech.com%2fblog%2fwp-content%2fuploads%2f2019%2f06%2fUnity-vs-Unreal-Engine.jpg&ehk=l82DlMLvgGIhEPiM1M%2bNjG2WhDvCHrRNsEGxVG0w3sY%3d&risl=&pid=ImgRaw&r=0",
                "https://cdn.akamai.steamstatic.com/steam/apps/1809540/extras/enemies_boss.png?t=1692978426"
            };

                        Random random = new Random();
                        int randomIndex = random.Next(backupImageUrls.Count);
                        return backupImageUrls[randomIndex];
                    }

                    switch (BlogPicker)
                    {
                        case 0: // Turbo360 blog within specific container
                            {
                                // Locate the main container for the blog list
                                var container = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'blog-list bg-blue-light py-80')]");

                                if (container != null)
                                {
                                    // From the main container, find all article blocks
                                    var products = container.SelectNodes(".//div[contains(@class, 't360-card')]");
                                    if (products != null)
                                    {
                                        foreach (var div in products)
                                        {
                                            // Extract author information, date, etc.
                                            string author = div
                                                .SelectSingleNode(".//p[@class='text-xs']")?.InnerText.Trim()
                                                ?? "Unknown Author";

                                            // Extract title
                                            string title = div
                                                .SelectSingleNode(".//h5/a")?.InnerText.Trim()
                                                ?? "No Title";

                                            // Extract summary (assumed as the immediate sibling of the paragraph with class 'text-xs')
                                            string summary = div
                                                .SelectSingleNode(".//p[@class='text-xs']/following-sibling::p[1]")?.InnerText.Trim()
                                                ?? "No Summary";

                                            // Extract link
                                            string link = div
                                                .SelectSingleNode(".//h5/a")?.GetAttributeValue("href", "#")
                                                ?? "#";

                                            // Extract image source (from <img> inside <a> within <figure>)
                                            string img = div
                                                .SelectSingleNode(".//figure/a/img")?.GetAttributeValue("src", "")
                                                ?? GetBackupImageUrl();

                                            var Sumo = "Article by " + author + ": " + summary;
                                            BlogStat Blgo = new BlogStat(link, Sumo, title, img);

                                            Blog.Add(Blgo);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("No blog articles found in the specified container.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("The specified container was not found in the HTML.");
                                }
                                break;
                            }
                        case 1: //css-tricks
                            {
                                // Adjust this XPath to select relevant articles or sections
                                var products = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'latest-articles')]//article");

                                if (products != null)
                                {
                                    foreach (var prod in products)
                                    {
                                        var titleNode = prod.SelectSingleNode(".//h2/a");
                                        string title = titleNode?.InnerText.Trim() ?? "No Title";
                                        string link = titleNode?.GetAttributeValue("href", "") ?? "#";
                                        string summary = prod.SelectSingleNode(".//div[contains(@class, 'card-content')]/p")?.InnerText.Trim() ?? "Summary Missing";
                                        string author = prod.SelectSingleNode(".//a[contains(@class, 'author-name')]")?.InnerText.Trim() ?? "Not Me";
                                        string img = prod.SelectSingleNode(".//img")?.GetAttributeValue("src", "") ?? GetBackupImageUrl();

                                        var Sumo = "Article by " + author + ": " + summary;

                                        BlogStat Blogo = new BlogStat(link, Sumo, title, img);

                                        Blog.Add(Blogo);
                                    }

                                }
                                break;
                            }
                        case 2: // Azure Blog example
                            {
                                var products =
                                    htmlDocument.DocumentNode.SelectNodes("//div[@class='wp-block-msx-content-card']//article[contains(@class, 'msx-card')]");
                                if (products != null)
                                {
                                    foreach (var div in products)
                                    {
                                        string title = div.SelectSingleNode(".//h3/a/span")?.InnerText.Trim() ?? "Azure Blog Post";
                                        string link = div.SelectSingleNode(".//h3/a")?.GetAttributeValue("href", "") ?? "#";
                                        string summary = div.SelectSingleNode(".//div[contains(@class, 'msx-card__content')]")?.InnerText.Trim() ?? "Discover a variety of insights, updates, and stories about Microsoft's cloud platform, covering AI, security, analytics, and more.";
                                        string author = div.SelectSingleNode(".//div[contains(@class, 'msx-card__byline')]/a")?.InnerText.Trim() ?? "Unknown Author";
                                        string img = div.SelectSingleNode(".//img")?.GetAttributeValue("src", "") ?? GetBackupImageUrl();

                                        summary = $"Article by {author}: {summary}";
                                        var Blogo = new BlogStat(link, summary, title, img);

                                        Blog.Add(Blogo);
                                    }
                                }
                                break;
                            }
                        case 3:
                            {
                                //w-full min-h-full flex flex-col

                                var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='w-full flex flex-wrap pb-10 justify-center mt-8']/div[contains(@class, 'w-full')]");
                                Console.WriteLine($"The Div {products.Count} \n");

                                List<string> ImgList = new List<string>()
                        { "https://unity.com/sites/default/files/styles/cards_16_9/public/2021-08/unity-pro-card.jpeg.jpg?itok=TeJ3l-by",
                            "https://blog-api.unity.com/sites/default/files/styles/focal_crop_ratio_3_1/public/2022-06/Blog%20header-1230x410%20%282%29.png?imwidth=1260&h=3643c662&itok=NPwXjRlB",
                            "https://www.protocol.com/media-library/unity-logo.jpg?id=30046849&width=1200&height=600&coordinates=341%2C0%2C341%2C0",
                            "https://www.cgdirector.com/wp-content/uploads/media/2021/10/Unity-3D-System-Requirements-Twitter-1200x675.jpg",
                            "https://blog.codemagic.io/uploads/covers/codemagic-blog-header-mix-unity.png"};

                                foreach (var divo in products)
                                {
                                    Random randy = new Random();
                                    int randum = randy.Next(ImgList.Count);


                                    // Extract title
                                    var titleNode = divo.SelectSingleNode(".//h3[@class='loco-text-body-lg-medium']");
                                    string title = titleNode?.InnerText.Trim() ?? "No Title";

                                    // Extract link
                                    var linkNode = divo.SelectSingleNode(".//a[@role='button']");
                                    string link = linkNode?.GetAttributeValue("href", "") ?? "#";

                                    // Extract image URL
                                    var imgNode = divo.SelectSingleNode(".//img");
                                    string img = imgNode?.GetAttributeValue("src", "") ?? ImgList[randum];

                                    // Extract author
                                    // Assuming author information is included in a specific class/element
                                    var authorNode = divo.SelectSingleNode(".//div[@class='author-info' or similar]");
                                    string author = authorNode?.InnerText.Trim() ?? "Unknown Author";

                                    // Extract summary
                                    var summaryNode = divo.SelectSingleNode(".//div[contains(@class, 'loco-text-body')]");
                                    string summary = summaryNode?.InnerText.Trim() ?? "Explore the latest insights and innovations.";
                                    var Sumo = "Article By " + author + "\n" + "Unity news and features from Unity Blog";

                                    var Blogo = new BlogStat(link, Sumo, title, img);


                                    Blog.Add(Blogo);
                                }

                                break;
                            }
                        case 4:
                            {
                                /*
                                //w-full min-h-full flex flex-col
                                var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='sl-blog-posts']/div[@class='sl-blog-posts__item']");
                                Console.WriteLine($"The Div {products.Count} \n");

                                foreach (var divo in products)
                                {
                                    var Blogo = new BlogStat
                                    {
                                        Summary = divo.SelectSingleNode(".//span[@class='sl-blog-item__topic']").InnerText,
                                        Title = divo.SelectSingleNode(".//h2[@class='sl-blog-item__title']").InnerText,
                                        Link = divo.SelectSingleNode(".//a").ChildAttributes("href").FirstOrDefault().Value
                                    };
                                    Blog.Add(Blogo);
                                }

                                */

                                Console.WriteLine("got em");
                                // FeedCardcontainer__FeedCardWrapper - sc - kdxk6b - 0 eQiFpl
                                //Changing to unreal



                                var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='SmallFeedCard__SmallFeedCardWrapper-sc-6x1tbj-0 fOBZeL feed-item']");
                                var prodimg = htmlDocument.DocumentNode.SelectNodes("//div[@class='image-wrapper']");
                                Console.WriteLine($"The Div {products.Count}  {prodimg.Count} \n");
                                /*
                                    var ImgName = htmlDocument.DocumentNode.SelectNodes(".//a[@class='simple']");

                                    string ImgSrc = "https://cdn2.unrealengine.com/m2m-02-1920x1080-313b6089e0e7.jpg";

                                    List<string> ImgList = new List<string>();

                                    if(ImgName != null)
                                    {
                                        foreach (var dimg in ImgName)
                                        {

                                            var Sup = dimg.SelectSingleNode(".//img").ChildAttributes("src").FirstOrDefault().Value;
                                            if (Sup != null || !string.IsNullOrEmpty(Sup) || !string.IsNullOrWhiteSpace(Sup))
                                                ImgList.Add(Sup);
                                        }
                                    }


                                    if(ImgList.Count>1)
                                    {
                                        int randy = RandomInt(ImgList.Count);

                                        ImgSrc = ImgList[randy];
                                    }



                                    var Splitted = ImgSrc.Split("?");
                                    ImgSrc = Splitted[0];
                                    */
                                //unreal
                                // var div = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'searched-img')]");
                                //var url = Regex.Match(div.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value;
                                foreach (var divo in products)
                                {
                                    var lastKid = divo.LastChild;
                                    var NewImg = divo.LastChild.SelectNodes(".//div[@class='image-wrapper']");

                                    var Sumnews = lastKid.InnerText;
                                    Sumnews = Sumnews.Substring(0, Sumnews.IndexOf("News")); // summary

                                    string src = NewImg[0].InnerHtml.Substring(NewImg[0].InnerHtml.IndexOf("src=") + 5);

                                    // Remove everything after the src value (the end of the img tag). 
                                    src = src.Substring(0, src.IndexOf("\""));
                                    src = src.Substring(0, src.IndexOf("?resize"));

                                    var LinkUrl = lastKid.SelectSingleNode(".//a").ChildAttributes("href").FirstOrDefault().Value;

                                    var LinkUse = "https://www.unrealengine.com" + LinkUrl;
                                    var Sumo = divo.LastChild.InnerText;


                                    var Blogo = new BlogStat(LinkUse, Sumo, "Unreal Engine Announcements", src);

                                    Blog.Add(Blogo);



                                }

                                break;
                            }
                        case 5:
                            {
                                //mv mw mx l

                                var products = htmlDocument.DocumentNode.SelectNodes("//div[contains(@data-source, 'collection_home')]");  //l eo gs
                                                                                                                                           // var productsimg = htmlDocument.DocumentNode.SelectNodes("//div[@class='or os ot ou ov l']");
                                Console.WriteLine($"The Div {products.Count} \n");

                                // Console.WriteLine($"The Div for imgs  {productsimg.Count} \n");
                                int Count = 0;
                                foreach (var divo in products)
                                {
                                    // if (divo != null )
                                    {

                                        // Get link
                                        string link = divo.SelectSingleNode(".//a[@data-action='open-post']")?.GetAttributeValue("href", "");


                                        // Get image
                                        string imageUrl = divo.SelectSingleNode(".//div[@class='u-lineHeightBase postItem']/a")?.GetAttributeValue("style", "");
                                        // Console.WriteLine("style URL: " + imageUrl);

                                        string FimageUrl = ExtractImageUrl(imageUrl);
                                        //  Console.WriteLine("Image URL: " + FimageUrl);

                                        // Get title
                                        string title = divo.SelectSingleNode(".//h3/div")?.InnerText.Trim() ?? "Medium Post";


                                        // Get summary
                                        string summary = divo.SelectSingleNode(".//div[@class='u-contentSansThin u-lineHeightBaseSans u-fontSize24 u-xs-fontSize18 u-textColorNormal u-baseColor--textNormal']")?.InnerText.Trim();


                                        // Get author
                                        string Autho = "Medium Post";

                                        var AuthoDoc = divo.SelectNodes("//div[@class='postMetaInline postMetaInline-authorLockup ui-captionStrong u-flex1 u-noWrapWithEllipsis']");

                                        var AuthoSdoc = AuthoDoc[Count];
                                        if (AuthoDoc != null)
                                        {
                                            // Get author name
                                            Autho = AuthoSdoc.SelectSingleNode(".//a")?.InnerText.Trim();

                                        }

                                        var Sumo = "Article By " + Autho + "\n" + summary;


                                        //no summary
                                        var Blogo = new BlogStat(link, Sumo, title, FimageUrl);

                                        Blog.Add(Blogo);


                                    }
                                    Count++;
                                }
                                break;
                            }
                        case 6:
                            {
                                //elementor-posts-container elementor-posts elementor-posts--skin-cards elementor-grid elementor-has-item-ratio

                                var products = htmlDocument.DocumentNode.SelectNodes("//article");
                                Console.WriteLine($"The Div {products.Count} \n");

                                foreach (var divo in products)
                                {
                                    // Get link
                                    string link = divo.SelectSingleNode(".//a[@href]")?.GetAttributeValue("href", "");

                                    // Get image
                                    string imageUrl = divo.SelectSingleNode(".//div[contains(@class, 'blog-bg-image-metro')]")?
                                        .GetAttributeValue("style", "")
                                        .Split("url(")[1]
                                        .TrimEnd(';')
                                        .TrimEnd(')');

                                    if (imageUrl != null)
                                    {
                                        imageUrl = "https://reliance.systems" + imageUrl;
                                    }

                                    // Get title
                                    string title = divo.SelectSingleNode(".//h3/a")?.InnerText.Trim();

                                    // Get summary
                                    string summary = divo.SelectSingleNode(".//div[@class='entry-content']")?.InnerText.Trim();

                                    // Get author
                                    string author = divo.SelectSingleNode(".//span[@class='post-author']/a")?.InnerText.Trim();

                                    var Sumo = "Author: " + author + "\n" + summary;
                                    var Blogo = new BlogStat(link, Sumo, title, imageUrl);


                                    Blog.Add(Blogo);
                                }

                                break;
                            }

                        case 7:
                            {

                                var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='element-item ajax-result-item ']");  //l eo gs



                                int counter = 0;



                                foreach (var divo in products)
                                {
                                    if (divo != null)
                                    {
                                        var LinkTest = divo.SelectSingleNode("a").ChildAttributes("href").FirstOrDefault().Value;
                                        var TitleTest = divo.SelectSingleNode(".//h2[@class='catetemp-posttitle']").InnerText;
                                        TitleTest = TitleTest.Replace("\n", ""); //remove white space
                                        var ImgTest = divo.SelectSingleNode("a/div/img").GetAttributeValue("src", "nothing");
                                        var SummaryTest = divo.SelectSingleNode(".//div[@class='catetemp-postcnt']").InnerText;
                                        SummaryTest = SummaryTest.Replace("\n", ""); //remove white space

                                        var Blogo = new BlogStat(LinkTest, SummaryTest, TitleTest, ImgTest);

                                        Blog.Add(Blogo);

                                        counter++;
                                    }
                                }
                                break;
                            }

                        case 8:
                            {

                                var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='archive-grid-post-wrapper']/article");  //l eo gs

                                Console.WriteLine($"The Div {products.Count} \n");



                                foreach (var divo in products)
                                {
                                    if (divo != null)
                                    {
                                        // Get link
                                        string link = divo.SelectSingleNode(".//a[@class='post-thumbnail']")?.GetAttributeValue("href", "");
                                        //  Console.WriteLine("Link: " + link);

                                        // Get image
                                        string imageUrl = divo.SelectSingleNode(".//img[@class='attachment-color-blog-dark-full-width size-color-blog-dark-full-width wp-post-image']")?
                                            .GetAttributeValue("src", "");
                                        // Console.WriteLine("Image URL: " + imageUrl);

                                        // Get title
                                        string title = divo.SelectSingleNode(".//h2[@class='entry-title']/a")?.InnerText.Trim();
                                        // Console.WriteLine("Title: " + title);

                                        // Get summary
                                        string summary = divo.SelectSingleNode(".//div[@class='entry-content']/p")?.InnerText.Trim();
                                        //Console.WriteLine("Summary: " + summary);

                                        // Get author
                                        string author = divo.SelectSingleNode(".//span[@class='author vcard']/a")?.InnerText.Trim();
                                        //Console.WriteLine("Author: " + author);

                                        var Sumo = "By: " + author + "\n" + summary;

                                        var Blogo = new BlogStat(link, Sumo, title, imageUrl);

                                        Blog.Add(Blogo);


                                    }
                                }
                                break;
                            }

                        case 9:
                            {

                                var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='article-entry markdown-body']/div/div/div[@class='frontpage-grid']/div/div/ul/li");  //l eo gs

                                Console.WriteLine($"The Div {products.Count} \n");

                                //https://kubernetes.io/images/favicon.png
                                //https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS5neXBOnJNeaKmh2PwHdF_SnhLd2UOOZ4hZw&usqp=CAU
                                //https://img-0.journaldunet.com/2sFNEidxsH0BuBYpiWYovYrY9WE=/1500x/smart/4af81639040d4827b9e6d27a27ba5cc1/ccmcms-jdn/32083536.jpeg
                                //https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRp1IYzHRlchiHhbs59dwNOWAlAg22EyJJwCw&usqp=CAU

                                List<string> ImgList = new List<string>()
                        { "https://kubernetes.io/images/favicon.png", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS5neXBOnJNeaKmh2PwHdF_SnhLd2UOOZ4hZw&usqp=CAU", "https://img-0.journaldunet.com/2sFNEidxsH0BuBYpiWYovYrY9WE=/1500x/smart/4af81639040d4827b9e6d27a27ba5cc1/ccmcms-jdn/32083536.jpeg", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRp1IYzHRlchiHhbs59dwNOWAlAg22EyJJwCw&usqp=CAU" };

                                foreach (var divo in products)
                                {
                                    if (divo != null)
                                    {
                                        var LinkTest = "https://iximiuz.com" + divo.SelectSingleNode("a").ChildAttributes("href").FirstOrDefault().Value;
                                        var TitleTest = divo.SelectSingleNode("a").InnerText;

                                        TitleTest = TitleTest.Replace("\n", "").Replace("\r", ""); //remove white space
                                        Random randy = new Random();
                                        int randum = randy.Next(ImgList.Count);
                                        var ImgTest = ImgList[randum];

                                        var Sumo = "Learning Containers, Kubernetes, and Backend Development with Ivan Velichko";

                                        var Blogo = new BlogStat(LinkTest, Sumo, TitleTest, ImgTest);

                                        Blog.Add(Blogo);


                                    }
                                }
                                break;
                            }

                        case 10:
                            {

                                var products = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'publication-list')]/div/a");

                                Console.WriteLine($"The Div {products.Count} \n");

                               string GetDoorDashImageUrl()
                                    {
                                            List<string> backupImageUrls = new List<string>
                                            {
                                                "https://th.bing.com/th/id/OIP.YUgdF5IWa8mDjTSv4QndmgHaEK?rs=1&pid=ImgDetMain",
                                                "https://th.bing.com/th/id/OIP.GyTlB_Uy3Xj34YSlrIbnwAHaEN?w=301&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7",
                                                "https://th.bing.com/th/id/OIP.1Q_sPzE0Nc13qLFD80Xp2QHaEK?rs=1&pid=ImgDetMain",
                                                "https://cdn.dribbble.com/users/6758918/screenshots/16744461/media/f3c3cabca54f9e9c3acb490bea17e5bf.png",
                                                "https://th.bing.com/th/id/OIP.QNWuVL23N-jr9ttQ8C1EXwHaE7?rs=1&pid=ImgDetMain",
                                                "https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/d0d106121538797.60c8296fe53d4.png",

                                            };

                                    Random random = new Random();
                                    int randomIndex = random.Next(backupImageUrls.Count);
                                    return backupImageUrls[randomIndex];
                                }



                                foreach (var divo in products)
                                {
                                    if (divo != null)
                                    {
                                        //HtmlNode ArticleNode = divo.SelectSingleNode(".//div[@class='featured card clearfix']");

                                        //if (ArticleNode != null)
                                        {
                                            // Get link
                                            string link = divo.GetAttributeValue("href", "");
                                        

                                            // Get image URL from the style attribute
                                            var imageDiv = divo.SelectSingleNode(".//div[contains(@class, 'bg-cover')]");
                                            string? imageUrl = imageDiv?.GetAttributeValue("style", "");

                                            /*
                                            if (!string.IsNullOrEmpty(imageUrl))
                                            {
                                                var startIndex = imageUrl.IndexOf("url(") + 4;
                                                var endIndex = imageUrl.IndexOf(")", startIndex);
                                                imageUrl = imageUrl.Substring(startIndex, endIndex - startIndex);
                                            }
                                            else
                                            {
                                                imageUrl = GetBackupImageUrl();
                                            }
                                            */
                                            imageUrl = GetDoorDashImageUrl(); 

                                            // Get title
                                            string? title = divo.SelectSingleNode(".//h3")?.InnerText.Trim();

                                            // Get summary
                                            string? summary = divo.SelectSingleNode(".//div[contains(@class, 'text-sm')]")?.InnerText.Trim();



                                            // Get author
                                            string author = divo.SelectSingleNode(".//span[contains(@class, 'author')]")?.InnerText.Trim() ?? "DoorDash Blog";



                                            var Blogo = new BlogStat(link, "By " + author + "\n" + summary, title, imageUrl);


                                            Blog.Add(Blogo);
                                        }



                                    }
                                }
                                break;
                            }

                        case 11:
                            {

                                // hj
                                var products = htmlDocument.DocumentNode.SelectNodes("//li");
                                Console.WriteLine($"The Div {products.Count} \n");

                                List<string> ImgList = new List<string>()
                        { "https://miro.medium.com/max/1400/1*n4_NMnZmIhJ9Q9r4KDw1ew.png",
                            "https://miro.medium.com/max/602/1*bO6lRwKN8TlPhEbxNTHhAA.png",
                            "https://blog.eduonix.com/wp-content/uploads/2018/09/Scientific-Python-Scipy.jpg",
                            "https://iitb.emeritus.org/iitb-certificate-program-in-machine-learning-and-ai-with-python/images/main-banner.jpg",
                            "https://d6vdma9166ldh.cloudfront.net/media/images/265455c1-9741-4eb2-bc2f-b287a87c4eb9.jpg",
                            "https://bs-uploads.toptal.io/blackfish-uploads/components/blog_post_page/content/cover_image_file/cover_image/955519/retina_1708x683_REDESIGN-TensorFlow-Python-Tutorial-Luke_Newsletter-b8b1c21533773f01e1ca133a12eab772.png"
                        };

                                //   var httpClient = new HttpClient();
                                //   var html = await httpClient.GetStringAsync(Url);
                                //   var htmlDocument = new HtmlDocument();

                                var PickRandy = RandomInt(products.Count);

                                var divo = products[PickRandy];

                                // foreach (var divo in products)
                                {
                                    Random randy = new Random();
                                    int randum = randy.Next(ImgList.Count);
                                    var ImgTest = ImgList[randum];

                                    var NewHtml = divo.SelectSingleNode(".//a").ChildAttributes("href").FirstOrDefault()?.Value;
                                    var NewText = divo.SelectSingleNode(".//a").InnerText;


                                    html = await httpClient.GetStringAsync(NewHtml);
                                    var htmlDocu = new HtmlDocument();
                                    // kl
                                    htmlDocu.LoadHtml(html);
                                    var Prod = htmlDocu.DocumentNode.SelectSingleNode("//div[@class='entry-content']/p");

                                    var Mvo = Prod.InnerText;
                                    var Autho = htmlDocu.DocumentNode.SelectSingleNode("//a[@class='ct-meta-element-author']/span").InnerText;

                                    var Sumo = "Article by  " + Autho + "\n" + Mvo;

                                    var Blogo = new BlogStat(NewHtml, Sumo, NewText, ImgTest);


                                    Blog.Add(Blogo);
                                }
                                //for the Madchine Learning
                                break;
                            }
                    }


                    //cant remeber what this part was
                    int BlogPick = 0;

                    if (Blog != null && Blog.Count > 1)
                    {
                        Random randy = new Random();

                        BlogPick = randy.Next(0, Blog.Count);
                    }


                    TheResult[0] = "Success"; TheResult[1] = Blog[BlogPick].Title; TheResult[2] = Blog[BlogPick].Summary;
                    TheResult[3] = Blog[BlogPick].Link; TheResult[4] = Blog[BlogPick].Img;

                    TheResult[5] = $"_ Was {NewPick}, BlogPicker {BlogPicker} and the day was {DoW}";

                    return TheResult;
                }
                catch (HttpRequestException e)
                {

                    TheResult[0] = "Failure";
                    TheResult[1] = "We got an issue";
                    TheResult[2] = "Fix this BlogPicker " + BlogPicker;
                    TheResult[3] = $":D  {e.Message}";
                    TheResult[4] = "No Image";
                    TheResult[5] = $"-1 {NewPick} the day {DoW}";
                    return TheResult;
                }
            }
        }
    }

    public class BlogStat
    {
        //the link of the blog to post on Linkedin
        public string Link { get; set; }
        //the summary to post on linkedin
        public string Summary { get; set; }
        //the title to post on Linkedin
        public string Title { get; set; }
        //the image link to post on Linekdin
        public string Img { get; set; }

        public BlogStat(string link, string summary, string title, string img)
        {
            Link = link;
            Summary = summary;
            Title = title;
            Img = img;
        }
    }
}
