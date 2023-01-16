//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Moq;
using ToDoList.Core.Api.Brokers.Storages;
using ToDoList.Core.Api.Models.Assignments;
using ToDoList.Core.Api.Services.Foundations.Assignments;
using Tynamix.ObjectFiller;

namespace ToDoList.Core.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IAssignmentService assignmentService;

        public AssignmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.assignmentService = new AssignmentService(
                storageBroker: this.storageBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Assignment CreateRandomAssignment() =>
            CreateAssignmentFiller().Create();

        private static Filler<Assignment> CreateAssignmentFiller()
        {
            var filler = new Filler<Assignment>();
            DateTimeOffset dates = GetRandomDateTime();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
