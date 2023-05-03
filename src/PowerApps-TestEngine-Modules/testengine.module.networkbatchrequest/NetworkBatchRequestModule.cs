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

namespace testengine.module
{
    [Export(typeof(ITestEngineModule))]
    public class NetworkBatchRequestModule : ITestEngineModule
    {
        public void ExtendBrowserContextOptions(BrowserNewContextOptions options, TestSettings settings)
        {

        }

        public void RegisterPowerFxFunction(PowerFxConfig config, ITestInfraFunctions testInfraFunctions, IPowerAppFunctions powerAppFunctions, ISingleTestInstanceState singleTestInstanceState, ITestState testState, IFileSystem fileSystem)
        {
        }

        public async Task RegisterNetworkRoute(ITestState testState, ISingleTestInstanceState singleTestInstanceState, IFileSystem fileSystem, IPage page, NetworkRequestMock mock)
        {
            if ( !mock.IsExtension )
            {
                return;
            }

            if ( mock.ExtensionProperties == null ) {  
                return;
            }

            if ( !mock.ExtensionProperties.ContainsKey("batchRequestUrl") )
            {
                return;
            }

            var url = mock.RequestURL;

            if (String.IsNullOrEmpty(url))
            {
                return;
            }

            if ( url.StartsWith("http://") )
            {
                singleTestInstanceState.GetLogger().LogError("http:// requests not supported use https://");
                return;
            }

            if (!String.IsNullOrEmpty(url) && !url.StartsWith("http"))
            {
                url = "**" + url;
            }

            await page.RouteAsync(url, async route => await RouteNetworkRequest(route, testState, fileSystem, singleTestInstanceState, mock));
        }

        public async Task RouteNetworkRequest(IRoute route, ITestState testState, IFileSystem fileSystem, ISingleTestInstanceState singleTestInstanceState, NetworkRequestMock mock)
        {
            // For optional properties of NetworkRequestMock, if the property is not specified, 
            // the routing applies to all. Ex: If Method is null, we mock response whatever the method is.
            bool notMatch = false;

            if (!string.IsNullOrEmpty(mock.Method))
            {
                notMatch = !string.Equals(mock.Method, route.Request.Method);
            }

            if (!string.IsNullOrEmpty(mock.RequestBodyFile))
            {
                notMatch = notMatch || !string.Equals(route.Request.PostData, fileSystem.ReadAllText(mock.RequestBodyFile));
            }

            if (mock.Headers != null && mock.Headers.Count != 0)
            {
                foreach (var header in mock.Headers)
                {
                    var requestHeaderValue = await route.Request.HeaderValueAsync(header.Key);
                    notMatch = notMatch || !string.Equals(header.Value, requestHeaderValue) || !Regex.IsMatch(header.Value, requestHeaderValue);
                }
            }

            if (!string.IsNullOrEmpty(mock.ExtensionProperties["batchRequestUrl"]))
            {
                if (!string.IsNullOrEmpty(route.Request.PostData))
                {
                    using (var reader = new StringReader(route.Request.PostData))
                    {
                        bool found = false;
                        while (reader.Peek() > 0 && !found)
                        {
                            var line = reader.ReadLine();
                            if (line.StartsWith("GET ") && line.Contains(mock.ExtensionProperties["batchRequestUrl"]))
                            {
                                singleTestInstanceState.GetLogger().LogTrace("Route replace " + line);
                                found = true;
                            }
                        }
                        notMatch = notMatch || !found;
                    }
                }
                else
                {
                    notMatch = true;
                }
            }

            if (!notMatch)
            {
                if (!string.IsNullOrEmpty(mock.ExtensionProperties["batchRequestUrl"]))
                {
                    var body = fileSystem.ReadAllText(GetFullFile(testState, mock.ResponseDataFile));
                    var request = route.Request.PostData;

                    using (var reader = new StringReader(request))
                    {
                        var batchId = await reader.ReadLineAsync();
                        var batchResponseId = batchId.Replace("--batch_", "--batchresponse_");
                        body = batchResponseId + "\r\n" + body + "\r\n" + batchResponseId + "--";
                        singleTestInstanceState.GetLogger().LogTrace(route.Request.Url);
                        await route.FulfillAsync(new RouteFulfillOptions { Body = body });
                    }
                }
                else
                {
                    singleTestInstanceState.GetLogger().LogTrace("Replace " + route.Request.Url + " with " + Path.GetFileName(mock.ResponseDataFile));
                    await route.FulfillAsync(new RouteFulfillOptions { Path = GetFullFile(testState, mock.ResponseDataFile) });
                }
            }
            else
            {
                await route.ContinueAsync();
            }
        }

        private string GetFullFile(ITestState testState, string filename)
        {
            var testResultDirectory = Path.GetDirectoryName(testState.GetTestConfigFile().FullName);
            if (!Path.IsPathRooted(filename))
            {
                filename = Path.Combine(testResultDirectory, filename);
            }
            return filename;
        }
    }
}