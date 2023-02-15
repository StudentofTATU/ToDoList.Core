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
        public async Task ShouldModifyAssignmentAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(randomDateTime);
            Assignment inputAssignment = randomAssignment;
            Assignment storageAssignment = inputAssignment.DeepClone();
            Assignment updateAssignment = inputAssignment;
            Assignment expectedAssignment = updateAssignment.DeepClone();
            Guid assignmentId = inputAssignment.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(assignmentId))
                    .ReturnsAsync(storageAssignment);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAssignmentAsync(inputAssignment))
                    .ReturnsAsync(updateAssignment);
            // when
            Assignment actualAssignment =
                await this.assignmentService.ModifyAssignmentAsync(inputAssignment);

            // then
            actualAssignment.Should().BeEquivalentTo(expectedAssignment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(assignmentId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAssignmentAsync(inputAssignment), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
