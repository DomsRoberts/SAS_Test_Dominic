using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchResults
    {
        private readonly Lazy<IReadOnlyCollection<Shirt>> shirts;

        private readonly Lazy<IReadOnlyCollection<SizeCount>> sizes;

        private readonly Lazy<IReadOnlyCollection<ColorCount>> colors;

        public SearchResults(IEnumerable<Shirt> shirts, IEnumerable<SizeCount> sizes, IEnumerable<ColorCount> colors)
        {
            this.shirts = new Lazy<IReadOnlyCollection<Shirt>>(() => shirts?.ToArray());
            this.sizes = new Lazy<IReadOnlyCollection<SizeCount>>(() => sizes?.ToArray());
            this.colors = new Lazy<IReadOnlyCollection<ColorCount>>(() => colors?.ToArray());
        }

        public IReadOnlyCollection<Shirt> Shirts => this.shirts.Value;


        public IReadOnlyCollection<SizeCount> SizeCounts => this.sizes.Value;


        public IReadOnlyCollection<ColorCount> ColorCounts => this.colors.Value;
    }


    public class SizeCount
    {
        public SizeCount(Size size, int count)
        {
            this.Size = size;
            this.Count = count;
        }

        public Size Size { get; }

        public int Count { get; }
    }


    public class ColorCount
    {
        public ColorCount(Color color, int count)
        {
            this.Color = color;
            this.Count = count;
        }

        public Color Color { get; }

        public int Count { get; }
    }
}