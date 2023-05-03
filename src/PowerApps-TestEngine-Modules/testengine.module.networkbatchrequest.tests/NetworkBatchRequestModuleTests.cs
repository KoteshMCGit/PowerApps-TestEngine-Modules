using Microsoft.PowerApps.TestEngine.TestInfra;
using Microsoft.PowerFx;
using Microsoft.Playwright;
using Moq;
using Microsoft.PowerApps.TestEngine.Config;
using Microsoft.PowerApps.TestEngine.PowerApps;
using Microsoft.PowerApps.TestEngine.System;

namespace testengine.module.browserlocale.tests
{
    public class NetworkBatchRequestModuleTests
    {
        private Mock<ITestInfraFunctions> MockTestInfraFunctions;
        private Mock<ITestState> MockTestState;
        private Mock<IPowerAppFunctions> MockPowerAppFunctions;
        private Mock<ISingleTestInstanceState> MockSingleTestInstanceState;
        private Mock<IFileSystem> MockFileSystem;
        private Mock<IPage> MockPage;
        private PowerFxConfig TestConfig;
        private NetworkRequestMock TestNetworkRequestMock;

        public NetworkBatchRequestModuleTests()
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

        [Fact]
        public void ExtendBrowserContextOptionsLocaleUpdate()
        {
            // Arrange
            var module = new NetworkBatchRequestModule();
            var options = new BrowserNewContextOptions();
            var settings = new TestSettings() { };

            // Act
            module.ExtendBrowserContextOptions(options, settings);
        }

        [Fact]
        public void RegisterPowerFxFunction()
        {
            // Arrange
            var module = new NetworkBatchRequestModule();

            // Act
            module.RegisterPowerFxFunction(TestConfig, MockTestInfraFunctions.Object, MockPowerAppFunctions.Object, MockSingleTestInstanceState.Object, MockTestState.Object, MockFileSystem.Object);
        }

        [Theory]
        [InlineData(false, null, null, null, false, null)]
        [InlineData(true, null, null, null, false, null)]
        [InlineData(true, "https://localhost.com", null, null, false, null)]
        [InlineData(true, "https://localhost.com", "batchRequestUrl", "test", true, "https://localhost.com")]
        [InlineData(true, "/invoke", "batchRequestUrl", "test", true, "**/invoke")]
        public async Task RegisterNetworkRoute(bool isExtension, string requestUrl, string? property, string propertyValue, bool expectRegistration, string expectedUrl)
        {
            // Arrange
            var module = new NetworkBatchRequestModule();

            if (expectRegistration)
            {
                MockPage.Setup(x => x.RouteAsync(It.IsAny<string>(), It.IsAny<Action<IRoute>>(), null)).Returns(Task.CompletedTask);
            }

            if ( string.IsNullOrEmpty(property) )
            {
                TestNetworkRequestMock.ExtensionProperties = null;
            }

            if (!string.IsNullOrEmpty(property))
            {
                TestNetworkRequestMock.ExtensionProperties = new Dictionary<string, string> { { property, propertyValue } };
            }

            TestNetworkRequestMock.IsExtension = isExtension;
            TestNetworkRequestMock.RequestURL = requestUrl;

            // Act
            await module.RegisterNetworkRoute(MockTestState.Object, MockSingleTestInstanceState.Object, MockFileSystem.Object, MockPage.Object, TestNetworkRequestMock);

            // Assert
            if ( expectRegistration )
            {
                MockPage.Verify(v => v.RouteAsync(expectedUrl, It.IsAny<Action<IRoute>>(), null), Times.Once());
            }
        }
    }
}