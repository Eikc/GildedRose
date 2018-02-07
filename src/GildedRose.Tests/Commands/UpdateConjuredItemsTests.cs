using GildedRose.Console;
using GildedRose.Console.Commands;
using NSubstitute;
using Xunit;

namespace GildedRose.Tests.Commands
{
    public class UpdateConjuredItemsTests
    {
        private readonly IDecreaseQuality _decreaseQuality;
        private readonly IDecreaseSellIn _decreaseSellIn;
        private readonly UpdateConjuredItems _sut;

        public UpdateConjuredItemsTests()
        {
            _decreaseQuality = Substitute.For<IDecreaseQuality>();
            _decreaseSellIn = Substitute.For<IDecreaseSellIn>();

            _sut = new UpdateConjuredItems(_decreaseQuality, _decreaseSellIn);
        }

        [Fact]
        public void ConjuredItemsDecreaseTwiceInQuality()
        {
            var manaCake = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 };

            _sut.Execute(manaCake);

            _decreaseQuality.Received(2).Execute(Arg.Is(manaCake));
        }

        [Fact]
        public void ConjuredItemsDecreaseInSellInInANormalRate()
        {
            var manaCake = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 };

            _sut.Execute(manaCake);

            _decreaseSellIn.Received(1).Execute(Arg.Is(manaCake));
        }

        [Fact]
        public void ConjuredItemsThatAreTooOldDegradeTwiceAsFast()
        {
            var manaCake = new Item { Name = "Conjured Mana Cake", SellIn = -1, Quality = 6 };

            _sut.Execute(manaCake);

            _decreaseQuality.Received(4).Execute(Arg.Is(manaCake));
        }

        [Fact]
        public void IfTheItemIsNotAConjuredItemItWillReturnInstanly()
        {
            var fakeManaCake = new Item { Name = "A real Cake", SellIn = 1, Quality = 6 };

            _sut.Execute(fakeManaCake);

            _decreaseSellIn.DidNotReceive().Execute(Arg.Any<Item>());
            _decreaseQuality.DidNotReceive().Execute(Arg.Any<Item>());
        }
    }
}
