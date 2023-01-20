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
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(randomDateTime);
            Assignment inputAssignment = randomAssignment;
            Assignment persistAssignment = inputAssignment;
            Assignment expectedAssignment = persistAssignment.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(inputAssignment))
                    .ReturnsAsync(persistAssignment);

            // when
            Assignment assignment =
                await this.assignmentService.AddAssignmentAsync(inputAssignment);

            // then
            assignment.Should().BeEquivalentTo(expectedAssignment);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(inputAssignment), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
