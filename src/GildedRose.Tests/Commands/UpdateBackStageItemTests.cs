using FluentAssertions;
using GildedRose.Console;
using GildedRose.Console.Commands;
using NSubstitute;
using Xunit;

namespace GildedRose.Tests.Commands
{
    public class UpdateBackStageItemTests
    {
        private readonly IIncreaseQuality _increaseQuality;
        private readonly IDecreaseSellIn _decreaseSellIn;
        private readonly UpdateBackStageItems _sut;

        public UpdateBackStageItemTests()
        {
            _increaseQuality = Substitute.For<IIncreaseQuality>();
            _decreaseSellIn = Substitute.For<IDecreaseSellIn>();

            _sut = new UpdateBackStageItems(_increaseQuality, _decreaseSellIn);
        }

        [Fact]
        public void UpdateBackStageWillReturnIfTheItemIsNotABackStagePass()
        {
            var theLegendaryAshesOfAlar = new Item { Name = "Ashes of Al'ar", SellIn = 55, Quality = 80 };

            _sut.Execute(theLegendaryAshesOfAlar);

            _increaseQuality.DidNotReceiveWithAnyArgs();
            _decreaseSellIn.DidNotReceiveWithAnyArgs();
        }

        [Fact]
        public void UpdateBackStageWillIncrementQualityCorrectly()
        {
            var backstage = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 };

            _sut.Execute(backstage);

            _increaseQuality.Received(1).Execute(Arg.Is(backstage));
        }

        [Fact]
        public void UpdateBackStageWillDecreaseSellInCorrectly()
        {
            var backstage = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 };

            _sut.Execute(backstage);

            _decreaseSellIn.Received(1).Execute(Arg.Is(backstage));
        }

        [Fact]
        public void UpdateBackStageWillDoubleWhenTheConcertIsInLessThan11Days()
        {
            var backstage = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 };

            _sut.Execute(backstage);

            _increaseQuality.Received(2).Execute(Arg.Is(backstage));
        }

        [Fact]
        public void UpdateBackStageWillBeIncreasedBy3WhenTheConcertIsInLessThan6Days()
        {
            var backstage = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 };

            _sut.Execute(backstage);

            _increaseQuality.Received(3).Execute(Arg.Is(backstage));
        }

        [Fact]
        public void UpdateBackStageWillDecreaseQualityToZeroWhenConcertIsOver()
        {
            var backstage = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = -1, Quality = 20 };

            _sut.Execute(backstage);

            backstage.Quality.ShouldBeEquivalentTo(0);
        }
    }
}
