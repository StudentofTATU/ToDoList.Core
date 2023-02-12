//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using ToDoList.Core.Api.Brokers.DateTimes;
using ToDoList.Core.Api.Brokers.Loggings;
using ToDoList.Core.Api.Brokers.Storages;
using ToDoList.Core.Api.Models.Assignments;

namespace ToDoList.Core.Api.Services.Foundations.Assignments
{
    public partial class AssignmentService : IAssignmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AssignmentService(IStorageBroker storageBroker, ILoggingBroker loggingBroker, IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Assignment> AddAssignmentAsync(Assignment assignment) =>
        TryCatch(async () =>
        {
            ValidateAssignment(assignment);

            return await this.storageBroker.InsertAssignmentAsync(assignment);
        });

        public ValueTask<Assignment> RetrieveAssignmentByIdAsync(Guid assignmentId) =>
        TryCatch(async () =>
        {
            ValidateAssignmentId(assignmentId);

            Assignment maybeAssignment =
                await this.storageBroker.SelectAssignmentByIdAsync(assignmentId);

            ValidateStorageAssignmentExists(maybeAssignment, assignmentId);

            return maybeAssignment;
        });

        public ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment) =>
        TryCatch(async () =>
        {
            ValidateAssignmentOnModify(assignment);

            return await this.storageBroker.UpdateAssignmentAsync(assignment);
        });

        public ValueTask<Assignment> RemoveAssignmentByIdAsync(Guid assignmentId) =>
        TryCatch(async () =>
        {
            ValidateAssignmentId(assignmentId);

            Assignment maybeAssignment =
                await this.storageBroker.SelectAssignmentByIdAsync(assignmentId);

            ValidateStorageAssignmentExists(maybeAssignment, assignmentId);

            return await this.storageBroker.DeleteAssignmentAsync(maybeAssignment);
        });
    }
}
