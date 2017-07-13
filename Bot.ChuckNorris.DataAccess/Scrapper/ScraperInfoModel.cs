using System;

namespace Bot.ChuckNorris.DataAccess
{
  public  class ScraperInfoModel
    {
        public string Order { get; set; }

        public DateTime LastRun { get; set; }

        public int LastPage { get; set; }
    }
}
