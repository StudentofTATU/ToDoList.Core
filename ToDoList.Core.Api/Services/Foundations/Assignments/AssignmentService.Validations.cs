//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using ToDoList.Core.Api.Models.Assignments;

namespace ToDoList.Core.Api.Services.Foundations.Assignments
{
    public partial class AssignmentService
    {
        private static void ValidateAssignmentNotNull(Assignment assignment)
        {
            if (assignment is null)
            {
                throw new NullAssignmentException();
            }
        }
    }
}