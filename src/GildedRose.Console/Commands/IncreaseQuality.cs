namespace GildedRose.Console.Commands
{
    public interface IIncreaseQuality
    {
        void Execute(Item item);
    }

    public class IncreaseQuality : IIncreaseQuality
    {
        public void Execute(Item item)
        {
            if (item.Quality >= 50)
                return;

            item.Quality++;
        }
    }
}
