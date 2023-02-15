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
        public async Task ShouldThrowValidationExceptionOnModifyIfOrderIsNullAndLogItAsync()
        {
            // given 
            Assignment nullAssignment = null;
            var nullAssignmentException = new NullAssignmentException();

            var expectedAssignmentValidationException =
                new AssignmentValidationException(nullAssignmentException);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(nullAssignment);

            AssignmentValidationException actualAssignmentValidationException =
                await Assert.ThrowsAsync<AssignmentValidationException>(
                    modifyAssignmentTask.AsTask);

            // then
            actualAssignmentValidationException.Should()
                .BeEquivalentTo(expectedAssignmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAssignmentAsync(It.IsAny<Assignment>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfAssignmentIsInvalidAndLogItAsync(string invalidString)
        {
            // given
            var invalidAssignment = new Assignment
            {
                Title = invalidString
            };

            var invalidAssignmentException = new InvalidAssignmentException();

            invalidAssignmentException.AddData(
                key: nameof(Assignment.Title),
                values: "Text is required");

            invalidAssignmentException.AddData(
               key: nameof(Assignment.Description),
               values: "Text is required");

            invalidAssignmentException.AddData(
               key: nameof(Assignment.CreatedDate),
               values: "Value is required");

            invalidAssignmentException.AddData(
                key: nameof(Assignment.UpdatedDate),
                values: "Value is required");

            AssignmentValidationException expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentException);

            // when
            ValueTask<Assignment> modifyAssginmentTask =
                this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

            AssignmentValidationException actualAssignmentValidationException =
                await Assert.ThrowsAsync<AssignmentValidationException>(
                    modifyAssginmentTask.AsTask);

            // then
            actualAssignmentValidationException.Should()
                .BeEquivalentTo(expectedAssignmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAssignmentAsync(It.IsAny<Assignment>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
