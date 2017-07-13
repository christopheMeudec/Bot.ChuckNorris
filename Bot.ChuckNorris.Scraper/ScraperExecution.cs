using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Bot.ChuckNorris.BusinessServices;
using Newtonsoft.Json.Linq;

namespace Bot.ChuckNorris.Scraper
{
    public class ScraperExecution : IScraperExecution
    {
        private const OrderEnum order = OrderEnum.mtop;
        private const string url = "https://www.chucknorrisfacts.fr/api/get?data=tri:{0};nb:99;page:{1};type:txt";

        private readonly IScraperInfoService _scraperInfoService;
        private readonly IChuckNorrisService _chuckNorrisService;

        public ScraperExecution(IChuckNorrisService chuckNorrisService, IScraperInfoService scraperInfoService)
        {
            _scraperInfoService = scraperInfoService;
            _chuckNorrisService = chuckNorrisService;
        }

        public void Run()
        {
            WriteToConsole($"Start ScraperExecution - Current entries {_chuckNorrisService.GetFacts().Count}");
            var scraperInfo = _scraperInfoService.GetScraperInfo(order);

            var page = scraperInfo.LastPage;
            while (page < scraperInfo.LastPage + 15)
            {
                page++;
                var returnValue = GetJson(page);

                if (string.IsNullOrEmpty(returnValue))
                    break;

                ParseAndPopulateDB(returnValue);

                WriteToConsole($"ScraperExecution - Page {page}");
            }

            _scraperInfoService.AddUpdate(new ScraperInfoDto()
            {
                Order = order,
                LastPage = page,
                LastRun = DateTime.Now
            });

            WriteToConsole($"End ScraperExecution - Current entries {_chuckNorrisService.GetFacts().Count}");
        }

        private string GetJson(int page)
        {
            var jsonResult = string.Empty;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(string.Format(url, Enum.GetName(typeof(OrderEnum), order), page)).Result;
                if (response.IsSuccessStatusCode)
                {
                    jsonResult = response.Content.ReadAsStringAsync().Result;
                }
            }

            return jsonResult;
        }

        private void ParseAndPopulateDB(string json)
        {
            var o = JObject.Parse(string.Concat("{facts:", json, "}"));

            foreach (var item in (JArray)o["facts"])
            {
                if (!_chuckNorrisService.IsExists((int)item["id"]))
                {
                    _chuckNorrisService.Create(new ChuckNorrisDto()
                    {
                        Id = (int)item["id"],
                        FactDescription = (string)item["fact"],
                        FactDate = (string)item["date"],
                        Vote = (int)item["vote"],
                        Points = (int)item["points"]
                    });
                }
            }
        }

        private void WriteToConsole(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")} - {message}");
        }
    }
}