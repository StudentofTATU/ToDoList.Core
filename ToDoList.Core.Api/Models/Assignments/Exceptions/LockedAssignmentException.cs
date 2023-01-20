//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments.Exceptions
{
    public class LockedAssignmentException : Xeption
    {
        public LockedAssignmentException(Exception innerException)
            : base(message: "Assignment is locked, please try again.", innerException)
        { }
    }
}