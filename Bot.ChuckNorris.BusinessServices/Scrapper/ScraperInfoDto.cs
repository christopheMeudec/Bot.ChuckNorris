using System;

namespace Bot.ChuckNorris.BusinessServices
{
    public class ScraperInfoDto
    {
        public OrderEnum Order { get; set; }

        public DateTime LastRun { get; set; }

        public int LastPage { get; set; }
    }

    public enum OrderEnum
    {
        last,
        first,
        top,
        flop,
        mtop,
        alea,
        mflop
    }
}
