using Microsoft.PowerApps.TestEngine.TestInfra;
using Microsoft.PowerFx;
using Microsoft.Playwright;
using Moq;
using Microsoft.PowerApps.TestEngine.Config;
using Microsoft.PowerApps.TestEngine.PowerApps;
using Microsoft.PowerApps.TestEngine.System;

namespace testengine.module.browserlocale.tests
{
    public class BrowserLocaleModuleTest
    {
        private Mock<ITestInfraFunctions> MockTestInfraFunctions;
        private Mock<ITestState> MockTestState;
        private Mock<IPowerAppFunctions> MockPowerAppFunctions;
        private Mock<ISingleTestInstanceState> MockSingleTestInstanceState;
        private Mock<IFileSystem> MockFileSystem;
        private Mock<IPage> MockPage;
        private PowerFxConfig TestConfig;
        private NetworkRequestMock TestNetworkRequestMock;

        public BrowserLocaleModuleTest()
        {
            MockTestInfraFunctions = new Mock<ITestInfraFunctions>(MockBehavior.Strict);
            MockTestState = new Mock<ITestState>(MockBehavior.Strict);
            MockPowerAppFunctions = new Mock<IPowerAppFunctions>();
            MockSingleTestInstanceState = new Mock<ISingleTestInstanceState>(MockBehavior.Strict);
            MockFileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            MockPage = new Mock<IPage>(MockBehavior.Strict);
            TestConfig = new PowerFxConfig();
            TestNetworkRequestMock = new NetworkRequestMock();
        }

        [Theory]
        [InlineData("other", "", null)]
        [InlineData("other", "fr-FR", null)]
        [InlineData("browserLocale", "fr-FR", "fr-FR")]
        public void ExtendBrowserContextOptionsLocaleUpdate(String parameter, String parameterValue, String expected)
        {
            // Arrange
            var module = new BrowserLocaleModule();
            var options = new BrowserNewContextOptions();
            var settings = new TestSettings() { ExtensionModules = new TestSettingExtensions() { Parameters = new Dictionary<string, string> { { parameter, parameterValue } } } };

            // Act
            module.ExtendBrowserContextOptions(options, settings);

            // Assert
            Assert.Equal(expected, options.Locale);
        }

        [Fact]
        public void RegisterPowerFxFunction()
        {
            // Arrange
            var module = new BrowserLocaleModule();

            // Act
            module.RegisterPowerFxFunction(TestConfig, MockTestInfraFunctions.Object, MockPowerAppFunctions.Object, MockSingleTestInstanceState.Object, MockTestState.Object, MockFileSystem.Object);
        }

        [Fact]
        public async Task RegisterNetworkRoute()
        {
            // Arrange
            var module = new BrowserLocaleModule();

            // Act
            await module.RegisterNetworkRoute(MockTestState.Object, MockSingleTestInstanceState.Object, MockFileSystem.Object, MockPage.Object, TestNetworkRequestMock);
        }
    }
}