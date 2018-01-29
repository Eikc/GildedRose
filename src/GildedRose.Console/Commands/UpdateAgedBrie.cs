namespace GildedRose.Console.Commands
{
    public interface IUpdateAgedBrie
    {
        void Execute(Item item);
    }

    public class UpdateAgedBrie : IUpdateAgedBrie
    {
        private readonly IIncreaseQuality _increaseQuality;
        private readonly IDecreaseSellIn _decreaseSellIn;

        public UpdateAgedBrie(
            IIncreaseQuality increaseQuality,
            IDecreaseSellIn decreaseSellIn
        )
        {
            _increaseQuality = increaseQuality;
            _decreaseSellIn = decreaseSellIn;
        }

        public void Execute(Item item)
        {
            if (!item.IsAgedBrie())
                return;

            _increaseQuality.Execute(item);
            _decreaseSellIn.Execute(item);

            if (ItemIsToOld(item))
                _increaseQuality.Execute(item);
        }

        private static bool ItemIsToOld(Item item)
            => item.SellIn < 0;
    }
}