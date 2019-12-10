using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class ShirtLookupTable : IShirtLookup
    {
        private readonly IReadOnlyDictionary<Guid, IEnumerable<ShirtKey>> colorLookupTable;

        private readonly IReadOnlyDictionary<Guid, IEnumerable<ShirtKey>> sizeLookupTable;

        private readonly IReadOnlyDictionary<ShirtKey, IEnumerable<Shirt>> shirtStore;

        public ShirtLookupTable(IEnumerable<Shirt> shirts)
        {
            if(shirts == null)
            {
                throw new ArgumentNullException(nameof(shirts));
            }

            this.shirtStore = CreateShirtLookupTable(shirts);
            this.colorLookupTable = CreateColourTable(this.shirtStore);
            this.sizeLookupTable = CreateSizeTable(this.shirtStore);
        }

        public IEnumerable<Shirt> Find(SearchOptions searchOptions)
        {
            if(searchOptions == null)
            {
                return Enumerable.Empty<Shirt>();
            }

            if(searchOptions.Colors.IsNullOrEmpty() &&
                searchOptions.Sizes.IsNullOrEmpty())
            {
                return Enumerable.Empty<Shirt>();
            }

            if(searchOptions.Colors.IsNullOrEmpty())
            {
                var sizeKeys = GetSizeMatchKeys(searchOptions);

                return sizeKeys.SelectMany(s => this.shirtStore.TryGetValue(s, out var shirts) ? shirts : Enumerable.Empty<Shirt>());
            }

            if(searchOptions.Sizes.IsNullOrEmpty())
            {
                var colorKeys = GetColorMatchKeys(searchOptions);
                return colorKeys.SelectMany(s => this.shirtStore.TryGetValue(s, out var shirts) ? shirts : Enumerable.Empty<Shirt>());
            }

            var colorMatchesKeys = GetColorMatchKeys(searchOptions);
            var sizeMatchKeys = GetSizeMatchKeys(searchOptions);

            var jointKeys = colorMatchesKeys.Where(key => sizeMatchKeys.Contains(key)).ToArray();
            
            var combinedKeys = jointKeys.SelectMany(s => this.shirtStore.TryGetValue(s, out var shirts) ? shirts : Enumerable.Empty<Shirt>());

            return combinedKeys;
        } 

        private IEnumerable<ShirtKey> GetColorMatchKeys(SearchOptions options)
        {
            var colorKeys = options.Colors.SelectMany(col => this.colorLookupTable.TryGetValue(col.Id, out var keys) ? keys : Enumerable.Empty<ShirtKey>());

            return colorKeys.ToArray();
        }

        private IEnumerable<ShirtKey> GetSizeMatchKeys(SearchOptions options)
        {
            var sizeKeys = options.Sizes.SelectMany(col => this.sizeLookupTable.TryGetValue(col.Id, out var keys) ? keys : Enumerable.Empty<ShirtKey>());

            return sizeKeys.ToArray();
        }

        protected IReadOnlyDictionary<Guid, IEnumerable<ShirtKey>> CreateColourTable(IReadOnlyDictionary<ShirtKey, IEnumerable<Shirt>> shirts)
        {
            var shirtsByColor = shirts.Keys.GroupBy(s => s.Color.Id);

            return new ReadOnlyDictionary<Guid, IEnumerable<ShirtKey>>(shirtsByColor.ToDictionary(s => s.Key, s => s.ToArray().AsEnumerable()));
        }

        protected IReadOnlyDictionary<Guid, IEnumerable<ShirtKey>> CreateSizeTable(IReadOnlyDictionary<ShirtKey, IEnumerable<Shirt>> shirts)
        {
            var shirtsBySize = shirts.Keys.GroupBy(s => s.Size.Id);

            return new ReadOnlyDictionary<Guid, IEnumerable<ShirtKey>>(shirtsBySize.ToDictionary(s => s.Key, s => s.ToArray().AsEnumerable()));
        }

        private IReadOnlyDictionary<ShirtKey, IEnumerable<Shirt>> CreateShirtLookupTable(IEnumerable<Shirt> shirts)
        {
            var groupedOnKey = shirts.Where(s => s != null)
                            .GroupBy(s => CreateKey(s));
            var asDictionary = groupedOnKey.ToDictionary(s => s.Key, s => s.ToArray().AsEnumerable());

            return new ReadOnlyDictionary<ShirtKey, IEnumerable<Shirt>>(asDictionary);
        }

        private ShirtKey CreateKey(Shirt shirt)
        {
            return ShirtKey.Create(shirt.Size, shirt.Color);
        }
        
        protected class ShirtKey
        {
            private static readonly Lazy<ConcurrentDictionary<string, ShirtKey>> existingKeys = new Lazy<ConcurrentDictionary<string, ShirtKey>>(() => new ConcurrentDictionary<string, ShirtKey>());

            private ShirtKey(Size size, Color color)
            {
                this.Size = size ?? throw new ArgumentNullException(nameof(size));
                this.Color = color ?? throw new ArgumentNullException(nameof(color));

                this.Key = GenerateKey(this.Size, this.Color);
            }

            public string Key { get; }

            public Color Color { get; }

            public Size Size { get; }

            public static ShirtKey Create(Size size, Color color)
            {
                var descriptionKey = GenerateKey(size, color);

                var key = existingKeys.Value.GetOrAdd(descriptionKey, new ShirtKey(size, color));

                return key;
            }

            public override int GetHashCode()
            {
                return this.Key?.GetHashCode() ?? 0;
            }

            public override bool Equals(object obj)
            {
                if(obj == null)
                {
                    return false;
                }

                var converted = obj as ShirtKey;
                return converted == null ? base.Equals(obj) : this.Equals(converted.Key);
            }

            private bool Equals(string key)
            {
                if(this.Key == null && key == null)
                {
                    return false;
                }

                return this.Key?.Equals(key) ?? false;
            }

            private static string GenerateKey(Size size, Color color)
            {
                return size.Id + "##" + color.Id;
            }
        }
    }
}
