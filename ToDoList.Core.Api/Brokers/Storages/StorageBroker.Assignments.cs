//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Api.Models.Assignments;

namespace ToDoList.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Assignment> Assignments { get; set; }

        public async ValueTask<Assignment> InsertAssignmentAsync(Assignment assignment) =>
            await InsertAsync(assignment);

        public IQueryable<Assignment> SelectAllAssignments() =>
            SelectAll<Assignment>();

        public async ValueTask<Assignment> SelectAssignmentByIdAsync(Guid id) =>
            await SelectAsync<Assignment>(id);

        public async ValueTask<Assignment> UpdateAssignmentAsync(Assignment assignment) =>
            await UpdateAsync(assignment);
    }
}
