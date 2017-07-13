using System.Collections.Generic;

namespace Bot.ChuckNorris.DataAccess
{
    public interface IChuckNorrisRepository
    {
        ICollection<ChuckNorrisModel> GetFacts();

        ChuckNorrisModel GetFact(int factId);

        ChuckNorrisModel Create(ChuckNorrisModel fact);

        ICollection<ChuckNorrisModel> Bulk(ICollection<ChuckNorrisModel> facts);

        void Delete(int factId);

        bool IsExists(int factId);
    }
}