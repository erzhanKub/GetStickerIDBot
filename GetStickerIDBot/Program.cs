using DotNetEnv;
using GetStickerIDBot;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);
Env.Load();
builder.Services.AddHostedService<StickerIDBot>();
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN")
        ?? throw new ArgumentNullException("Environment variable 'TELEGRAM_BOT_TOKEN' is not set")));

var host = builder.Build();
host.Run();
