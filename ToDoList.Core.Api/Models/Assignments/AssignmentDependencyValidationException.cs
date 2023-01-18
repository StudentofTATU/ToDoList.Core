//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments
{
    public class AssignmentDependencyValidationException : Xeption
    {
        public AssignmentDependencyValidationException(Xeption innerException)
            : base(message: "Assignment dependency validation error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}