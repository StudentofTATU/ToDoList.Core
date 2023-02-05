//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using ToDoList.Core.Api.Models.Assignments;
using ToDoList.Core.Api.Models.Assignments.Exceptions;
using Xunit;

namespace ToDoList.Core.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someAssignmentId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAssignmentException =
                new LockedAssignmentException(databaseUpdateConcurrencyException);

            var expectedAssignmentDependencyValidationException =
                new AssignmentDependencyValidationException(lockedAssignmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Assignment> removeAssignmentByIdTask =
                this.assignmentService.RemoveAssignmentByIdAsync(someAssignmentId);

            AssignmentDependencyValidationException actualAssignmentDependencyValidationException =
                await Assert.ThrowsAsync<AssignmentDependencyValidationException>(
                    removeAssignmentByIdTask.AsTask);

            // then
            actualAssignmentDependencyValidationException.Should().BeEquivalentTo(
                expectedAssignmentDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAsync(It.IsAny<Assignment>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid groupId = Guid.NewGuid();
            Guid assignmentId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedAssignmentStorageException =
                new FailedAssignmentStorageException(sqlException);

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(failedAssignmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Assignment> deleteAssignmentTask =
                this.assignmentService.RemoveAssignmentByIdAsync(groupId);

            AssignmentDependencyException actualAssignmentDependencyException =
                await Assert.ThrowsAsync<AssignmentDependencyException>(
                    deleteAssignmentTask.AsTask);

            // then
            actualAssignmentDependencyException.Should().BeEquivalentTo(
                expectedAssignmentDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExpressionAs(
                    expectedAssignmentDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
