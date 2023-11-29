using FlickPickBot.BotHandlers;

namespace FlickPickBot
{
    public static class Program
    {
        public static void Main()
        {
            var creator = new BotCreator();
            creator.CreateBot();

            var botHandlerDebug = new BotHandlerDebug(creator);
            
            Console.ReadLine();
        }
    }
}