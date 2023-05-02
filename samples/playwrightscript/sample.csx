
#r "Microsoft.Playwright.dll"
#r "Microsoft.Extensions.Logging.dll"
using Microsoft.Playwright;
using Microsoft.Extensions.Logging;

public class PlaywrightScript {
    public static void Run(IBrowserContext context, ILogger logger) {
        logger.LogInformation("Hello World");
    }
}