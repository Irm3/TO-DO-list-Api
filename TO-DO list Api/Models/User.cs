﻿using System;
using System.Collections.Generic;

namespace TO_DO_list_Api.Models
{
    public partial class User
    {
        public User()
        {
            ToDoLists = new HashSet<ToDoList>();
        }

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int UserId { get; set; }

        public virtual ICollection<ToDoList> ToDoLists { get; set; }
    }
}