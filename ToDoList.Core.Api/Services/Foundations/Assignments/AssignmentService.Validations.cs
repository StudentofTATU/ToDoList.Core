//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using ToDoList.Core.Api.Models.Assignments;
using ToDoList.Core.Api.Models.Assignments.Exceptions;

namespace ToDoList.Core.Api.Services.Foundations.Assignments
{
    public partial class AssignmentService
    {
        private void ValidateAssignment(Assignment assignment)
        {
            ValidateAssignmentNotNull(assignment);

            Validate(
                (Rule: IsInvalid(assignment.Id), Parameter: nameof(Assignment.Id)),
                (Rule: IsInvalid(assignment.Title), Parameter: nameof(Assignment.Title)),
                (Rule: IsInvalid(assignment.Description), Parameter: nameof(Assignment.Description)),
                (Rule: IsInvalid(assignment.CreatedDate), Parameter: nameof(Assignment.CreatedDate)),
                (Rule: IsInvalid(assignment.UpdatedDate), Parameter: nameof(Assignment.UpdatedDate)),
                (Rule: IsNotRecent(assignment.CreatedDate), Parameter: nameof(Assignment.CreatedDate)),
                (Rule: IsNotSame(
                    firstDate: assignment.CreatedDate,
                    secondDate: assignment.UpdatedDate,
                    secondDateName: nameof(Assignment.UpdatedDate)),

                Parameter: nameof(Assignment.CreatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTime();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void ValidateAssignmentNotNull(Assignment assignment)
        {
            if (assignment is null)
            {
                throw new NullAssignmentException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            InvalidAssingmentException invalidAssingmentException = new InvalidAssingmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAssingmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAssingmentException.ThrowIfContainsErrors();
        }
    }
}