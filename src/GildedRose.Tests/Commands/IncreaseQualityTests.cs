using FluentAssertions;
using GildedRose.Console;
using GildedRose.Console.Commands;
using Xunit;

namespace GildedRose.Tests.Commands
{
    public class IncreaseQualityTests
    {
        private readonly IncreaseQuality _sut;

        public IncreaseQualityTests()
        {
            _sut = new IncreaseQuality();
        }

        [Theory]
        [InlineData(50)]
        [InlineData(60)]
        [InlineData(80)]
        public void DoesNotIncreasesQualityWhenItemIs50OrAbove(int quality)
        {
            var item = new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = quality };

            _sut.Execute(item);

            item.Quality.ShouldBeEquivalentTo(quality);
        }

        [Fact]
        public void IncreasesQualityWhenItemIsBelow50()
        {
            var item = new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 10 };

            _sut.Execute(item);

            item.Quality.ShouldBeEquivalentTo(11);
        }
    }
}
