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

            string[] TheResult = await CrawlBlog(RandomInt(7),RequestBody);

            string FstResult = TheResult[0]; string SndResult = TheResult[1];
            string TrdResult = TheResult[2]; string FthResult = TheResult[3];
            string FifthResult = TheResult[4];

            return (ActionResult)new OkObjectResult
                (
                new
                {
                    FstResult, SndResult, TrdResult, FthResult, FifthResult
                }
                );

        }

        private static int RandomInt(int Mxn)
        {
            Random randy = new Random();

            int RandNum = randy.Next(0, 7);

            return RandNum;
        }

        private static async Task<string[]> CrawlBlog(int Picker, string req)
        {
            int BlogPicker = Picker;

            string[] TheResult = new string[5];

            TheResult[0] = req;

            //Get The url we want to go
            var Url = "";


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
            else
            {
                TheResult[0] = "Failure";
                TheResult[1] = "We got an issue";
                TheResult[2] = "Fix this";
                TheResult[3] = ":D";
                TheResult[4] = "No Image";
                return TheResult;
            }

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(Url);
            var htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(html);

            //a list to add all availabel blogs we found
            var Blog = new List<BlogStat>();

            switch (BlogPicker)
            {
                case 0:
                    {
                        //   var divs =
                        //     htmlDocument.DocumentNode.Descendants("div")
                        //      .Where(node => node.GetAttributeValue("class", "").Equals("grid-desc")).ToList();

                        //    Console.WriteLine($"The Div {divs.Count} \n");

                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='grid-desc']");
                       // Console.WriteLine($"The Div {products.Count} \n");

                        foreach (var divo in products)
                        {
                            var Blogo = new BlogStat
                            {
                                Summary = divo.SelectSingleNode(".//p[@class='pb-3']").InnerText,
                                Title = divo.SelectSingleNode(".//h4/a").ChildAttributes("title").FirstOrDefault().Value,
                                Link = divo.SelectSingleNode(".//h4/a").ChildAttributes("href").FirstOrDefault().Value,
                                //tried to get from img src but woulkd return in base 64
                                Img = "https://www.serverless360.com/wp-content/uploads/2022/04/MicrosoftTeams-image-228.png"
                            };
                            Blog.Add(Blogo);
                        }

                       

                        break;
                    }
                case 1:
                    {
                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='post-content ast-grid-common-col']");
                       // Console.WriteLine($"The Div {products.Count} \n");

                        foreach (var divo in products)
                        {
                            var Blogo = new BlogStat
                            {
                                Summary = divo.SelectSingleNode(".//div[@class='entry-content clear']/p").InnerText,
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

                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='column large-4']");
                        Console.WriteLine($"The Div {products.Count} \n");

                        foreach (var divo in products)
                        {
                            var Blogo = new BlogStat
                            {
                                Summary = "Microsoft Azure Blog",//divo.SelectSingleNode(".//article/p").InnerText,
                                Title = divo.SelectSingleNode(".//h3/a").ChildAttributes("title").FirstOrDefault().Value,
                                Link = "https://azure.microsoft.com" + divo.SelectSingleNode(".//h3/a").ChildAttributes("href").FirstOrDefault().Value,
                                Img = "https://daxg39y63pxwu.cloudfront.net/images/blog/microsoft-azure-projects-ideas-for-beginners-for-learning/Microsoft_Azure_Projects.png"
                            };
                            Blog.Add(Blogo);
                        }

                        break;
                    }
                case 3:
                    {
                        //w-full min-h-full flex flex-col
                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='w-full min-h-full flex flex-col']");
                        Console.WriteLine($"The Div {products.Count} \n");

                        foreach (var divo in products)
                        {
                            var Blogo = new BlogStat
                            {
                                Summary = "Unity news and features",//divo.SelectSingleNode(".//article/p").InnerText,
                                Title = divo.SelectSingleNode(".//a[@class='component-featured-post__link']/p").InnerText,
                                Link = "https://blog.unity.com" + divo.SelectSingleNode(".//a[@class='component-featured-post__link']").ChildAttributes("href").FirstOrDefault().Value,
                                Img = "https://blog-api.unity.com/sites/default/files/styles/focal_crop_ratio_3_1/public/2022-06/Blog%20header-1230x410%20%282%29.png?imwidth=1260&h=3643c662&itok=NPwXjRlB"
                            };
                            Blog.Add(Blogo);
                        }

                        break;
                    }
                case 4:
                    {
                      

                       // Console.WriteLine("got em");
                        // FeedCardcontainer__FeedCardWrapper - sc - kdxk6b - 0 eQiFpl
                        //Changing to unreal


                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='FeedCardcontainer__FeedCardWrapper-sc-kdxk6b-0 eQiFpl']");
                        Console.WriteLine($"The Div {products.Count} \n");

                        //---- This will be used to get a random Image
                        var ImgName = htmlDocument.DocumentNode.SelectNodes(".//div[@class='image-wrapper']");

                        string ImgSrc = "https://cdn2.unrealengine.com/m2m-02-1920x1080-313b6089e0e7.jpg";

                        List<string> ImgList = new List<string>();

                        if (ImgName != null)
                        {
                            foreach (var dimg in ImgName)
                            {

                                var Sup = dimg.SelectSingleNode(".//img").ChildAttributes("src").FirstOrDefault().Value;
                                if (Sup != null || !string.IsNullOrEmpty(Sup) || !string.IsNullOrWhiteSpace(Sup))
                                    ImgList.Add(Sup);
                            }
                        }


                        if (ImgList.Count > 1)
                        {
                            int randy = RandomInt(ImgList.Count);

                            ImgSrc = ImgList[randy];
                        }



                        var Splitted = ImgSrc.Split("?");
                        ImgSrc = Splitted[0];

                        //---- This will be used to get a random Image

                        foreach (var divo in products)
                        {
                            var Blogo = new BlogStat
                            {
                                Summary = divo.LastChild.InnerText,//divo.SelectSingleNode(".//span[@class='SimpleLink__SimpleLinkWrap-sc-f8kw59-0 bOtKnb simple-link-wrap']//*[self::div or self::p]").InnerText,
                                Title = "Unreal Engine", //divo.FirstChild.InnerText.Trim('\r', '\n', '\t'),//"title",//divo.SelectSingleNode(".//a[@class='simple']/div").InnerText,
                                Link = "https://www.unrealengine.com" + divo.SelectSingleNode(".//span[@class='SimpleLink__SimpleLinkWrap-sc-f8kw59-0 bOtKnb simple-link-wrap']/a").ChildAttributes("href").FirstOrDefault().Value,
                                Img = ImgSrc
                            };
                            Blog.Add(Blogo);



                        }

                        break;
                    }
                case 5:
                    {
                        //mv mw mx l

                        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='mv mw mx l']");
                        Console.WriteLine($"The Div {products.Count} \n");


                        foreach (var divo in products)
                        {
                            var Blogo = new BlogStat
                            {
                                Summary = "Better programming meduim",
                                Title = divo.SelectSingleNode(".//a[@class='au av aw ax ay az ba bb bc bd be bf bg bh bi']/div/h2").InnerText,
                                Link = "https://medium.com" + divo.SelectSingleNode(".//div[@class='l']/a[@class='au av aw ax ay az ba bb bc bd be bf bg bh bi']").ChildAttributes("href").FirstOrDefault().Value,
                                Img = divo.SelectSingleNode(".//div[@class='ah ai j i d']/img").ChildAttributes("src").FirstOrDefault().Value
                            };
                            Blog.Add(Blogo);



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
            }

            int BlogPick = 0;

            if (Blog != null && Blog.Count > 1)
            {
                Random randy = new Random();

                BlogPick = randy.Next(0, Blog.Count);
            }


            TheResult[0] = "Success"; TheResult[1] = Blog[BlogPick].Title; TheResult[2] = Blog[BlogPick].Summary;
            TheResult[3] = Blog[BlogPick].Link; TheResult[4] = Blog[BlogPick].Img;

            return TheResult;
        }
    }

    public class BlogStat
    {
        public string Link { get; set; }

        public string Summary { get; set; }

        public string Title { get; set; }

        public string Img { get; set; }
    }


}
