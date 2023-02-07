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
        public async Task ShouldRetrieveAssignmentByIdAsync()
        {
            // given
            Guid randomAssignmentId = Guid.NewGuid();
            Guid inputAssignmentId = randomAssignmentId;
            Assignment randomAssignment = CreateRandomAssignment();
            Assignment storageAssignment = randomAssignment;
            Assignment expectedAssignment = storageAssignment.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(inputAssignmentId))
                    .ReturnsAsync(storageAssignment);

            // when
            Assignment actualAssignment =
                await this.assignmentService.RetrieveAssignmentByIdAsync(inputAssignmentId);

            // then
            actualAssignment.Should().BeEquivalentTo(expectedAssignment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}