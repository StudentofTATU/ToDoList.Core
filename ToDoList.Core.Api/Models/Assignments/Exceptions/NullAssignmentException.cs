//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments.Exceptions
{
    public class NullAssignmentException : Xeption
    {
        public NullAssignmentException()
            : base(message: "Assignment is null.")
        { }
    }
}