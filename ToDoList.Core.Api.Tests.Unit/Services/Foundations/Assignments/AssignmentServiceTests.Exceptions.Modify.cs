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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Assignment randomAssignment = CreateRandomAssignment();
            Assignment someAssignment = randomAssignment;
            Guid assignmentId = someAssignment.Id;
            SqlException sqlException = CreateSqlException();

            var failedAssignmentStorageException =
                new FailedAssignmentStorageException(sqlException);

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(failedAssignmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(assignmentId))
                    .Throws(sqlException);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(someAssignment);

            AssignmentDependencyException actualAssignmentDependencyException =
                await Assert.ThrowsAsync<AssignmentDependencyException>(
                     modifyAssignmentTask.AsTask);

            // then
            actualAssignmentDependencyException.Should().BeEquivalentTo(
                expectedAssignmentDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExpressionAs(
                    expectedAssignmentDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(assignmentId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAssignmentAsync(someAssignment), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
