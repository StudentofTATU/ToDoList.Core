//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments.Exceptions
{
    public class NotFoundAssignmentException : Xeption
    {
        public NotFoundAssignmentException(Guid assignmentId)
           : base(message: $"Couldn't find assignment with id: {assignmentId}.")
        { }
    }
}
