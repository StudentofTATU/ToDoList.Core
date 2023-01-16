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
        public async Task ShouldAddAssignmentAsync()
        {
            // given
            Assignment randomAssignment = CreateRandomAssignment();
            Assignment inputAssignment = randomAssignment;
            Assignment persistAssignment = inputAssignment;
            Assignment expectedAssignment = persistAssignment.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(inputAssignment))
                    .ReturnsAsync(persistAssignment);

            // when
            Assignment assignment =
                await this.assignmentService.AddAssignmentAsync(inputAssignment);

            // then
            assignment.Should().BeEquivalentTo(expectedAssignment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(inputAssignment), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
