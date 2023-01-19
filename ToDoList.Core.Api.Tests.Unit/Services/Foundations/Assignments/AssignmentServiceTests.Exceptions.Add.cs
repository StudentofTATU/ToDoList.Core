﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using ToDoList.Core.Api.Models.Assignments;
using ToDoList.Core.Api.Models.Assignments.Exceptions;
using Xunit;

namespace ToDoList.Core.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Assignment someAssignment = CreateRandomAssignment();
            SqlException sqlException = CreateSqlException();

            var failedAssignmentStorageException =
                new FailedAssignmentStorageException(sqlException);

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(failedAssignmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>())).ThrowsAsync(sqlException);

            // when
            ValueTask<Assignment> addAssigmentTask =
                this.assignmentService.AddAssignmentAsync(someAssignment);

            AssignmentDependencyException actualAssignmentDependencyException =
                await Assert.ThrowsAsync<AssignmentDependencyException>(addAssigmentTask.AsTask);

            // then
            actualAssignmentDependencyException.Should().BeEquivalentTo(expectedAssignmentDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExpressionAs(
                    expectedAssignmentDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            Assignment someAssignment = CreateRandomAssignment();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsAssignmentException =
                new AlreadyExistsAssignmentException(duplicateKeyException);

            var expectedAssignmentDependencyValidationException =
                new AssignmentDependencyValidationException(
                    alreadyExistsAssignmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()))
                    .ThrowsAsync(duplicateKeyException);
            // when
            ValueTask<Assignment> addAssignmentTask =
                this.assignmentService.AddAssignmentAsync(someAssignment);

            AssignmentDependencyValidationException assignmentDependencyValidationException =
                await Assert.ThrowsAsync<AssignmentDependencyValidationException>(addAssignmentTask.AsTask);

            // then
            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedAssignmentDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}