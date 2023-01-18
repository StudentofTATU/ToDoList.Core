//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments
{
    public class FailedAssignmentStorageException : Xeption
    {
        public FailedAssignmentStorageException(Exception innerException)
            : base(message: "Failed Assignment error occurred, contact support.", innerException)
        { }
    }
}
