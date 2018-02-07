using FluentAssertions;
using GildedRose.Console;
using GildedRose.Console.Commands;
using Xunit;

namespace GildedRose.Tests.Commands
{
    public class DecreaseQualityTests
    {
        private readonly DecreaseQuality _sut;

        public DecreaseQualityTests()
        {
            _sut = new DecreaseQuality();
        }

        [Fact]
        public void IfQualityIsZeroTheQualityCantBeDecreased()
        {
            var item = new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 0 };

            _sut.Execute(item);

            item.Quality.ShouldBeEquivalentTo(0);
        }

        [Fact]
        public void IfQualityIsAboveZeroItIsDecreased()
        {
            var item = new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 5 };

            _sut.Execute(item);

            item.Quality.ShouldBeEquivalentTo(4);
        }
    }
}
