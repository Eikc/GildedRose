using GildedRose.Console;
using GildedRose.Console.Commands;
using NSubstitute;
using Xunit;

namespace GildedRose.Tests.Commands
{
    public class UpdateOrdinaryItemTests
    {
        private readonly IDecreaseQuality _decreaseQuality;
        private readonly IDecreaseSellIn _decreaseSellIn;
        private readonly UpdateOrdinaryItem _sut;

        public UpdateOrdinaryItemTests()
        {
            _decreaseQuality = Substitute.For<IDecreaseQuality>();
            _decreaseSellIn = Substitute.For<IDecreaseSellIn>();

            _sut = new UpdateOrdinaryItem(_decreaseQuality, _decreaseSellIn);
        }

        [Theory]
        [InlineData("Aged Brie")]
        [InlineData("Backstage passes to a TAFKAL80ETC concert")]
        [InlineData("Sulfuras, Hand of Ragnaros")]
        public void IfItemIsNotOrdinaryThenItReturnsDirectly(string itemName)
        {
            var item = new Item { Name = itemName, SellIn = 10, Quality = 20 };

            _sut.Execute(item);

            _decreaseQuality.DidNotReceive().Execute(Arg.Is(item));
            _decreaseSellIn.DidNotReceive().Execute(Arg.Is(item));
        }

        [Fact]
        public void OrdinaryItemsDecreaseNormallyInQuality()
        {
            var item = new Item { Name = "Elixir of the Mongoose", SellIn = 10, Quality = 20 };

            _sut.Execute(item);

            _decreaseQuality.Received(1).Execute(Arg.Is(item));
        }

        [Fact]
        public void OrdinaryItemsDecreaseInSellIn()
        {
            var item = new Item { Name = "Elixir of the Mongoose", SellIn = 10, Quality = 20 };

            _sut.Execute(item);

            _decreaseSellIn.Received(1).Execute(Arg.Is(item));
        }

        [Fact]
        public void IfAnOrdinaryItemIsTooOldItDegradesTwice()
        {
            var item = new Item { Name = "Elixir of the Mongoose", SellIn = -1, Quality = 20 };

            _sut.Execute(item);

            _decreaseQuality.Received(2).Execute(Arg.Is(item));
        }
    }
}
