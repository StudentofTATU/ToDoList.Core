//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments.Exceptions
{
    public class AlreadyExistsAssignmentException : Xeption
    {
        public AlreadyExistsAssignmentException(Exception innerException)
            : base(message: "Assignment already exists.",
                innerException)
        { }
    }
}