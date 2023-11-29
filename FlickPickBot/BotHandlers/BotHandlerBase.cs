using Telegram.Bot.Types;

namespace FlickPickBot.BotHandlers;

public abstract class BotHandlerBase
{
    public BotHandlerBase(BotCreator botCreator)
    {
        _botCreator = botCreator;
        _botCreator.OnGotUpdate += HandleGotUpdate;
    }

    ~BotHandlerBase()
    {
        _botCreator.OnGotUpdate -= HandleGotUpdate;
    }

    protected readonly BotCreator _botCreator;

    protected abstract void HandleGotUpdateInternal(Update update);

    protected virtual bool IsValidUpdate(Update update)
    {
        return true;
    }
    
    private void HandleGotUpdate(Update update)
    {
        Console.WriteLine($"Got update with type {update.Type}");

        if (!IsValidUpdate(update)) {
            return;
        }

        HandleGotUpdateInternal(update);
    }
}