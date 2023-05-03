using Microsoft.PowerApps.TestEngine.TestInfra;
using Microsoft.PowerFx;
using Microsoft.Playwright;
using Moq;
using Microsoft.PowerApps.TestEngine.Config;
using Microsoft.PowerApps.TestEngine.PowerApps;
using Microsoft.PowerApps.TestEngine.System;
using Microsoft.Extensions.Logging;
using Microsoft.PowerFx.Types;

namespace testengine.module.browserlocale.tests
{
    public class SleepFunctionTests
    {
        private Mock<ITestInfraFunctions> MockTestInfraFunctions;
        private Mock<ITestState> MockTestState;
        private Mock<IPowerAppFunctions> MockPowerAppFunctions;
        private Mock<ISingleTestInstanceState> MockSingleTestInstanceState;
        private Mock<IFileSystem> MockFileSystem;
        private Mock<IPage> MockPage;
        private PowerFxConfig TestConfig;
        private NetworkRequestMock TestNetworkRequestMock;
        private Mock<ILogger> MockLogger;

        public SleepFunctionTests()
        {
            MockTestInfraFunctions = new Mock<ITestInfraFunctions>(MockBehavior.Strict);
            MockTestState = new Mock<ITestState>(MockBehavior.Strict);
            MockPowerAppFunctions = new Mock<IPowerAppFunctions>();
            MockSingleTestInstanceState = new Mock<ISingleTestInstanceState>(MockBehavior.Strict);
            MockFileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            MockPage = new Mock<IPage>(MockBehavior.Strict);
            TestConfig = new PowerFxConfig();
            TestNetworkRequestMock = new NetworkRequestMock();
            MockLogger = new Mock<ILogger>(MockBehavior.Strict);
        }

        [Theory]
        [InlineData(2.0, 2)]
        [InlineData(1.5, 1)]
        public void SleepExecute(double value, int expected)
        {
            // Arrange

            var function = new SleepFunction(MockLogger.Object);

            int? actual = null;

            function.ExecuteSleep = (executeValue) => actual = executeValue;

            MockLogger.Setup(x => x.Log(
               It.IsAny<LogLevel>(),
               It.IsAny<EventId>(),
               It.IsAny<It.IsAnyType>(),
               It.IsAny<Exception>(),
               (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

            // Act
            function.Execute(NumberValue.New(value));

            // Assert
            MockLogger.Verify(l => l.Log(It.Is<LogLevel>(l => l == LogLevel.Information),
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((v, t) => v.ToString() == "------------------------------\n\n" +
                "Executing Sleep function."),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.AtLeastOnce);

            MockLogger.Verify(l => l.Log(It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Successfully finished executing Sleep function."),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.AtLeastOnce);

            Assert.Equal(expected, actual);
        }
    }
}