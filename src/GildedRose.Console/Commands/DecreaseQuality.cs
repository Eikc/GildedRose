namespace GildedRose.Console.Commands
{
    public interface IDecreaseQuality
    {
        void Execute(Item item);
    }

    public class DecreaseQuality : IDecreaseQuality
    {
        public void Execute(Item item)
        {
            if (item.Quality <= 0)
                return;

            item.Quality--;
        }
    }
}