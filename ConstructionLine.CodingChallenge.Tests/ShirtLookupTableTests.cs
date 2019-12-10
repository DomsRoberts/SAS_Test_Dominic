using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class ShirtLookupTableTests
    {
        [Test]
        public void ThatAShirtKeyWillBeTheSameForTheSameColorAndSize()
        {
            var key1 = TestShirtLookupTable.CreateShirtLookupKey(Color.Black, Size.Large);
            var key2 = TestShirtLookupTable.CreateShirtLookupKey(Color.Black, Size.Large);

            Assert.AreEqual(key1, key2);
        }

        [Test]
        public void ThatDifferentShirtSizeCreateDifferentKeys()
        {
            var key1 = TestShirtLookupTable.CreateShirtLookupKey(Color.Black, Size.Large);
            var key2 = TestShirtLookupTable.CreateShirtLookupKey(Color.Black, Size.Medium);

            Assert.AreNotEqual(key1, key2);
        }        

        [Test]
        public void ThatDifferentcolorsCreateDifferentKeys()
        {
            var key1 = TestShirtLookupTable.CreateShirtLookupKey(Color.Black, Size.Large);
            var key2 = TestShirtLookupTable.CreateShirtLookupKey(Color.Blue, Size.Large);

            Assert.AreNotEqual(key1, key2);
        }

        [Test]
        public void WhenTheSearchOptionsAreNullAnEmptyListIsReturned()
        {
            var shirts = new[]
            {
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
            };

            var lookupTable = new ShirtLookupTable(shirts);
            var result = lookupTable.Find(null);

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void ThatFindingAShirtByColorWillReturnAllShirtsOfTheCorrectColor()
        {
            var shirts = new[]
            {
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
            };

            var lookupTable = new ShirtLookupTable(shirts);

            var options = new SearchOptions();
            options.Colors.Add(Color.Black);

            var result = lookupTable.Find(options);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void ThatFindingBySizeWillFindTheCorrectNumberOfResults()
        {
            var shirts = new[]
            {
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Red),
            };

            var lookupTable = new ShirtLookupTable(shirts);

            var options = new SearchOptions();
            options.Sizes.Add(Size.Medium);

            var result = lookupTable.Find(options);

            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(r => r.Size == Size.Medium));
        }

        [Test]
        public void ThatSearchingForMultipleSizesWillReturnBothSizesCorrectly()
        {
            var shirts = new[]
            {
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Red),
            };

            var lookupTable = new ShirtLookupTable(shirts);

            var options = new SearchOptions();
            options.Sizes.Add(Size.Medium);
            options.Sizes.Add(Size.Small);

            var result = lookupTable.Find(options);

            Assert.AreEqual(4, result.Count());
            Assert.AreEqual(3, result.Count(r => r.Size == Size.Medium));
            Assert.AreEqual(1, result.Count(r => r.Size == Size.Small));
        }

        [Test]
        public void ThatSearchingForMultipleColoursWillReturnBothColorsCorrectly()
        {
            var shirts = new[]
            {
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Red),
            };

            var lookupTable = new ShirtLookupTable(shirts);

            var options = new SearchOptions();
            options.Colors.Add(Color.Blue);
            options.Colors.Add(Color.Red);

            var result = lookupTable.Find(options);

            Assert.AreEqual(4, result.Count());
            Assert.AreEqual(3, result.Count(r => r.Color == Color.Blue));
            Assert.AreEqual(1, result.Count(r => r.Color == Color.Red));
        }

        [Test]
        public void ThatFindingBySizeAndColorWillFindTheCorrectNumberOfResults()
        {
            var shirts = new[]
            {
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), string.Empty, Size.Medium, Color.Red),
            };

            var lookupTable = new ShirtLookupTable(shirts);

            var options = new SearchOptions();
            options.Sizes.Add(Size.Medium);
            options.Colors.Add(Color.Red);

            var result = lookupTable.Find(options);

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(r => r.Color == Color.Red));
        }

        private class TestShirtLookupTable : ShirtLookupTable
        {
            public TestShirtLookupTable(IEnumerable<Shirt> shirts) : base(shirts)
            {
            }

            public static string CreateShirtLookupKey(Color color, Size size)
            {
                var key = ShirtKey.Create(size, color);
                return key.Key;
            }
        }
    }
}
