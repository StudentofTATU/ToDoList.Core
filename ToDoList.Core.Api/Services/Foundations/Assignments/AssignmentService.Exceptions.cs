//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using ToDoList.Core.Api.Models.Assignments;
using Xeptions;

namespace ToDoList.Core.Api.Services.Foundations.Assignments
{
    public partial class AssignmentService
    {
        private delegate ValueTask<Assignment> ReturningAssignmentFunction();

        private async ValueTask<Assignment> TryCatch(ReturningAssignmentFunction returningAssignmentFunction)
        {
            try
            {
                return await returningAssignmentFunction();
            }
            catch (NullAssignmentException nullAssignmentException)
            {
                throw CreateAndLogValidationException(nullAssignmentException);
            }
            catch (InvalidAssingmentException invalidAssingmentException)
            {
                throw CreateAndLogValidationException(invalidAssingmentException);
            }
        }

        private AssignmentValidationException CreateAndLogValidationException(Xeption exception)
        {
            var assignmentValidationException = new AssignmentValidationException(exception);
            this.loggingBroker.LogError(assignmentValidationException);

            return assignmentValidationException;
        }
    }
}