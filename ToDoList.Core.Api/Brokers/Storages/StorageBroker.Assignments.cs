﻿//=================================
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
    }
}
