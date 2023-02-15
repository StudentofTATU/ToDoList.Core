//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using ToDoList.Core.Api.Models.Assignments;

namespace ToDoList.Core.Api.Services.Foundations.Assignments
{
    public interface IAssignmentService
    {
        ValueTask<Assignment> AddAssignmentAsync(Assignment assignment);
        ValueTask<Assignment> RetrieveAssignmentByIdAsync(Guid assignmentId);
        ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment);
        ValueTask<Assignment> RemoveAssignmentByIdAsync(Guid assignmentId);
    }
}
