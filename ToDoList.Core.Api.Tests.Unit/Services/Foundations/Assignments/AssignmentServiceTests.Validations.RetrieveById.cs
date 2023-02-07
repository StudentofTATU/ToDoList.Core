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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidAssignmentId = Guid.Empty;

            var invalidAssignmentException = new InvalidAssignmentException();

            invalidAssignmentException.AddData(
                key: nameof(Assignment.Id),
                values: "Id is required");

            var expectedAssignmentValidationException = new
                AssignmentValidationException(invalidAssignmentException);

            // when
            ValueTask<Assignment> retrieveAssignmentByIdTask =
                this.assignmentService.RetrieveAssignmentByIdAsync(invalidAssignmentId);

            AssignmentValidationException actualAssignmentValidationException =
                await Assert.ThrowsAsync<AssignmentValidationException>(
                    retrieveAssignmentByIdTask.AsTask);

            // then
            actualAssignmentValidationException.Should()
                .BeEquivalentTo(expectedAssignmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfAssignmentIsNotFoundAndLogItAsync()
        {
            //given
            Guid someAssignmentId = Guid.NewGuid();
            Assignment noAssignment = null;

            var notFoundAssignmentException =
                new NotFoundAssignmentException(someAssignmentId);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(notFoundAssignmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noAssignment);

            //when
            ValueTask<Assignment> retrieveAssignmentByIdTask =
                this.assignmentService.RetrieveAssignmentByIdAsync(someAssignmentId);

            AssignmentValidationException actualAssignmentValidationException =
                await Assert.ThrowsAsync<AssignmentValidationException>(
                    retrieveAssignmentByIdTask.AsTask);

            // then
            actualAssignmentValidationException.Should()
                .BeEquivalentTo(expectedAssignmentValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
