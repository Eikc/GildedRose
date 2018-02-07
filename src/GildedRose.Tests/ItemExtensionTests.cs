using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GildedRose.Console;
using Xunit;

namespace GildedRose.Tests
{
    public class ItemExtensionTests
    {
        [Fact]
        public void ItemIsAnAgedBrie()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 1, Quality = 0 };

            item.IsAgedBrie().Should().BeTrue();
        }

        [Fact]
        public void ItemIsABackStagePass()
        {
            var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 1, Quality = 0 };

            item.IsBackStagePass().Should().BeTrue();
        }

        [Fact]
        public void ItemIsSulfaras()
        {
            var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 1, Quality = 0 };

            item.IsSulfurasTheHandOfRagnaros().Should().BeTrue();
        }

        [Fact]
        public void ItemIsConjured()
        {
            var item = new Item { Name = "Conjured Mana Cake", SellIn = 1, Quality = 0 };

            item.IsConjured().Should().BeTrue();
        }
    }
}
