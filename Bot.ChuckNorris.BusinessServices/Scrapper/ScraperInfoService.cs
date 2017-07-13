using System;
using System.Collections.Generic;
using System.Linq;
using Bot.ChuckNorris.DataAccess;

namespace Bot.ChuckNorris.BusinessServices
{
    public class ScraperInfoService : IScraperInfoService
    {
        private readonly IScraperInfoRepository _scraperInfoRepository;

        public ScraperInfoService(IScraperInfoRepository scraperInfoRepository)
        {
            _scraperInfoRepository = scraperInfoRepository;
        }

        public ScraperInfoDto AddUpdate(ScraperInfoDto scraperInfo)
        {
            var result = _scraperInfoRepository.AddUpdate(new ScraperInfoModel()
            {
                Order = Enum.GetName(typeof(OrderEnum), scraperInfo.Order),
                LastPage = scraperInfo.LastPage,
                LastRun = scraperInfo.LastRun
            });

            return new ScraperInfoDto()
            {
                Order = (OrderEnum)Enum.Parse(typeof(OrderEnum), result.Order),
                LastPage = result.LastPage,
                LastRun = result.LastRun
            };
        }

        public ScraperInfoDto GetScraperInfo(OrderEnum order)
        {
            var result = _scraperInfoRepository.GetScraperInfo(Enum.GetName(typeof(OrderEnum), order));
            if (result != null)
            {
                return new ScraperInfoDto
                {
                    Order = (OrderEnum)Enum.Parse(typeof(OrderEnum), result.Order),
                    LastPage = result.LastPage,
                    LastRun = result.LastRun
                };
            }
            else
            {
                return new ScraperInfoDto
                {
                    Order = order,
                    LastPage = 0,
                    LastRun = DateTime.Now
                };
            }
        }

        public ICollection<ScraperInfoDto> GetScraperInfo()
        {
            var returnValue = _scraperInfoRepository.GetScraperInfo().Select(
                x => new ScraperInfoDto
                {
                    Order = (OrderEnum)Enum.Parse(typeof(OrderEnum), x.Order),
                    LastPage = x.LastPage,
                    LastRun = x.LastRun
                }).ToList();

            return returnValue;
        }
    }
}
