using System.Collections.Generic;
using GildedRose.Console;
using GildedRose.Console.Commands;
using NSubstitute;
using Xunit;

namespace GildedRose.Tests.Commands
{
    public class UpdateInventoryTests
    {
        private readonly IUpdateAgedBrie _updateAgedBrie;
        private readonly IUpdateBackStageItems _updateBackStage;
        private readonly IUpdateConjuredItems _updateConjuredItems;
        private readonly IUpdateOrdinaryItems _updateOrdinaryItems;

        private readonly UpdateInventory _sut;

        public UpdateInventoryTests()
        {
            _updateAgedBrie = Substitute.For<IUpdateAgedBrie>();
            _updateBackStage = Substitute.For<IUpdateBackStageItems>();
            _updateConjuredItems = Substitute.For<IUpdateConjuredItems>();
            _updateOrdinaryItems = Substitute.For<IUpdateOrdinaryItems>();

            _sut = new UpdateInventory(_updateBackStage, _updateAgedBrie, _updateConjuredItems, _updateOrdinaryItems);
        }

        [Fact]
        public void UpdateInventoryWillLoopOverAListOfItems()
        {
            var agedBrie = new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 };
            var elixir = new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 };
            var manaCake = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 };
            var backStagePass =
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 };


            var items = new List<Item> { agedBrie, elixir, manaCake, backStagePass };

            _sut.Execute(items);

            _updateAgedBrie.Received(1).Execute(Arg.Is(agedBrie));
            _updateBackStage.Received(1).Execute(Arg.Is(backStagePass));
            _updateConjuredItems.Received(1).Execute(Arg.Is(manaCake));
            _updateOrdinaryItems.Received(1).Execute(Arg.Is(elixir));
        }

        [Fact]
        public void UpdateInventoryWillAvoidSulfurasSinceItsALegendaryWeaponThatIsNotSold()
        {
            var sulfuras = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 };

            var items = new List<Item> { sulfuras };

            _sut.Execute(items);

            _updateAgedBrie.DidNotReceive().Execute(Arg.Any<Item>());
            _updateBackStage.DidNotReceive().Execute(Arg.Any<Item>());
            _updateConjuredItems.DidNotReceive().Execute(Arg.Any<Item>());
            _updateOrdinaryItems.DidNotReceive().Execute(Arg.Any<Item>());
        }
    }
}
