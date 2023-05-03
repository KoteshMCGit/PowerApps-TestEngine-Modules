// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.PowerFx.Types;
using Microsoft.PowerFx;
using Microsoft.Playwright;
using Microsoft.PowerApps.TestEngine.TestInfra;
using Microsoft.Extensions.Logging;
using Microsoft.PowerApps.TestEngine.Config;
using Microsoft.PowerApps.TestEngine.Helpers;
using Microsoft.PowerApps.TestEngine.PowerApps;

namespace testengine.module
{
    /// <summary>
    /// This will check if the current Power Apps page is ready
    /// </summary>
    public class ReadyFunction : ReflectionFunction
    {
        private readonly IPowerAppFunctions _powerAppFunctions;
        private readonly ITestState _testState;
        private readonly ISingleTestInstanceState _singleTestInstanceState;
        private readonly ILogger _logger;

        public ReadyFunction(IPowerAppFunctions powerAppFunctions, ITestState testState, ISingleTestInstanceState singleTestInstanceState, ILogger logger)
            : base("Ready", FormulaType.Blank)
        {
            _powerAppFunctions = powerAppFunctions;
            _testState = testState;
            _singleTestInstanceState = singleTestInstanceState;
            _logger = logger;
        }

        public BlankValue Execute()
        {
            _logger.LogInformation("------------------------------\n\n" +
                "Executing Ready function.");

            _powerAppFunctions.CheckAndHandleIfLegacyPlayerAsync().Wait();

            PollingHelper.PollAsync<bool>(false, (x) => !x, () => _powerAppFunctions.CheckIfAppIsIdleAsync(), _testState.GetTestSettings().Timeout, _logger, "Something went wrong when Test Engine tried to get App status.").Wait();

            _logger.LogInformation("Successfully finished executing Ready function.");


            return FormulaValue.NewBlank();
        }
    }
}

