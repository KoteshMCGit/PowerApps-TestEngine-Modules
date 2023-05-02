// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.PowerFx.Types;
using Microsoft.PowerFx;
using Microsoft.Playwright;
using Microsoft.PowerApps.TestEngine.TestInfra;
using Microsoft.Extensions.Logging;
using Microsoft.PowerApps.TestEngine.Config;

namespace testengine.module
{
    /// <summary>
    /// This will reload the current page
    /// </summary>
    public class ReloadFunction : ReflectionFunction
    {
        private readonly ITestInfraFunctions _testInfraFunctions;
        private readonly ITestState _testState;
        private readonly ILogger _logger;

        public ReloadFunction(ITestInfraFunctions testInfraFunctions, ITestState testState, ILogger logger)
            : base("Reload", FormulaType.Blank)
        {
            _testInfraFunctions = testInfraFunctions;
            _testState = testState;
            _logger = logger;
        }

        public BlankValue Execute()
        {
            _logger.LogInformation("------------------------------\n\n" +
                "Executing Reload function.");

            var page = _testInfraFunctions.GetContext().Pages.First();
            page.ReloadAsync().Wait();

            _logger.LogInformation("Successfully finished executing Reload function.");


            return FormulaValue.NewBlank();
        }
    }
}

