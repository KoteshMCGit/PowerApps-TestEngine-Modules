// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.PowerFx;
using System.ComponentModel.Composition;
using Microsoft.Extensions.Logging;
using Microsoft.PowerApps.TestEngine.Config;
using Microsoft.PowerApps.TestEngine.Modules;
using Microsoft.PowerApps.TestEngine.PowerApps;
using Microsoft.PowerApps.TestEngine.System;
using Microsoft.PowerApps.TestEngine.TestInfra;
using Microsoft.Playwright;

namespace testengine.module
{
    [Export(typeof(ITestEngineModule))]
    public class PauseModule : ITestEngineModule
    {
        public void RegisterPowerFxFunction(PowerFxConfig config, ITestInfraFunctions testInfraFunctions, IPowerAppFunctions powerAppFunctions, ISingleTestInstanceState singleTestInstanceState, ITestState testState, IFileSystem fileSystem)
        {
            ILogger logger = singleTestInstanceState.GetLogger();
            config.AddFunction(new PauseFunction(testInfraFunctions, testState, logger));
            logger.LogInformation("Registered Pause()");
        }

        public async Task RegisterNetworkRoute(ISingleTestInstanceState singleTestInstanceState, IFileSystem fileSystem, IPage Page, NetworkRequestMock mock)
        {
            //await Page.RouteAsync(mock.RequestURL, (IRoute route) => route.ContinueAsync(), null);
        }
    }
}