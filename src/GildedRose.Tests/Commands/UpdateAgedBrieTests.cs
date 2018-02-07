using GildedRose.Console;
using GildedRose.Console.Commands;
using NSubstitute;
using Xunit;

namespace GildedRose.Tests.Commands
{
    public class UpdateAgedBrieTests
    {
        private readonly UpdateAgedBrie _sut;
        private readonly IIncreaseQuality _increaseQuality;
        private readonly IDecreaseSellIn _decreaseSellIn;

        public UpdateAgedBrieTests()
        {
            _increaseQuality = Substitute.For<IIncreaseQuality>();
            _decreaseSellIn = Substitute.For<IDecreaseSellIn>();

            _sut = new UpdateAgedBrie(_increaseQuality, _decreaseSellIn);
        }

        [Fact]
        public void UpdateAgedBrieIncreasesInQualityTheOlderItGets()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 1, Quality = 0 };

            _sut.Execute(item);

            _increaseQuality.Received(1).Execute(Arg.Is(item));
        }

        [Fact]
        public void UpdateAgedBrieDecreasesSellInTime()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 1, Quality = 0 };

            _sut.Execute(item);

            _decreaseSellIn.Received(1).Execute(Arg.Is(item));
        }

        [Fact]
        public void AgedBrieQualityIsIncreasedDoubleWhenItsTooOld()
        {
            var item = new Item { Name = "Aged Brie", SellIn = -1, Quality = 0 };

            _sut.Execute(item);

            _increaseQuality.Received(2).Execute(Arg.Is(item));
        }

        [Fact]
        public void IfTheItemIsNotAnAgedBrieWeReturnDirectly()
        {
            var item = new Item { Name = "LEEEROOOOOY", SellIn = -1, Quality = 0 };

            _sut.Execute(item);

            _increaseQuality.DidNotReceive().Execute(Arg.Is(item));
            _decreaseSellIn.DidNotReceive().Execute(Arg.Is(item));
        }
    }
}
