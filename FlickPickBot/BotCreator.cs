using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FlickPickBot;

public class BotCreator
{
    public event Action<Update> OnGotUpdate;
    
    public async void CreateBot()
    {
        var key = Environment.GetEnvironmentVariable(ENV_BOT_API_KEY);
        Debug.Assert(key != null, $"Can't find environment variable with name {ENV_BOT_API_KEY}");
        
        _botClient = new TelegramBotClient(key);
        _botUser = await _botClient.GetMeAsync();
        
        Console.WriteLine($"Hello, World! I am user {_botUser.Id} and my name is {_botUser.FirstName}.");
        
        using CancellationTokenSource cts = new();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );
    }

    public TelegramBotClient GetBotClient()
    {
        return _botClient;
    }
    
    public User GetBotUser()
    {
        return _botUser;
    }

    #region private
    
    private const string ENV_BOT_API_KEY = "TELEGRAM_BOT_API_KEY";
    
    private TelegramBotClient _botClient;
    private User _botUser;

    private Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        OnGotUpdate(update);
        return Task.CompletedTask;
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
    {
        var errorMsg = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMsg);
            
        return Task.FromResult(Task.CompletedTask);
    }

    #endregion
}