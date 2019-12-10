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
            var sizes = allShirts.GroupBy(s => s.Size).Select(s => new SizeCount(s.Key, s.Count()));
            var colors = allShirts.GroupBy(s => s.Color).Select(s => new ColorCount(s.Key, s.Count()));
            sw.Stop();

            return new SearchResults(allShirts, sizes, colors);
        }
    }
}