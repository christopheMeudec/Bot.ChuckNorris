using System.Collections.Generic;

namespace Bot.ChuckNorris.BusinessServices
{
    public interface IScraperInfoService
    {
        ICollection<ScraperInfoDto> GetScraperInfo();

        ScraperInfoDto GetScraperInfo(OrderEnum order);

        ScraperInfoDto AddUpdate(ScraperInfoDto scraperInfo);
    }
}
