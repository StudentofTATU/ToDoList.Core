//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using ToDoList.Core.Api.Brokers.Loggings;
using ToDoList.Core.Api.Brokers.Storages;
using ToDoList.Core.Api.Models.Assignments;

namespace ToDoList.Core.Api.Services.Foundations.Assignments
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public AssignmentService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Assignment> AddAssignmentAsync(Assignment assignment) =>
            await this.storageBroker.InsertAssignmentAsync(assignment);
    }
}
