using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Bot.ChuckNorris.DataAccess;

namespace Bot.ChuckNorris.BusinessServices
{
    public class ChuckNorrisService : IChuckNorrisService
    {
        private readonly IChuckNorrisRepository _chuckNorrisRepository;

        public ChuckNorrisService(IChuckNorrisRepository chuckNorrisRepository)
        {
            _chuckNorrisRepository = chuckNorrisRepository;
        }

        public void Create(ChuckNorrisDto fact)
        {
            _chuckNorrisRepository.Create(new ChuckNorrisModel()
            {
                Id = fact.Id,
                FactDescription = WebUtility.HtmlDecode(fact.FactDescription),
                CleanText = CleanSentence(fact.FactDescription),
                FactDate = fact.FactDate,
                Vote = fact.Vote,
                Points = fact.Points
            });
        }

        public void Bulk(ICollection<ChuckNorrisDto> facts)
        {
            var bulkInsert = new Collection<ChuckNorrisModel>();
            foreach (var item in facts)
            {
                bulkInsert.Add(new ChuckNorrisModel()
                {
                    Id = item.Id,
                    FactDescription = WebUtility.HtmlDecode(item.FactDescription),
                    CleanText = CleanSentence(item.FactDescription),
                    FactDate = item.FactDate,
                    Vote = item.Vote,
                    Points = item.Points
                });
            }

            _chuckNorrisRepository.Bulk(bulkInsert);
        }

        public void Delete(int factId)
        {
            _chuckNorrisRepository.Delete(factId);
        }

        public ChuckNorrisDto GetFact(int factId)
        {
            var result = _chuckNorrisRepository.GetFact(factId);
            return new ChuckNorrisDto
            {
                Id = result.Id,
                FactDescription = result.FactDescription,
                CleanText = result.CleanText,
                FactDate = result.FactDate,
                Vote = result.Vote,
                Points = result.Points
            }; ;
        }

        public ICollection<ChuckNorrisDto> GetFacts()
        {
            var returnValue = _chuckNorrisRepository.GetFacts().Select(
                x => new ChuckNorrisDto
                {
                    Id = x.Id,
                    FactDescription = x.FactDescription,
                    CleanText = x.CleanText,
                    FactDate = x.FactDate,
                    Vote = x.Vote,
                    Points = x.Points
                }).ToList();

            return returnValue;
        }

        public bool IsExists(int factId)
        {
            return _chuckNorrisRepository.IsExists(factId);
        }

        public string FindBestFact(string text)
        {
            var startSentence = string.Empty;
            var facts = _chuckNorrisRepository.GetFacts();

            if (text.Contains("@"))
            {
                //Person
                var myRegEx = new Regex(@"(?:(?<=\s)|^)@(\w*[A-Za-z_]+\w*)");
                var namesMatches = myRegEx.Matches(text);
                var names = string.Join(", ", namesMatches.Cast<Match>().Select(m => m.Value));

                //Get Names
                startSentence = $"Hey {names} ! ";
            }

            var inputText = CleanSentence(text);
            var factSentence = string.Empty;

            //Find word
            var wordList = CleanSentence(inputText).Split(' ').OrderByDescending(t => t.Length);
            foreach (var word in wordList)
            {
                if (word.Trim().Length > 2)
                {
                    var searchResult = facts.Where(t => t.CleanText.Contains(word));
                    if (searchResult.ToList().Count > 0)
                    {
                        factSentence += GetRandomFact(searchResult.ToList());
                        break;
                    }
                }
            }

            if (factSentence == string.Empty)
            {
                factSentence += GetRandomFact(facts);
            }

            return string.Concat(startSentence, factSentence);
        }

        private static string GetRandomFact(ICollection<ChuckNorrisModel> facts)
        {
            if (facts.Count == 1)
                return facts.First().FactDescription;

            var rnd = new Random();
            var number = rnd.Next(0, facts.Count);

            return facts.Skip(number).First().FactDescription;
        }

        private static string CleanSentence(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var inputText = WebUtility.HtmlDecode(text).ToLower();
            inputText = inputText.Replace("chuck", string.Empty);

            var sb = new StringBuilder();
            foreach (var c in inputText)
            {
                if (c >= 'a' && c <= 'z')
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append(' ');
                }
            }
            return sb.ToString();
        }
    }
}