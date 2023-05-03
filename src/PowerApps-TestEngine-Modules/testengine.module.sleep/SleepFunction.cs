// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.PowerFx.Types;
using Microsoft.PowerFx;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace testengine.module
{
    /// <summary>
    /// This sleep test execution for the defined time in milliseconds
    /// </summary>
    public class SleepFunction : ReflectionFunction
    {
        private readonly ILogger _logger;

        public SleepFunction(ILogger logger)
            : base("Sleep", FormulaType.Blank, FormulaType.Number)
        {
            _logger = logger;
        }

        public Action<int> ExecuteSleep = (int milliseconds) => Thread.Sleep(milliseconds);

        public BlankValue Execute(NumberValue time)
        {
            _logger.LogInformation("------------------------------\n\n" +
                "Executing Sleep function.");

            ExecuteSleep((int)time.Value);

            _logger.LogInformation("Successfully finished executing Sleep function.");

            return FormulaValue.NewBlank();
        }
    }
}

