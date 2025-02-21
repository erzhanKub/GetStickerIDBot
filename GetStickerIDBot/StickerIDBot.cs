using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GetStickerIDBot;

public sealed class StickerIDBot(ITelegramBotClient telegramBotClient) : BackgroundService
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        => await StartReceivingUpdatesAsync(stoppingToken);

    private async Task StartReceivingUpdatesAsync(CancellationToken cancellationToken)
    {
        _telegramBotClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message],
            }
        );

        await Task.Delay(-1, cancellationToken);
    }

    public async Task HandleUpdateAsync(
    ITelegramBotClient telegramBotClient,
    Update update, CancellationToken cancellationToken)
    {
        if (update.Message?.Chat?.Id is null)
            return;

        var chatId = update.Message.Chat.Id;

        try
        {
            if (update.Message.Sticker != null)
            {
                var stickerId = update.Message.Sticker.FileId;
                await _telegramBotClient.SendMessage(
                    chatId, $"Sticker ID: `{stickerId}`", ParseMode.Markdown, cancellationToken: cancellationToken);
            }
        }
        catch (Exception exception)
        {
            await _telegramBotClient.SendMessage(
                chatId, $"An error occurred while processing the request: {exception.Message}", cancellationToken: cancellationToken);
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient telegramBotClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
