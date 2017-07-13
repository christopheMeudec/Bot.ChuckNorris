using System.Collections.Generic;

namespace Bot.ChuckNorris.DataAccess
{
    public interface IScraperInfoRepository
    {
        ICollection<ScraperInfoModel> GetScraperInfo();

        ScraperInfoModel GetScraperInfo(string order);

        ScraperInfoModel AddUpdate(ScraperInfoModel scraperInfo);

        bool IsExists(string order);
    }
}
