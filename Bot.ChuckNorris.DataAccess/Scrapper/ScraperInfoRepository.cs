using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Bot.ChuckNorris.DataAccess
{
    public class ScraperInfoRepository : IScraperInfoRepository
    {
        private ICollection<ScraperInfoModel> _scraperInfo = default(ICollection<ScraperInfoModel>);
        private readonly string _jSonFile;

        public ScraperInfoRepository(string path)
        {
            _jSonFile = string.Concat(path, "scraperInfo.json");
            ReadJsonFile();
        }

        public ScraperInfoModel AddUpdate(ScraperInfoModel value)
        {
            var res = _scraperInfo.FirstOrDefault(t => t.Order == value.Order);
            if (res == null)
            {
                _scraperInfo.Add(value);
            }
            else
            {
                res.LastPage = value.LastPage;
                res.LastRun = value.LastRun;
            }

            SaveJsonFile();

            return value;
        }

        public ScraperInfoModel GetScraperInfo(string order)
        {
            return _scraperInfo.FirstOrDefault(t => t.Order == order);
        }

        public ICollection<ScraperInfoModel> GetScraperInfo()
        {
            return _scraperInfo;
        }

        public bool IsExists(string order)
        {
            return _scraperInfo.Any(t => t.Order == order);
        }

        private void ReadJsonFile()
        {
            if (!File.Exists(_jSonFile))
            {
                _scraperInfo = new Collection<ScraperInfoModel>();
                return;
            }

            using (var file = File.OpenText(_jSonFile))
            {
                var serializer = new JsonSerializer();
                _scraperInfo = (ICollection<ScraperInfoModel>)serializer.Deserialize(file, typeof(ICollection<ScraperInfoModel>));
            }
        }

        private void SaveJsonFile()
        {
            using (var file = File.CreateText(_jSonFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, _scraperInfo);
            }
        }
    }
}
