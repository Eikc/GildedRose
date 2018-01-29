namespace GildedRose.Console.Commands
{
    public interface IDecreaseSellIn
    {
        void Execute(Item item);
    }

    public class DecreaseSellIn : IDecreaseSellIn
    {
        public void Execute(Item item)
        {
            item.SellIn--;
        }
    }
}
