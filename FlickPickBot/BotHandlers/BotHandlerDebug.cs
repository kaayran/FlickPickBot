using Telegram.Bot;
using Telegram.Bot.Types;

namespace FlickPickBot.BotHandlers;

public class BotHandlerDebug : BotHandlerBase
{
    public BotHandlerDebug(BotCreator botCreator) : base(botCreator)
    {
    }

    protected override void HandleGotUpdateInternal(Update update)
    {
        SendMessage(update, $"Incoming MessageType is {Enum.GetName(update.Message.Type)}");
    }

    protected override bool IsValidUpdate(Update update)
    {
        if (!base.IsValidUpdate(update)) {
            return false;
        }

        return update.Message != null;
    }

    private async void SendMessage(Update update, string text)
    {
        Message sentMessage = await _botCreator.GetBotClient().SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: text);
    }
}