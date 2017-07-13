using System.Collections.Generic;

namespace Bot.ChuckNorris.BusinessServices
{
    public interface IChuckNorrisService
    {
        ICollection<ChuckNorrisDto> GetFacts();

        ChuckNorrisDto GetFact(int factId);

        void Create(ChuckNorrisDto fact);

        void Bulk(ICollection<ChuckNorrisDto> facts);

        void Delete(int factId);

        bool IsExists(int factId);

        string FindBestFact(string text);

    }
}