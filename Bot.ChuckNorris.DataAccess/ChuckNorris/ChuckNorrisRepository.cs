using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Bot.ChuckNorris.DataAccess
{
    public class ChuckNorrisRepository : IChuckNorrisRepository
    {
        private ICollection<ChuckNorrisModel> _factsRepo = default(ICollection<ChuckNorrisModel>);
        private readonly string _jSonFile;

        public ChuckNorrisRepository(string path)
        {
            _jSonFile = Path.Combine(path, "facts.json");
            ReadJsonFile();
        }

        public ICollection<ChuckNorrisModel> GetFacts()
        {
            return _factsRepo;
        }

        public ChuckNorrisModel Create(ChuckNorrisModel fact)
        {
            _factsRepo.Add(fact);

            SaveJsonFile();

            return fact;
        }

        public ICollection<ChuckNorrisModel> Bulk(ICollection<ChuckNorrisModel> facts)
        {
            foreach (var item in facts)
            {
                if (_factsRepo.FirstOrDefault(t => t.Id == item.Id) == null)
                    _factsRepo.Add(item);
            }

            SaveJsonFile();

            return facts;
        }

        public ChuckNorrisModel GetFact(int factId)
        {
            return _factsRepo.Single(t => t.Id == factId);
        }

        public bool IsExists(int factId)
        {
            return _factsRepo.Any(t => t.Id == factId);
        }

        private void ReadJsonFile()
        {
            if (!File.Exists(_jSonFile))
            {
                _factsRepo = new Collection<ChuckNorrisModel>();
                return;
            }

            using (var file = File.OpenText(_jSonFile))
            {
                var serializer = new JsonSerializer();
                _factsRepo = (ICollection<ChuckNorrisModel>)serializer.Deserialize(file, typeof(ICollection<ChuckNorrisModel>));
            }
        }

        private void SaveJsonFile()
        {
            using (var file = File.CreateText(_jSonFile))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, _factsRepo);
            }
        }

        public void Delete(int factId)
        {
            throw new NotImplementedException();
        }

    }
}