using FluentAssertions;
using GildedRose.Console;
using GildedRose.Console.Commands;
using Xunit;

namespace GildedRose.Tests.Commands
{
    public class DecreaseSellInTests
    {
        private readonly DecreaseSellIn _sut;

        public DecreaseSellInTests()
        {
            _sut = new DecreaseSellIn();
        }

        [Fact]
        public void DecreaseSellInDecreasesCorrectlyByOne()
        {
            var item = new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 10 };

            _sut.Execute(item);

            item.SellIn.ShouldBeEquivalentTo(4);
        }
    }
}
