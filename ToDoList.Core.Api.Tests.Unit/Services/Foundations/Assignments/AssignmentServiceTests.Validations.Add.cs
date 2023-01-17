//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Moq;
using ToDoList.Core.Api.Models.Assignments;
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
    }
}
