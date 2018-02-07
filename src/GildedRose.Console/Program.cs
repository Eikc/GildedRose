using System.Collections.Generic;
using System.Linq;
using GildedRose.Console.Commands;

namespace GildedRose.Console
{
    public class Program
    {
        private readonly IList<Item> _items;
        private readonly IUpdateInventory _updateInventory;

        public Program(IList<Item> items, IUpdateInventory updateInventory)
        {
            _items = items;
            _updateInventory = updateInventory;
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("OMGHAI!");
            var updateAgedBrie = new UpdateAgedBrie(new IncreaseQuality(), new DecreaseSellIn());
            var updateBackStageItems = new UpdateBackStageItems(new IncreaseQuality(), new DecreaseSellIn());
            var updateOrdinaryItems = new UpdateOrdinaryItem(new DecreaseQuality(), new DecreaseSellIn());
            var updateConjuredItems = new UpdateConjuredItems(new DecreaseQuality(), new DecreaseSellIn());

            var updateInventory = new UpdateInventory(updateBackStageItems, updateAgedBrie, updateConjuredItems,
                updateOrdinaryItems);


            var app = new Program(new List<Item>
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
            }, updateInventory);

            app.UpdateQuality();

            System.Console.ReadKey();

        }

        public void UpdateQuality()
        {

            _updateInventory.Execute(_items);
        }
    }


    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }


    public static class ItemExtenstions
    {
        public static bool IsAgedBrie(this Item item) => item.Name == "Aged Brie";

        public static bool IsBackStagePass(this Item item) => item.Name == "Backstage passes to a TAFKAL80ETC concert";

        public static bool IsSulfurasTheHandOfRagnaros(this Item item) => item.Name == "Sulfuras, Hand of Ragnaros";

        public static bool IsConjured(this Item item) => item.Name.Contains("Conjured");
    }
}
