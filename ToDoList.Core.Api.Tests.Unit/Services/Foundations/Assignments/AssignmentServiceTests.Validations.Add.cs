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
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Assignment noAssignment = null;
            var nullAssignmentException = new NullAssignmentException();

            var expectedAssignmentValidationException =
                new AssignmentValidationException(nullAssignmentException);

            // when
            ValueTask<Assignment> addAssignmentTask =
                this.assignmentService.AddAssignmentAsync(noAssignment);

            AssignmentValidationException actualAssignmentValidationException =
                await Assert.ThrowsAsync<AssignmentValidationException>(addAssignmentTask.AsTask);

            // then
            actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfAssignmentIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
            var invalidAssignment = new Assignment
            {
                Title = invalidString
            };

            var invalidAssingmentException = new InvalidAssingmentException();

            invalidAssingmentException.AddData(
                key: nameof(Assignment.Id),
                values: "Id is required");

            invalidAssingmentException.AddData(
                key: nameof(Assignment.Title),
                values: "Text is required");

            invalidAssingmentException.AddData(
               key: nameof(Assignment.Description),
               values: "Text is required");

            invalidAssingmentException.AddData(
               key: nameof(Assignment.CreatedDate),
               values: "Value is required");

            invalidAssingmentException.AddData(
                key: nameof(Assignment.UpdatedDate),
                values: "Value is required");

            AssignmentValidationException expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssingmentException);

            // when
            ValueTask<Assignment> addAssginmentTask =
                this.assignmentService.AddAssignmentAsync(invalidAssignment);

            AssignmentValidationException actualAssignmentValidationException =
                await Assert.ThrowsAsync<AssignmentValidationException>(addAssginmentTask.AsTask);

            // then
            actualAssignmentValidationException.Should().BeEquivalentTo(expectedAssignmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset anotherRandomDate = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(randomDateTime);
            Assignment invalidAssignment = randomAssignment;
            randomAssignment.UpdatedDate = anotherRandomDate;
            var invalidAssingmentException = new InvalidAssingmentException();

            invalidAssingmentException.AddData(
                key: nameof(Assignment.CreatedDate),
                values: $"Date is not same as {nameof(Assignment.UpdatedDate)}");

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssingmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Assignment> addAssignmentTask =
                this.assignmentService.AddAssignmentAsync(invalidAssignment);

            AssignmentValidationException actualAssignmentValidationException =
                await Assert.ThrowsAsync<AssignmentValidationException>(addAssignmentTask.AsTask);

            // then
            actualAssignmentValidationException.Should()
                .BeEquivalentTo(expectedAssignmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(int invalidSeconds)
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset invalidRandomDateTime = randomDateTime.AddSeconds(invalidSeconds);
            Assignment randomInvalidAssignment = CreateRandomAssignment(invalidRandomDateTime);
            Assignment invalidAssignment = randomInvalidAssignment;

            var invalidAssingmentException = new InvalidAssingmentException();

            invalidAssingmentException.AddData(
                key: nameof(Assignment.CreatedDate),
                values: "Date is not recent"
                );

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssingmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Assignment> addAssignmentTask =
                this.assignmentService.AddAssignmentAsync(invalidAssignment);

            AssignmentValidationException actualAssignmentValidationException =
                await Assert.ThrowsAsync<AssignmentValidationException>(addAssignmentTask.AsTask);

            // then
            actualAssignmentValidationException.Should()
                .BeEquivalentTo(expectedAssignmentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
