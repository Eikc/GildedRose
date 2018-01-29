namespace GildedRose.Console.Commands
{
    public interface IUpdateOrdinaryItems
    {
        void Execute(Item item);
    }

    public class UpdateOrdinaryItem : IUpdateOrdinaryItems
    {
        private readonly IDecreaseQuality _decreaseQuality;
        private readonly IDecreaseSellIn _decreaseSellIn;

        public UpdateOrdinaryItem(IDecreaseQuality decreaseQuality, IDecreaseSellIn decreaseSellIn)
        {
            _decreaseQuality = decreaseQuality;
            _decreaseSellIn = decreaseSellIn;
        }

        public void Execute(Item item)
        {
            if (ItemIsNotOrdinary(item))
                return;

            _decreaseQuality.Execute(item);
            _decreaseSellIn.Execute(item);

            if (ItemIsToOld(item))
                _decreaseQuality.Execute(item);
        }

        private static bool ItemIsNotOrdinary(Item item)
            => item.IsBackStagePass() ||
               item.IsAgedBrie() ||
               item.IsSulfurasTheHandOfRagnaros();

        private static bool ItemIsToOld(Item item)
            => item.SellIn < 0;
    }
}
