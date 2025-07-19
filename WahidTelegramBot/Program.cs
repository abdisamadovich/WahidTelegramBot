using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WahidTelegramBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient("8063166313:AAHspeuZWZNtyPfauVD5EADpP2S4QmVpG_g");

            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Bot ishga tushdi: @{me.Username}");
            Console.ReadLine();
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message || string.IsNullOrWhiteSpace(message.Text))
                return;

            var chatId = message.Chat.Id;
            var userId = message.From?.Id;
            var messageText = message.Text.Trim();

            Console.WriteLine($"[{userId}] -> {messageText}");

            if (!messageText.StartsWith("/"))
            {
                await botClient.SendTextMessageAsync(chatId, "Iltimos, avatar olish uchun buyruqdan foydalaning.", cancellationToken: cancellationToken);
                return;
            }

            var parts = messageText.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var command = parts[0].ToLowerInvariant();
            var seed = parts.Length > 1 ? parts[1].Trim() : null;

            var styles = new Dictionary<string, string>
            {
                ["/fun-emoji"] = "fun-emoji",
                ["/avataaars"] = "avataaars",
                ["/bottts"] = "bottts",
                ["/pixel-art"] = "pixel-art",
                ["/help"] = "help"
            };

            if (command == "/help")
            {
                var help = """
                Avatar yaratish uchun quyidagi buyruqlardan foydalaning:

                /fun-emoji Ali
                /bottts Robot
                /avataaars John
                /pixel-art PixelUser

                Har bir buyruqdan keyin ism (seed) yozing.
                Misol: /fun-emoji John Doe
                """;

                await botClient.SendTextMessageAsync(chatId, help, cancellationToken: cancellationToken);
                return;
            }

            if (!styles.ContainsKey(command))
            {
                var msg = "Noma’lum buyruq. Quyidagilarni ishlatishingiz mumkin:\n" +
                          "/fun-emoji, /bottts, /avataaars, /pixel-art";
                await botClient.SendTextMessageAsync(chatId, msg, cancellationToken: cancellationToken);
                return;
            }

            if (string.IsNullOrWhiteSpace(seed))
            {
                await botClient.SendTextMessageAsync(chatId, "Iltimos, buyruqdan keyin matn (seed) kiriting. Masalan: /fun-emoji Ali", cancellationToken: cancellationToken);
                return;
            }

            var style = styles[command];
            var imageUrl = $"https://api.dicebear.com/8.x/{style}/png?seed={Uri.EscapeDataString(seed)}";

            try
            {
                using var http = new HttpClient();
                var result = await http.GetAsync(imageUrl, cancellationToken);

                if (!result.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Dicebear xatolik: {result.StatusCode}");
                    await botClient.SendTextMessageAsync(chatId, "Avatar yaratishda xatolik yuz berdi. Keyinroq urinib ko‘ring.", cancellationToken: cancellationToken);
                    return;
                }

                var stream = await result.Content.ReadAsStreamAsync(cancellationToken);
                await botClient.SendPhotoAsync(chatId, InputFile.FromStream(stream, $"{style}-{seed}.png"), cancellationToken: cancellationToken);

                Console.WriteLine($"✅ {command} {seed} uchun avatar yuborildi.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http xatolik: {ex.Message}");
                await botClient.SendTextMessageAsync(chatId, "Avatar yaratishda xatolik yuz berdi. Keyinroq urinib ko‘ring.", cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Umumiy xatolik: {ex.Message}");
                await botClient.SendTextMessageAsync(chatId, "Rasmni yuborishda xatolik yuz berdi.", cancellationToken: cancellationToken);
            }
        }

        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string error = exception switch
            {
                ApiRequestException apiEx => $"Telegram API xatosi: {apiEx.Message}",
                _ => exception.Message
            };

            Console.WriteLine($"Xatolik: {error}");
            return Task.CompletedTask;
        }
    }
}
