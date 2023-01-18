//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments
{
    public class FailedAssignmentDependencyValidationException : Xeption
    {
        public FailedAssignmentDependencyValidationException(Exception innerException)
            : base(message: "Falied assignment dependency validation error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}