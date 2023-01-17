//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments
{
    public class AssignmentValidationException
        : Xeption
    {
        public AssignmentValidationException(Xeption innerException)
            : base(message: "Assignment error occured, fix the errors and try again.", innerException)
        { }
    }
}