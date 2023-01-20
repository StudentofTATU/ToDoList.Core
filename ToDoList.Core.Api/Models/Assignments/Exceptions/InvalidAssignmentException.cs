//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments.Exceptions
{
    public class InvalidAssignmentException : Xeption
    {
        public InvalidAssignmentException() : base(message: "Assignment is invalid.")
        { }
    }
}