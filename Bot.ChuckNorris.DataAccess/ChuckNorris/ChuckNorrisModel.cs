namespace Bot.ChuckNorris.DataAccess
{
    public class ChuckNorrisModel
    {
        public int Id { get; set; }

        public string FactDescription { get; set; }

        public string CleanText { get; set; }

        public string FactDate { get; set; }

        public int Vote { get; set; }

        public int Points { get; set; }
    }
}