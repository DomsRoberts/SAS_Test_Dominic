using System.Collections.Generic;

namespace ConstructionLine.CodingChallenge
{
    public interface IShirtLookup
    {
        IEnumerable<Shirt> Find(SearchOptions searchOptions);
    }
}