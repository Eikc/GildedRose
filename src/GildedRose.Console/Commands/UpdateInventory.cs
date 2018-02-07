using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Console.Commands
{
    public interface IUpdateInventory
    {
        void Execute(IList<Item> items);
    }

    public class UpdateInventory : IUpdateInventory
    {
        private readonly IUpdateBackStageItems _updateBackStageItems;
        private readonly IUpdateAgedBrie _updateAgedBrie;
        private readonly IUpdateConjuredItems _updateConjuredItems;
        private readonly IUpdateOrdinaryItems _updateOrdinaryItems;

        public UpdateInventory(IUpdateBackStageItems updateBackStageItems,
            IUpdateAgedBrie updateAgedBrie,
            IUpdateConjuredItems updateConjuredItems,
            IUpdateOrdinaryItems updateOrdinaryItems)
        {
            _updateBackStageItems = updateBackStageItems;
            _updateAgedBrie = updateAgedBrie;
            _updateConjuredItems = updateConjuredItems;
            _updateOrdinaryItems = updateOrdinaryItems;
        }

        public void Execute(IList<Item> items)
        {
            foreach (var item in items.Where(x => !x.IsSulfurasTheHandOfRagnaros()))
            {
                if (item.IsBackStagePass())
                    _updateBackStageItems.Execute(item);
                else if (item.IsAgedBrie())
                    _updateAgedBrie.Execute(item);
                else if (item.IsConjured())
                    _updateConjuredItems.Execute(item);
                else
                    _updateOrdinaryItems.Execute(item);
            }
        }
    }
}
