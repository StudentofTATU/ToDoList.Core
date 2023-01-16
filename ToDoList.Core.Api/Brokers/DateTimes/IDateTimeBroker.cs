//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

namespace ToDoList.Core.Api.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        public DateTimeOffset GetCurrnetDateTime();
    }
}