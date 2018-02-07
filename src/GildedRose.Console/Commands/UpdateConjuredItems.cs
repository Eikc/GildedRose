namespace GildedRose.Console.Commands
{
    public interface IUpdateConjuredItems
    {
        void Execute(Item item);
    }

    public class UpdateConjuredItems : IUpdateConjuredItems
    {
        private readonly IDecreaseQuality _decreaseQuality;
        private readonly IDecreaseSellIn _decreaseSellIn;

        public UpdateConjuredItems(IDecreaseQuality decreaseQuality,
            IDecreaseSellIn decreaseSellIn)
        {
            _decreaseQuality = decreaseQuality;
            _decreaseSellIn = decreaseSellIn;
        }

        public void Execute(Item item)
        {
            if (!item.IsConjured())
                return;

            _decreaseQuality.Execute(item);
            _decreaseQuality.Execute(item);
            _decreaseSellIn.Execute(item);

            if (ItemIsNotTooOld(item)) return;

            _decreaseQuality.Execute(item);
            _decreaseQuality.Execute(item);
        }

        private static bool ItemIsNotTooOld(Item item)
            => item.SellIn >= 0;
    }
}