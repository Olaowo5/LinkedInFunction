using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Collections.Generic;

/// <summary>
/// CralwerApp Built on visual studio
/// An Azure function to get a request from Azure logic Apps
/// Webcrawl from selected blog sites,
/// return with blog link, an image, a selected text for summary and title
/// </summary>
namespace CrawlerApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string RequestBody = await new StreamReader(req.Body).ReadToEndAsync();

            string[] TheResult = await CrawlBlog(RandomInt(11),RequestBody);

            string FstResult = TheResult[0]; string SndResult = TheResult[1];
            string TrdResult = TheResult[2]; string FthResult = TheResult[3];
            string FifthResult = TheResult[4];
                string SixthResult = TheResult[5];

            return (ActionResult)new OkObjectResult
                (
                new
                {
                    FstResult, SndResult, TrdResult, FthResult, FifthResult,
                    SixthResult
                }
                );

        }

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

            if(DoW > 0)
            {
                //Now will be using the day of the week to edit
                //if value is greater zero
                //will overide the random number pass in the function
                //i.e blogpicker will be overwrriten if a case is valid
                 NewPick = RandomInt(3);
               switch(DoW)
                {


                    case 1:
                        {
                            if(NewPick == 0)
                            {
                                BlogPicker = 0; //serverless
                            }

                            else if(NewPick == 1)
                            {
                                BlogPicker = 2; //azure
                            }
                            else
                            {
                                BlogPicker = 11; //pyhton Machine
                            }

                            break;
                        }

                    case 2:
                        {
                            if (NewPick == 0)
                            {
                                BlogPicker = 5; //technix
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 5; //meduim
                            }
                            else
                            {
                                BlogPicker = 4; //unreal
                            }
                            break;
                        }

                    case 3:
                        {

                            if (NewPick == 0)
                            {
                                BlogPicker = 3; //Unity
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 8; //parveen
                            }
                            else
                            {
                                BlogPicker = 11; //pyhton Machine
                            }
                            break;
                        }

                    case 4:
                        {
                            if (NewPick == 0)
                            {
                                BlogPicker = 0; //serverless
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 3; //unity
                            }
                            else
                            {
                                BlogPicker = 4; //unreal
                            }
                            break;
                        }

                    case 5:
                        {
                            if (NewPick == 0)
                            {
                                BlogPicker = 0; //serverless
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 5; //technix
                            }
                            else
                            {
                                BlogPicker = 9; //iximuiz
                            }
                            break;
                        }

                    case 6:
                        {
                            if (NewPick == 0)
                            {
                                BlogPicker = 0; //serverless
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 4; //unreal
                            }
                            else
                            {
                                BlogPicker = 11; //pyhton Machine
                            }
                            break;
                        }

                    case 7:
                        {
                            if (NewPick == 0)
                            {
                                BlogPicker = 5; //meduim
                            }

                            else if (NewPick == 1)
                            {
                                BlogPicker = 2; //azure
                            }
                            else
                            {
                                BlogPicker = 11; //pyhton Machine
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
                if (BlogPicker == 0)
                {
                    Url = "https://www.serverless360.com/blog";

                }
                else if (BlogPicker == 1)
                {
                    Url = "https://techgenix.com/cloud-computing/microsoft-azure/";
                }
                else if (BlogPicker == 2)
                {
                    Url = "https://azure.microsoft.com/en-in/blog/?utm_source=devglan";
                }
                else if (BlogPicker == 3)
                {
                    Url = "https://blog.unity.com/";
                }
                else if (BlogPicker == 4)
                {
                    Url = "https://www.unrealengine.com/en-US/feed/blog"; // "https://www.sololearn.com/blog";
                }
                else if (BlogPicker == 5)
                {
                    Url = "https://medium.com/better-programming";
                }
                else if (BlogPicker == 6)
                {
                    Url = "https://reliance.systems/news-update/";
                }
                else if (BlogPicker == 7)
                {
                    Url = "https://www.upgrad.com/blog/full-stack-development/";
                }
                else if (BlogPicker == 8)
                {
                    Url = "https://parveensingh.com/";
                    Console.WriteLine("Hacker rank");
                }
                else if (BlogPicker == 9)
                {
                    Url = "https://iximiuz.com/en/";

                }
                else if (BlogPicker == 10)
                {
                    //not using this
                    Url = "https://www.thorsten-hans.com/";
                }
                else if(BlogPicker == 11)
                {
                    //okay Machine Learning
                    Url = "https://medium.com/coders-camp/230-machine-learning-projects-with-python-5d0c7abf8265";
                }
                else
                {
                    ItsFailed = true;
                }

                if(BlogPicker == 10 || BlogPicker == 6 || BlogPicker == 7 || BlogPicker == 12)
                {
                   // Running = true;
                   //to prevent this blogs from being picked
                    BlogPicker = RandomInt(12); //get a new picker
                }
                else if(ItsFailed)
                {
                    Running = false;
                }
                else
                {
                    Running = false;
                }

            } while (Running);



            if(ItsFailed)
            {
                TheResult[0] = "Failure";
                TheResult[1] = "We got an issue";
                TheResult[2] = "Fix this BlogPicker " + BlogPicker;
                TheResult[3] = ":D";
                TheResult[4] = "No Image";
                TheResult[5] = "-1";
                return TheResult;
            }


            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(Url);
            var htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(html);

            //Will be used to remove By from the autho name in Unity
            string RemoveBy(string str)
            {
                if (str.Contains("By"))
                {
                    String tempWord = "By";
                    str = str.Replace(tempWord, "");
                }

                return str;
            }

            //a list to add all availabel blogs we found
            var Blog = new List<BlogStat>();

            switch (BlogPicker)
            {
                //the following cases will be used to web crawl the sites picked
                //
                case 0:
                    {
                        //   var divs =
                        //     htmlDocument.DocumentNode.Descendants("div")
                        //      .Where(node => node.GetAttributeValue("class", "").Equals("grid-desc")).ToList();

                        //    Console.WriteLine($"The Div {divs.Count} \n");

                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='grid-desc']");
                        Console.WriteLine($"The Div {products.Count} \n");

                        List<string> ImgList = new List<string>()
                        { "https://www.serverless360.com/wp-content/uploads/2022/04/MicrosoftTeams-image-228.png",
                            "https://www.serverless360.com/wp-content/uploads/2019/03/serverless360-blog-01.png",
                            "https://www.serverless360.com/wp-content/uploads/2022/08/MicrosoftTeams-image-308.png",
                            "https://pbs.twimg.com/media/FaQ_otFUIAARXVc.jpg",
                            "https://external-preview.redd.it/8O12XMlg6pH_-Nw3lva_6KckGqHE0MouWMO3jp5-Yoo.jpg?width=640&crop=smart&auto=webp&s=16fc1bb1b9020ee36d7841c544da2796d0f54666"};


                        foreach (var divo in products)
                        {
                            Random randy = new Random();
                            int randum = randy.Next(ImgList.Count);
                            var ImgTest = ImgList[randum];

                            var Autho = divo.SelectNodes(".//p[@class='text-small']/span")[1].InnerText;
                            var Blogo = new BlogStat
                            {
                                Summary = "Article by  " + Autho + "\n" + divo.SelectSingleNode(".//p[@class='pb-3']").InnerText,
                                Title = divo.SelectSingleNode(".//h4/a").ChildAttributes("title").FirstOrDefault().Value,
                                Link = divo.SelectSingleNode(".//h4/a").ChildAttributes("href").FirstOrDefault().Value,
                                //tried to get from img src but woulkd return in base 64
                                Img = ImgTest
                            };
                            Blog.Add(Blogo);
                        }

                        /*
                        foreach (var divo in divs)
                        {
                            var Blogo = new BlogStat
                            {
                               // Summary = divo.Descendants("p").LastOrDefault().InnerText,
                               //Summary =divo.SelectNodes
                                Title = divo.Descendants("a").FirstOrDefault().ChildAttributes("title").FirstOrDefault().Value,

                                Link = divo.Descendants("a").FirstOrDefault().ChildAttributes("href").FirstOrDefault().Value,
                            };

                            Blog.Add(Blogo);
                        }
                        */

                        break;
                    }
                case 1:
                    {
                        //something wrng with 1 think they added a security layer
                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='post-content ast-grid-common-col']");
                        Console.WriteLine($"The Div {products.Count} \n");

                        foreach (var divo in products)
                        {

                            var Autho = divo.SelectSingleNode(".//span[@class='author-name']").InnerText;
                            var Blogo = new BlogStat
                            {
                                Summary = "Article by: " + Autho + "\n" + divo.SelectSingleNode(".//div[@class='entry-content clear']/p").InnerText,
                                Title = divo.SelectSingleNode(".//h2/a").InnerText,
                                Link = divo.SelectSingleNode(".//h2/a").ChildAttributes("href").FirstOrDefault().Value,
                                Img = divo.SelectSingleNode(".//div[@class='post-thumb-img-content post-thumb']/a/img").ChildAttributes("src").FirstOrDefault().Value,
                            };
                            Blog.Add(Blogo);
                        }


                        break;
                    }
                case 2:
                    {

                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='row column blog-posts']/article");
                        Console.WriteLine($"The Div {products.Count} \n");

                        List<string> ImgList = new List<string>()
                        { "https://daxg39y63pxwu.cloudfront.net/images/blog/microsoft-azure-projects-ideas-for-beginners-for-learning/Microsoft_Azure_Projects.png",
                            "https://techcommunity.microsoft.com/t5/image/serverpage/image-id/382451iDE25F4EFC4A8DCC2/image-dimensions/665x374?v=v2",
                            "https://ccbtechnology.com/wp-content/uploads/2019/03/Blog-MS-Azure-Explained.jpg",
                            "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTWeNSZhuwZiRPcf0mHV_jO1k3lFMoI2G0EsA&usqp=CAU",
                            "https://community.connection.com/wp-content/uploads/2020/05/1095542-Benefits-of-Azure-Liz-Alton-BLOG.png"};

                        foreach (var divo in products)
                        {

                            Random randy = new Random();
                            int randum = randy.Next(ImgList.Count);
                            var ImgTest = ImgList[randum];

                            var SummaryTest = divo.SelectNodes("p").Last().InnerText;
                            var LinkTest = "https://azure.microsoft.com" + divo.SelectSingleNode("h2/a").ChildAttributes("href").FirstOrDefault().Value;

                            var TitleTest = divo.SelectSingleNode("h2/a").ChildAttributes("title").FirstOrDefault().Value;

                            var Autho = divo.SelectSingleNode(".//div[@class='name with-position']/a").InnerText;
                            var Blogo = new BlogStat
                            {


                                // contine here

                                Summary = "Article By " + Autho + "\n" + SummaryTest,
                                Title = TitleTest,
                                Link = LinkTest,
                                Img = ImgTest
                            };
                            Blog.Add(Blogo);
                        }

                        break;
                    }
                case 3:
                    {
                        //w-full min-h-full flex flex-col
                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='border-b border-cheap pb-4 w-full min-h-full flex flex-col']");   //("//div[@class='w-full min-h-full flex flex-col']");
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


                            var Autho = divo.SelectSingleNode(".//span[@class='inline-flex items-center']").InnerText;
                            Autho = RemoveBy(Autho);
                            var Title = divo.SelectSingleNode(".//p[@class='b-line-clamp-3']").InnerText;
                            var Link = "https://blog.unity.com" + divo.SelectSingleNode(".//div[@class='relative my-3']/a").ChildAttributes("href").FirstOrDefault().Value;
                            var imgbridge = divo.SelectSingleNode(".//div[@class='relative my-3']/a");

                            var ImgTest = imgbridge.SelectNodes(".//img").ElementAtOrDefault(1).ChildAttributes("srcset").ElementAtOrDefault(0).Value; //ImgList[randum];

                            var Blogo = new BlogStat
                            {
                                Summary = "Article By " + Autho + "\n" + "Unity news and features from Unity Blog",//divo.SelectSingleNode(".//article/p").InnerText,
                                Title = Title,
                                Link = Link,
                                //Img = divo.SelectSingleNode(".//a[@class='component-featured-post__link']//img").ChildAttributes("src").FirstOrDefault().Value
                                Img = ImgTest

                            };
                            Blog.Add(Blogo);
                        }

                        break;
                    }
                case 4:
                    {
                       
                        
                        // FeedCardcontainer__FeedCardWrapper - sc - kdxk6b - 0 eQiFpl
                        //Changing to unreal



                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='SmallFeedCard__SmallFeedCardWrapper-sc-6x1tbj-0 fOBZeL feed-item']");
                        var prodimg = htmlDocument.DocumentNode.SelectNodes("//div[@class='image-wrapper']");
                        //Console.WriteLine($"The Div {products.Count}  {prodimg.Count} \n");
                       
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


                            var Blogo = new BlogStat
                            {
                                Summary = divo.LastChild.InnerText,//divo.SelectSingleNode(".//span[@class='SimpleLink__SimpleLinkWrap-sc-f8kw59-0 bOtKnb simple-link-wrap']//*[self::div or self::p]").InnerText,
                                Title = "Unreal Engine Announcements", //divo.FirstChild.InnerText.Trim('\r', '\n', '\t'),//"title",//divo.SelectSingleNode(".//a[@class='simple']/div").InnerText,
                                Link = "https://www.unrealengine.com" + LinkUrl,


                                Img = src//NewImg//ImgSrc

                            };
                            Blog.Add(Blogo);



                        }

                        break;
                    }
                case 5:
                    {
                        //mv mw mx l

                        var products = htmlDocument.DocumentNode.SelectNodes("//article");  //l eo gs
                                                                                            // var productsimg = htmlDocument.DocumentNode.SelectNodes("//div[@class='or os ot ou ov l']");
                      

                        // Console.WriteLine($"The Div for imgs  {productsimg.Count} \n");
                        int Counter = 0;
                        foreach (var divo in products)
                        {
                            // if (divo != null )
                            {

                                var Autho = divo.SelectSingleNode(".//div[@class='ab q']/p").InnerText; //author

                                var LinkTest = "https://medium.com" + divo.SelectSingleNode(".//div[@class='l']/a[@class='ae af ag ah ai aj ak al am an ao ap aq ar as']").ChildAttributes("href").FirstOrDefault().Value;
                                LinkTest = LinkTest.Substring(0, LinkTest.IndexOf("?"));

                                var TitleTest = divo.SelectSingleNode(".//div[@class='me mf mg mh mi l']/h2").InnerText;
                                var ImgTest = divo.SelectSingleNode(".//div[@class='ab']/div[@class='on oo op oq or l']").InnerHtml;

                                string src = ImgTest.Substring(ImgTest.IndexOf("src=") + 5);
                                src = src.Substring(0, src.IndexOf("\""));
                                var SummaryTest = "Article By  " + Autho + "\n" + " " + divo.SelectSingleNode(".//div[@class='h k nb cx db']").InnerText;

                                var Blogo = new BlogStat
                                {
                                    Summary = SummaryTest,

                                    Title = TitleTest,
                                    Link = LinkTest,
                                    Img = src// ImgTest
                                };
                                Blog.Add(Blogo);

                                Counter++;
                            }
                        }
                        break;
                    }
                case 6:
                    {
                        //elementor-posts-container elementor-posts elementor-posts--skin-cards elementor-grid elementor-has-item-ratio

                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='elementor-element elementor-element-67d1b5b elementor-grid-3 elementor-grid-tablet-2 elementor-grid-mobile-1 elementor-posts--thumbnail-top elementor-posts--show-avatar elementor-card-shadow-yes elementor-posts__hover-gradient elementor-widget elementor-widget-posts']/div/div/article");
                        Console.WriteLine($"The Div {products.Count} \n");

                        foreach (var divo in products)
                        {
                            var Imgo = "";

                            if (divo.SelectSingleNode(".//div[@class='elementor-post__thumbnail']/img") != null)
                                Imgo = "https://reliance.systems" + divo.SelectSingleNode(".//div[@class='elementor-post__thumbnail']/img").ChildAttributes("src").FirstOrDefault().Value;

                            //cause not all have pictures
                            if (String.IsNullOrEmpty(Imgo))
                            {
                                Imgo = " https://media-exp1.licdn.com/dms/image/C4D1BAQHjTtdcsqrY2A/company-background_10000/0/1616869631036?e=2147483647&v=beta&t=B1OVrJYyt94u7Dr9u6uvCUa1AFIK5vnviQJqB_bn15Q";
                            }

                            var Blogo = new BlogStat
                            {
                                Summary = divo.SelectSingleNode(".//div/p").InnerText,
                                Title = divo.SelectSingleNode(".//h3/a").InnerText,
                                Link = divo.SelectSingleNode(".//div/a").ChildAttributes("href").FirstOrDefault().Value,
                                Img = Imgo
                            };
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

                                var Blogo = new BlogStat
                                {
                                    Summary = SummaryTest,

                                    Title = TitleTest,
                                    Link = LinkTest,
                                    Img = ImgTest
                                };
                                Blog.Add(Blogo);

                                counter++;
                            }
                        }
                        break;
                    }

                case 8:
                    {

                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='post-feed expanded container medium']/article");  //l eo gs

                        Console.WriteLine($"The Div {products.Count} \n");



                        foreach (var divo in products)
                        {
                            if (divo != null)
                            {
                                var LinkTest = "https://parveensingh.com/" + divo.SelectSingleNode("a").ChildAttributes("href").FirstOrDefault().Value;
                                var TitleTest = divo.SelectSingleNode(".//h2[@class='feed-title']").InnerText;

                                TitleTest = TitleTest.Replace("\n", "").Replace("\r", ""); //remove white space
                                var ImgTest = "https://parveensingh.com/" + divo.SelectSingleNode(".//div[@class='feed-image u-placeholder rectangle']/img").GetAttributeValue("src", "nothing");

                                var SummaryText = divo.SelectSingleNode(".//div[@class='feed-excerpt']").InnerText;
                                SummaryText = SummaryText.Replace("\n", ""); //remove white space
                                var Blogo = new BlogStat
                                {
                                    Summary = SummaryText,

                                    Title = TitleTest,
                                    Link = LinkTest,
                                    Img = ImgTest
                                };
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


                                var Blogo = new BlogStat
                                {
                                    Summary = "Learning Containers, Kubernetes, and Backend Development with Ivan Velichko",

                                    Title = TitleTest,
                                    Link = LinkTest,
                                    Img = ImgTest
                                };
                                Blog.Add(Blogo);


                            }
                        }
                        break;
                    }

                case 10:
                    {

                        var products = htmlDocument.DocumentNode.SelectNodes("//li[@class='post-stub']");  //l eo gs

                        Console.WriteLine($"The Div {products.Count} \n");

                        List<string> ImgList = new List<string>()
                        { "https://radiocrafts.com/wp-content/uploads/2019/12/cloud-services.png",
                            "https://community.connection.com/wp-content/uploads/2020/05/1095542-Benefits-of-Azure-Liz-Alton-BLOG.png",
                            "https://itsfoss.com/wp-content/uploads/2016/11/cloud-centric-Linux-distributions.jpg",
                            "https://www.devteam.space/wp-content/uploads/2022/04/What-is-Cloud-Development.png" };


                        foreach (var divo in products)
                        {
                            if (divo != null)
                            {
                                var LinkTest = "https://www.thorsten-hans.com" + divo.SelectSingleNode("a").ChildAttributes("href").FirstOrDefault().Value;
                                var TitleTest = divo.SelectSingleNode("a/h2").InnerText;
                                var SummryTest = divo.SelectSingleNode(".//p[@class='post-stub-description']").InnerText;
                                TitleTest = TitleTest.Replace("\n", "").Replace("\r", ""); //remove white space

                                Random randy = new Random();
                                int randum = randy.Next(ImgList.Count);
                                var ImgTest = ImgList[randum];


                                var Blogo = new BlogStat
                                {
                                    Summary = SummryTest,

                                    Title = TitleTest,
                                    Link = LinkTest,
                                    Img = ImgTest
                                };
                                Blog.Add(Blogo);


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

                            var NewHtml = divo.SelectSingleNode(".//a").ChildAttributes("href").FirstOrDefault().Value;
                            var NewText = divo.SelectSingleNode(".//a").InnerText;


                            html = await httpClient.GetStringAsync(NewHtml);
                            var htmlDocu = new HtmlDocument();
                            // kl
                            htmlDocu.LoadHtml(html);
                            var Prod = htmlDocu.DocumentNode.SelectSingleNode("//div[@class='entry-content']/p");

                            var Mvo = Prod.InnerText;

                            var Autho = htmlDocu.DocumentNode.SelectSingleNode("//a[@class='ct-meta-element-author']/span").InnerText;

                            var Blogo = new BlogStat
                            {
                                Summary = "Article by  " + Autho + "\n" + Mvo,
                                Title = NewText,
                                Link = NewHtml,
                                //tried to get from img src but woulkd return in base 64
                                Img = ImgTest
                            };
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

            TheResult[5] += " the pick was "+ NewPick;

            return TheResult;
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
    }


}
