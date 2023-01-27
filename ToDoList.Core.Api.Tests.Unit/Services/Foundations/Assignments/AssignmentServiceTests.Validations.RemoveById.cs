//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Moq;
using ToDoList.Core.Api.Models.Assignments;
using ToDoList.Core.Api.Models.Assignments.Exceptions;
using Xunit;

namespace ToDoList.Core.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidAssignmentId = Guid.Empty;
            var invalidAssignmentException = new InvalidAssignmentException();

            invalidAssignmentException.AddData(
                key: nameof(Assignment.Id),
                values: "Id is required");

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentException);

            // when
            ValueTask<Assignment> removeAssignmentByIdTask =
                this.assignmentService.RemoveAssignmentByIdAsync(invalidAssignmentId);

            AssignmentValidationException actualAssignmentValidationException =
                await Assert.ThrowsAsync<AssignmentValidationException>(removeAssignmentByIdTask.AsTask);

            // then
            actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAsync(It.IsAny<Assignment>()), Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
