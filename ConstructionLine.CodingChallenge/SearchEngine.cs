using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly IReadOnlyList<Shirt> _shirts;

        private readonly ShirtLookupTable lookupTable;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = new ReadOnlyCollection<Shirt>(shirts);

            this.lookupTable = new ShirtLookupTable(_shirts);
        }


        public SearchResults Search(SearchOptions options)
        {
            var results = this.lookupTable.Find(options);
            var allShirts = results.ToArray();
            var sw = new Stopwatch();
            sw.Start();
            var sizes = allShirts.GroupBy(s => s.Size);
            var colors = allShirts.GroupBy(s => s.Color);
            sw.Stop();

            var allSizes = CreateSizeCounts(sizes);
            var allColors = CreateColorCounts(colors);

            return new SearchResults(allShirts, allSizes, allColors);
        }

        private IEnumerable<SizeCount> CreateSizeCounts(IEnumerable<IGrouping<IHaveAnId, Shirt>> sizes)
        {
            var sizeCounts = Size.All.Select(s => CreateSizeCount(s, sizes)).ToArray();

            return sizeCounts;
        }

        private SizeCount CreateSizeCount(Size size, IEnumerable<IGrouping<IHaveAnId, Shirt>> sizes)
        {
            var resultSet = sizes.FirstOrDefault(s => s.Key.Id.Equals(size.Id));

            return new SizeCount(size, resultSet?.Count() ?? 0);
        }

        private IEnumerable<ColorCount> CreateColorCounts(IEnumerable<IGrouping<IHaveAnId, Shirt>> colors)
        {
            var colorCounts = Color.All.Select(s => CreateColorCount(s, colors)).ToArray();

            return colorCounts;
        }

        private ColorCount CreateColorCount(Color color, IEnumerable<IGrouping<IHaveAnId, Shirt>> colors)
        {
            var resultSet = colors.FirstOrDefault(s => s.Key.Id.Equals(color.Id));

            return new ColorCount(color, resultSet?.Count() ?? 0);
        }
    }
}