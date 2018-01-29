namespace GildedRose.Console.Commands
{
    public interface IUpdateBackStageItems
    {
        void Execute(Item item);
    }

    public class UpdateBackStageItems : IUpdateBackStageItems
    {
        private readonly IIncreaseQuality _increaseQuality;
        private readonly IDecreaseSellIn _decreaseSellIn;

        public UpdateBackStageItems(
            IIncreaseQuality increaseQuality,
            IDecreaseSellIn decreaseSellIn
        )
        {
            _increaseQuality = increaseQuality;
            _decreaseSellIn = decreaseSellIn;
        }

        public void Execute(Item item)
        {
            if (!item.IsBackStagePass())
                return;

            _increaseQuality.Execute(item);

            if (LessThan11DaysTillConcert(item))
                _increaseQuality.Execute(item);

            if (LessThan6DaysTillConcert(item))
                _increaseQuality.Execute(item);

            if (ConcertIsOver(item))
                item.Quality = 0;

            _decreaseSellIn.Execute(item);
        }

        private static bool LessThan11DaysTillConcert(Item item)
            => item.SellIn < 11;

        private static bool LessThan6DaysTillConcert(Item item)
            => item.SellIn < 6;

        private static bool ConcertIsOver(Item item)
            => item.SellIn <= 0;
    }
}