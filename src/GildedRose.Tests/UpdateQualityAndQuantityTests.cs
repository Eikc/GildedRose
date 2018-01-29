using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GildedRose.Console;
using GildedRose.Console.Commands;
using Xunit;

namespace GildedRose.Tests
{
    public class UpdateQualityAndQuantityTests
    {
        [Theory]
        [InlineData("+5 Dexterity Vest", 9, 19)]
        [InlineData("Aged Brie", 1, 1)]
        [InlineData("Elixir of the Mongoose", 4, 6)]
        [InlineData("Sulfuras, Hand of Ragnaros", 0, 80)]
        [InlineData("Backstage passes to a TAFKAL80ETC concert", 14, 21)]
        public void UpdateQualityUdatesCorrectly(string name, int sellIn, int quality)
        {
            var items = new List<Item>
            {
                new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 15,
                    Quality = 20
                },
                new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
            };

            var updateAgedBrie = new UpdateAgedBrie(new IncreaseQuality(), new DecreaseSellIn());
            var updateBackStageItems = new UpdateBackStageItems(new IncreaseQuality(), new DecreaseSellIn());
            var updateOrdinaryItems = new UpdateOrdinaryItem(new DecreaseQuality(), new DecreaseSellIn());
            var updateConjuredItems = new UpdateConjuredItems(new DecreaseQuality(), new DecreaseSellIn());

            var updateInventory = new UpdateInventory(updateBackStageItems, updateAgedBrie, updateConjuredItems,
                updateOrdinaryItems);


            var app = new Program(items, updateInventory);
            app.UpdateQuality();

            var item = items.Single(x => x.Name == name);

            var wantedItem = new Item
            {
                Name = name,
                SellIn = sellIn,
                Quality = quality
            };

            item.ShouldBeEquivalentTo(wantedItem);
        }


        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(0, 2, 0)]
        [InlineData(-1, 2, 0)]
        public void OnceTheSellByDateHasPassedQualityDegradesTwiceAsFast(int sellIn, int currentQuality, int wantedQuality)
        {
            var item = new Item {Name = "+5 Dexterity Vest", SellIn = sellIn, Quality = currentQuality};

            RunProgram(item);

            item.Quality.Should().Be(wantedQuality);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(0)]
        [InlineData(-1)]
        public void TheQualityOfAnItemIsNeverNegative(int sellIn)
        {
            var item = new Item { Name = "+5 Dexterity Vest", SellIn = sellIn, Quality = 0 };

            RunProgram(item);

            item.Quality.ShouldBeEquivalentTo(0);
        }

        [Fact]
        public void AgedBrieActuallyIncreasesInQualityTheOlderItGets()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 1, Quality = 0 };

            RunProgram(item);

            item.Quality.ShouldBeEquivalentTo(1);
        }


        [Theory]
        [InlineData(2)]
        [InlineData(0)]
        [InlineData(-1)]
        public void TheQualityOfAnItemIsNeverMoreThan50(int sellIn)
        {
            var item = new Item { Name = "Aged Brie", SellIn = sellIn, Quality = 49 };

            RunProgram(item);

            item.Quality.ShouldBeEquivalentTo(50);
        }

        [Fact]
        public void SulfurasBeingALegendaryItemNeverHasToBeSoldOrDecreasesInQuality()
        {
            var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 1, Quality = 80 };

            RunProgram(item);

            var originalSulfuras = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 1, Quality = 80 };

            item.ShouldBeEquivalentTo(originalSulfuras);
        }

        [Theory]
        [InlineData(11, 11)]
        [InlineData(12, 11)]
        public void BackstagePassesQualityIncreasesBy1WhenThereAreMoreThan10Days(int days, int wantedQuality)
        {
            var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = days, Quality = 10 };

            RunProgram(item);

            item.Quality.ShouldBeEquivalentTo(wantedQuality);
        }


        [Fact]
        public void BackstagePassesQualityIncreasesBy2WhenThereAreLessThan10Days()
        {
            var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 9, Quality = 10 };

            RunProgram(item);

            item.Quality.ShouldBeEquivalentTo(12);
        }


        [Fact]
        public void BackstagePassesQualityIncreasesBy3WhenThereAreLessThan5Days()
        {
            var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 4, Quality = 10 };

            RunProgram(item);

            item.Quality.ShouldBeEquivalentTo(13);
        }

        [Fact]
        public void BackstagePassesQualityDropsToZeroAfterTheConcert()
        {
            var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 10 };

            RunProgram(item);

            item.Quality.ShouldBeEquivalentTo(0);
        }

        [Theory]
        [InlineData(5, 8)]
        [InlineData(0, 6)]
        public void ConjuredItemsDegradeInQualityTwiceAsFastAsNormalItems(int sellIn, int wantedQuality)
        {
            var item = new Item { Name = "Conjured Mana Cake", SellIn = sellIn, Quality = 10 };

            RunProgram(item);

            item.Quality.ShouldBeEquivalentTo(wantedQuality);
        }


        private void RunProgram(Item item)
        {
            var updateAgedBrie = new UpdateAgedBrie(new IncreaseQuality(), new DecreaseSellIn());
            var updateBackStageItems = new UpdateBackStageItems(new IncreaseQuality(), new DecreaseSellIn());
            var updateOrdinaryItems = new UpdateOrdinaryItem(new DecreaseQuality(), new DecreaseSellIn());
            var updateConjuredItems = new UpdateConjuredItems(new DecreaseQuality(), new DecreaseSellIn());

            var updateInventory = new UpdateInventory(updateBackStageItems, updateAgedBrie, updateConjuredItems,
                updateOrdinaryItems);

            var app = new Program(new List<Item> { item }, updateInventory);
            app.UpdateQuality();
        }
    }
}
