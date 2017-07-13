using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using NFluent;
using NUnit.Framework;
using Bot.ChuckNorris.DataAccess;
using Bot.ChuckNorris.BusinessServices;

namespace Bot.ChuckNorris.Tests
{
    [TestFixture]
    public class ServicesTests
    {
        [Test]
        public void ShouldReturnListWhenCallGetFactsMethod()
        {
            //Arange
            var moqFactRepo = new Mock<IChuckNorrisRepository>();
            moqFactRepo.Setup(t => t.GetFacts()).Returns(GetFakeFactList());
            var factService = new ChuckNorrisService(moqFactRepo.Object);

            //Act
            var facts = factService.GetFacts();

            //Assert
            Check.That(facts).Not.IsNullOrEmpty();
        }

        [Test]
        public void ShouldReturnFactWhenCallGetFactMethod()
        {
            //Arange
            var moqFactRepo = new Mock<IChuckNorrisRepository>();
            moqFactRepo.Setup(t => t.GetFact(1)).Returns(GetFakeFact());
            var factService = new ChuckNorrisService(moqFactRepo.Object);

            //Act
            var fact = factService.GetFact(1);

            //Assert
            Check.That(fact).IsNotNull();
        }

        [Test]
        public void ShouldntThrowWhenCallCreateMethod()
        {
            //Arange
            var moqFactRepo = new Mock<IChuckNorrisRepository>();
            moqFactRepo.Setup(x => x.Create(It.IsAny<ChuckNorrisModel>()));

            var factService = new ChuckNorrisService(moqFactRepo.Object);

            Assert.DoesNotThrow(() =>
                factService.Create(new ChuckNorrisDto()
                {
                    Id = 1,
                    FactDate = "2017-06-01",
                    FactDescription = "hello world",
                    CleanText = "hello world",
                    Points = 12,
                    Vote = 56
                }));
        }

        [Test]
        public void ShouldntThrowWhenCallBulkMethod()
        {
            var moqFactRepo = new Mock<IChuckNorrisRepository>();
            moqFactRepo.Setup(x => x.Bulk(It.IsAny<ICollection<ChuckNorrisModel>>()));

            var factService = new ChuckNorrisService(moqFactRepo.Object);

            Assert.DoesNotThrow(() =>
                factService.Bulk(new Collection<ChuckNorrisDto>() {
                        new ChuckNorrisDto()
                        {
                            Id = 1,
                            FactDate = "2017-06-01",
                            FactDescription = "hello world",
                            CleanText = "hello world",
                            Points = 12,
                            Vote = 56
                        }
                }));
        }

        [Test]
        public void ShouldntThrowExceptionWhenCallDeleteMethod()
        {
            var moqFactRepo = new Mock<IChuckNorrisRepository>();
            moqFactRepo.Setup(t => t.Create(GetFakeFact())).Returns(GetFakeFact());
            var factService = new ChuckNorrisService(moqFactRepo.Object);

            Assert.DoesNotThrow(() =>
                factService.Delete(1));
        }

        private ChuckNorrisModel GetFakeFact()
        {
            return new ChuckNorrisModel()
            {
                Id = 1,
                FactDate = "2017-06-01",
                CleanText = "hello world",
                FactDescription = "hello world",
                Points = 12,
                Vote = 56
            };
        }

        private List<ChuckNorrisModel> GetFakeFactList()
        {
            return new List<ChuckNorrisModel>() {
                new ChuckNorrisModel()
                {
                    Id = 1,
                    FactDate = "2017-06-01",
                    FactDescription = "hello world",
                    Points = 12,
                    Vote = 56
                },
                new ChuckNorrisModel()
                {
                    Id = 2,
                    FactDate = "2017-06-01",
                    FactDescription = "hello world",
                    Points = 22,
                    Vote = 69
                }
            };
        }
    }
}

