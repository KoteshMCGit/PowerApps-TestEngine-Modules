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
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace testengine.module
{
    [Export(typeof(ITestEngineModule))]
    public class BrowserLocaleModule : ITestEngineModule
    {
        public void ExtendBrowserContextOptions(BrowserNewContextOptions options, TestSettings settings)
        {
            if ( settings != null && settings.ExtensionModuleParameters != null && settings.ExtensionModuleParameters.ContainsKey("browserLocale") ) {
                options.Locale = settings.ExtensionModuleParameters["browserLocale"];
            }
        }

        public void RegisterPowerFxFunction(PowerFxConfig config, ITestInfraFunctions testInfraFunctions, IPowerAppFunctions powerAppFunctions, ISingleTestInstanceState singleTestInstanceState, ITestState testState, IFileSystem fileSystem)
        {
        }

        public async Task RegisterNetworkRoute(ITestState testState, ISingleTestInstanceState singleTestInstanceState, IFileSystem fileSystem, IPage page, NetworkRequestMock mock)
        {
            await Task.CompletedTask;
        }
    }
}