using Microsoft.AspNetCore.Mvc;
using Rotten_tomatoes.Models;
using System.Diagnostics;
using Rotten_tomatoes.Data;
using HtmlAgilityPack;

namespace Rotten_tomatoes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult Tops()
        //{
        //    List<List<string>> tops_peliculas = scrape_data();
           

        //    ViewData["peliculas"] = tops_peliculas;
        //    return View();
        //}

        //List<List<string>> scrape_data()
        //{
        //    //Accediendo a url
        //    async Task<string> call_url(string fullUrl)
        //    {
        //        HttpClient client = new HttpClient();
        //        var response = await client.GetStringAsync(fullUrl);
        //        return response;
        //    }

        //    //parseando datos
        //    List<List<string>> parse_html(string html)
        //    {
        //        HtmlDocument htmlDoc = new HtmlDocument();
        //        htmlDoc.LoadHtml(html);

        //        //LINQ
        //        var parsed_data = htmlDoc.DocumentNode.Descendants("ul")
        //            .Where(node => node.GetAttributeValue("slot", "").Contains("list-items")).
        //            ToList();

        //        List<List<string>> top_data = new List<List<string>>();
        //        foreach (HtmlNode pelicula in parsed_data)
        //        {
        //            var titulos = pelicula.Descendants("li")
        //            .Select(node => node.Descendants("li").FirstOrDefault()?.InnerText)
        //            .ToList();


        //            top_data.Add(new List<string>() {titulos});
        //        }

        //        return top_data;
        //    }

        //    string url = "https://www.rottentomatoes.com/";
        //    var response = call_url(url).Result;
        //    List<List<string>> data = parse_html(response);
        //    return data;
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}