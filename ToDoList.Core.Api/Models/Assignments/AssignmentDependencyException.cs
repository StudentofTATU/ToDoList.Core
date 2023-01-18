//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace ToDoList.Core.Api.Models.Assignments
{
    public class AssignmentDependencyException : Xeption
    {
        public AssignmentDependencyException(Xeption innerException)
            : base(message: "Assignment dependency error occurred, contact support.", innerException)
        { }
    }
}