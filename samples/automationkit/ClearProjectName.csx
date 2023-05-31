#r "Microsoft.Playwright.dll"
#r "Microsoft.Extensions.Logging.dll"
using Microsoft.Playwright;
using Microsoft.Extensions.Logging;
using System.Linq;

public class PlaywrightScript {
    public static void Run(IBrowserContext context, ILogger logger) {
        var page = context.Pages.First();
        string strlocator="//input[@placeholder='Name of project']";
        foreach ( var frame in page.Frames ) 
        {
            if (frame.Locator(strlocator).IsVisibleAsync().Result)
                {
                    
                    frame.Locator(strlocator).FillAsync("").Wait();
                    //frame.Locator(strlocator).TypeAsync("kotesh from script file", new LocatorTypeOptions { Delay = 100 }).Wait();
                    
                }
        }
    }
}