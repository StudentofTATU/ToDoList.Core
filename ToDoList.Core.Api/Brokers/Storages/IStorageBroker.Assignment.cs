﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using ToDoList.Core.Api.Models.Assignments;

namespace ToDoList.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Assignment> InsertAssignmentAsync(Assignment assignment);
        ValueTask<Assignment> UpdateAssignmentAsync(Assignment assignment);
    }
}
