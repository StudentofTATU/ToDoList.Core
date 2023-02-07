//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using ToDoList.Core.Api.Models.Assignments;
using ToDoList.Core.Api.Models.Assignments.Exceptions;
using Xunit;

namespace ToDoList.Core.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someAssignmentId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedAssignmentStorageException =
                new FailedAssignmentStorageException(sqlException);

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(failedAssignmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(someAssignmentId))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Assignment> retrieveAssignmentByIdTask =
                this.assignmentService.RetrieveAssignmentByIdAsync(someAssignmentId);

            AssignmentDependencyException actualAssignmentDependencyException =
                await Assert.ThrowsAsync<AssignmentDependencyException>(
                    retrieveAssignmentByIdTask.AsTask);

            //then
            actualAssignmentDependencyException.Should().BeEquivalentTo(
                   expectedAssignmentDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync((It.IsAny<Guid>())), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExpressionAs(
                    expectedAssignmentDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someAssignmentId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAssignmentServiceException =
                new FailedAssignmentServiceException(serviceException);

            var expectedAssignmentServiceException =
                new AssignmentServiceException(failedAssignmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(someAssignmentId))
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<Assignment> retrieveAssignmentByIdTask =
                this.assignmentService.RetrieveAssignmentByIdAsync(someAssignmentId);

            AssignmentServiceException actualAssignmentServiceException =
                 await Assert.ThrowsAsync<AssignmentServiceException>(
                     retrieveAssignmentByIdTask.AsTask);

            //then
            actualAssignmentServiceException.Should()
                .BeEquivalentTo(expectedAssignmentServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
