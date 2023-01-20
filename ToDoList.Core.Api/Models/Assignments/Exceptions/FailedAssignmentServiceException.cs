//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments.Exceptions
{
    public class FailedAssignmentServiceException : Xeption
    {
        public FailedAssignmentServiceException(Exception innerException)
            : base(message: "Failed profile service occurred, please contact support", innerException)
        { }
    }
}