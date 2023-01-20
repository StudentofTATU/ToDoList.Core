//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Assignment someAssignment = CreateRandomAssignment();
            SqlException sqlException = CreateSqlException();

            var failedAssignmentStorageException =
                new FailedAssignmentStorageException(sqlException);

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(failedAssignmentStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(sqlException);

            // when
            ValueTask<Assignment> addAssigmentTask =
                this.assignmentService.AddAssignmentAsync(someAssignment);

            AssignmentDependencyException actualAssignmentDependencyException =
                await Assert.ThrowsAsync<AssignmentDependencyException>(addAssigmentTask.AsTask);

            // then
            actualAssignmentDependencyException.Should().BeEquivalentTo(expectedAssignmentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExpressionAs(
                    expectedAssignmentDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            Assignment someAssignment = CreateRandomAssignment();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsAssignmentException =
                new AlreadyExistsAssignmentException(duplicateKeyException);

            var expectedAssignmentDependencyValidationException =
                new AssignmentDependencyValidationException(
                    alreadyExistsAssignmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(duplicateKeyException);

            // when
            ValueTask<Assignment> addAssignmentTask =
                this.assignmentService.AddAssignmentAsync(someAssignment);

            AssignmentDependencyValidationException actualAssignmentDependencyValidationException =
                await Assert.ThrowsAsync<AssignmentDependencyValidationException>(addAssignmentTask.AsTask);

            // then
            actualAssignmentDependencyValidationException
                .Should().BeEquivalentTo(expectedAssignmentDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Assignment someAssignment = CreateRandomAssignment();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAssignmentException =
                new LockedAssignmentException(dbUpdateConcurrencyException);

            var expectedAssignmentDependencyValidationException =
                new AssignmentDependencyValidationException(lockedAssignmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<Assignment> addAssignmentTask =
                this.assignmentService.AddAssignmentAsync(someAssignment);

            AssignmentDependencyValidationException actualAssignmentDependencyValidationException =
                await Assert.ThrowsAsync<AssignmentDependencyValidationException>(addAssignmentTask.AsTask);

            // then
            actualAssignmentDependencyValidationException.Should()
                .BeEquivalentTo(expectedAssignmentDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Assignment someAssigment = CreateRandomAssignment();
            var serviceException = new Exception();

            var failedAssignmentServiceException =
                new FailedAssignmentServiceException(serviceException);

            var expectedAssignmentServiceException =
                new AssignmentServiceException(failedAssignmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Assignment> addAssignmentTask =
                this.assignmentService.AddAssignmentAsync(someAssigment);

            AssignmentServiceException actualAssignmentServiceException =
                await Assert.ThrowsAsync<AssignmentServiceException>(addAssignmentTask.AsTask);

            // then
            actualAssignmentServiceException.Should().BeEquivalentTo(expectedAssignmentServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}