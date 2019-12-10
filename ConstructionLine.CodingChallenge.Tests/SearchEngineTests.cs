using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test]
        public void Test()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions();
            searchOptions.Colors.AddRange(new List<Color> { Color.Red });
            searchOptions.Sizes.AddRange(new List<Size> { Size.Small });

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void ThatTheCorrectNumberOfAllColoursAreListed()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red)
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions();
            searchOptions.Colors.AddRange(new List<Color> { Color.Red });

            var result = searchEngine.Search(searchOptions);

            Assert.AreEqual(0, result.ColorCounts.FirstOrDefault(c => c.Color.Id.Equals(Color.Black.Id)).Count);
            Assert.AreEqual(0, result.ColorCounts.FirstOrDefault(c => c.Color.Id.Equals(Color.Blue.Id)).Count);
            Assert.AreEqual(3, result.ColorCounts.FirstOrDefault(c => c.Color.Id.Equals(Color.Red.Id)).Count);

            Assert.AreEqual(1, result.SizeCounts.FirstOrDefault(c => c.Size.Id.Equals(Size.Small.Id)).Count);
            Assert.AreEqual(2, result.SizeCounts.FirstOrDefault(c => c.Size.Id.Equals(Size.Medium.Id)).Count);
            Assert.AreEqual(0, result.SizeCounts.FirstOrDefault(c => c.Size.Id.Equals(Size.Large.Id)).Count);
        }

        [Test]
        public void ThatTheCorrectNumberOfAllSizesAreListed()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red)
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions();
            searchOptions.Sizes.Add(Size.Large);

            var result = searchEngine.Search(searchOptions);

            Assert.AreEqual(0, result.SizeCounts.FirstOrDefault(c => c.Size.Id.Equals(Size.Small.Id)).Count);
            Assert.AreEqual(0, result.SizeCounts.FirstOrDefault(c => c.Size.Id.Equals(Size.Medium.Id)).Count);
            Assert.AreEqual(1, result.SizeCounts.FirstOrDefault(c => c.Size.Id.Equals(Size.Large.Id)).Count);
        }
    }
}
