// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.PowerFx.Types;
using Microsoft.PowerFx;
using Microsoft.Playwright;
using Microsoft.PowerApps.TestEngine.TestInfra;
using Microsoft.Extensions.Logging;
using Microsoft.PowerApps.TestEngine.Config;
using Microsoft.PowerApps.TestEngine.System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace testengine.module
{
    /// <summary>
    /// This will execute playwright actions for the current page
    /// </summary>
    public class PlaywrightActionValueFunction : ReflectionFunction
    {
        private readonly ITestInfraFunctions _testInfraFunctions;
        private readonly ISingleTestInstanceState _singleTestInstanceState;
        private readonly IFileSystem _fileSystem;
        private readonly ITestState _testState;
        private readonly ILogger _logger;

        public PlaywrightActionValueFunction(ITestInfraFunctions testInfraFunctions, ISingleTestInstanceState singleTestInstanceState, IFileSystem fileSystem, ITestState testState, ILogger logger)
            : base("PlaywrightActionValue", FormulaType.Blank, FormulaType.String, FormulaType.String, FormulaType.String,FormulaType.Number)
        {
            _testInfraFunctions = testInfraFunctions;
            _singleTestInstanceState = singleTestInstanceState;
            _fileSystem = fileSystem;
            _testState = testState;
            _logger = logger;
        }


        public BooleanValue Execute(StringValue locator, StringValue action, StringValue value,NumberValue typingspeed)
        {
            _logger.LogInformation("------------------------------\n\n" +
                "Executing PlaywrightActionValue function. '"+ locator + "," + action +","+ value.Value +"'");

            if (string.IsNullOrEmpty(locator.Value))
            {
                _logger.LogError("locator cannot be empty.");
                throw new ArgumentException();
            }

            IPage page = _testInfraFunctions.GetContext().Pages.First();
            int delayfortyping = 0;

            switch (action.Value.ToLower())
            {
                
                case "click-in-iframe":
                    LocatorClickOptions locClickOptions  = new LocatorClickOptions();                    
                    locClickOptions.Delay=1000;
                    locClickOptions.Force=true;
                    foreach (var frame in page.Frames)
                    {
                        if (frame.Locator(locator.Value).First.IsVisibleAsync().Result)
                        {
                            frame.Locator(locator.Value).First.ClickAsync(locClickOptions).Wait();                            
                        }
                        
                    }
                    
                    break;
                case "fill-in-iframe":
                    foreach (var frame in page.Frames)
                    {
                        if (frame.Locator(locator.Value).IsVisibleAsync().Result)
                        {
                            frame.Locator(locator.Value).FillAsync("").Wait();
                            //frame.Locator(locator.Value).TypeAsync(value.Value, new LocatorTypeOptions { Delay = 100 }).Wait();
                            if ((int)typingspeed.Value>0)
                              {
                                delayfortyping = (int)typingspeed.Value;
                                frame.Locator(locator.Value).TypeAsync(value.Value, new LocatorTypeOptions { Delay = delayfortyping }).Wait();
                            }
                            else
                            {
                                frame.Locator(locator.Value).TypeAsync(value.Value, new LocatorTypeOptions { Delay = 50 }).Wait();
                            }
                           
                        }
                    }
                    
                    break;
                case "press":
                    
                    foreach (var frame in page.Frames)
                    {
                        if (frame.Locator(locator.Value).IsVisibleAsync().Result)
                        {
                            frame.Locator(locator.Value).PressAsync(value.Value).Wait(500);
                        }

                    }
                    break;
                case "fill":
                    _testInfraFunctions.FillAsync(locator.Value, value.Value).Wait();
                    break;
                case "screenshot":
                    var testResultDirectory = _singleTestInstanceState.GetTestResultsDirectory();
                    if (!_fileSystem.IsValidFilePath(testResultDirectory))
                    {
                        _logger.LogError("Test result directory needs to be set.");
                        throw new InvalidOperationException();
                    }

                    var fileName = value.Value;

                    if (string.IsNullOrEmpty(fileName))
                    {
                        _logger.LogTrace("File Name: " + nameof(fileName));
                        _logger.LogError("File must exist and cannot be empty.");
                        throw new ArgumentException();
                    }

                    if (Path.IsPathRooted(fileName))
                    {
                        _logger.LogError("Only support relative file paths");
                        throw new ArgumentException();
                    }

                    if (!fileName.EndsWith(".jpg") && !fileName.EndsWith(".jpeg") && !fileName.EndsWith("png"))
                    {
                        _logger.LogDebug("File extension: " + Path.GetExtension(fileName));
                        _logger.LogTrace("File name: " + fileName);
                        _logger.LogError("Only support jpeg and png files");
                        throw new ArgumentException();
                    }

                    var filePath = Path.Combine(testResultDirectory, fileName);

                    _logger.LogInformation("Screenshot item");
                    page.Locator(locator.Value).ScreenshotAsync(new LocatorScreenshotOptions() { Path = filePath }).Wait();
                    break;
                default:
                    _logger.LogError("Action not found " + action.Value);
                    throw new ArgumentException();
            }

            _logger.LogInformation("Successfully finished executing PlaywrightActionValue function.");

            return BooleanValue.New(true);
        }
    }
}

