//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================



using FluentAssertions;
using Force.DeepCloner;
using Moq;
using ToDoList.Core.Api.Models.Assignments;
using Xunit;

namespace ToDoList.Core.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveAssignmentByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputAssignmentId = randomId;
            Assignment randomAssignment = CreateRandomAssignment();
            Assignment storageAssignment = randomAssignment;
            Assignment expectedInputAssignment = storageAssignment;
            Assignment deleteAssignment = expectedInputAssignment;
            Assignment expectedAssignment = deleteAssignment.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(inputAssignmentId))
                    .ReturnsAsync(storageAssignment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAssignmentAsync(expectedInputAssignment))
                    .ReturnsAsync(deleteAssignment);

            // when
            Assignment actualAssignment =
                await this.assignmentService.RemoveAssignmentByIdAsync(inputAssignmentId);

            // then
            actualAssignment.Should().BeEquivalentTo(expectedAssignment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAsync(It.IsAny<Assignment>()), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}