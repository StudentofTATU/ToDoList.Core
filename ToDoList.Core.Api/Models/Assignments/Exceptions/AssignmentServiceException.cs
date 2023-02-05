//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments.Exceptions
{
    public class AssignmentServiceException : Xeption
    {
        public AssignmentServiceException(Exception innerException)
            : base(message: "Assignment service error occurred, contact support.", innerException)
        { }
    }
}