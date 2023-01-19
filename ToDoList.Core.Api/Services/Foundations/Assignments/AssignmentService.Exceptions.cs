//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using ToDoList.Core.Api.Models.Assignments;
using ToDoList.Core.Api.Models.Assignments.Exceptions;
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
            catch (SqlException sqlException)
            {
                var failedAssignmentStorageException =
                    new FailedAssignmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAssignmentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAssignmentException =
                    new AlreadyExistsAssignmentException(duplicateKeyException);

                throw CreateAndDependencyValidationException(alreadyExistsAssignmentException);
            }
        }

        private AssignmentValidationException CreateAndLogValidationException(Xeption exception)
        {
            var assignmentValidationException = new AssignmentValidationException(exception);
            this.loggingBroker.LogError(assignmentValidationException);

            return assignmentValidationException;
        }

        private AssignmentDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var assignmentDependencyException = new AssignmentDependencyException(exception);
            this.loggingBroker.LogCritical(assignmentDependencyException);

            return assignmentDependencyException;
        }

        private AssignmentDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var assignmentDependencyValidationException =
                new AssignmentDependencyValidationException(exception);

            this.loggingBroker.LogError(assignmentDependencyValidationException);

            return assignmentDependencyValidationException;
        }
    }
}