//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

namespace ToDoList.Core.Api.Brokers.Loggings
{
    public class LoggingBroker : ILoggingBroker
    {
        private readonly ILogger<LoggingBroker> logger;
        public LoggingBroker(ILogger<LoggingBroker> logger) =>
            this.logger = logger;

        void ILoggingBroker.LogError(Exception exception) =>
            this.logger.LogError(exception, exception.Message);

        void ILoggingBroker.LogCritical(Exception exception) =>
            this.logger.LogCritical(exception.Message, exception);
    }
}
