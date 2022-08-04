using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TO_DO_list_Api.Models
{
    public partial class ToDoList
    {
        public string Name { get; set; } = null!;
        public int TodoListId { get; set; }
        public bool Status { get; set; }
        public int FkUserId { get; set; }

        public virtual User FkUser { get; set; } = null!;
    }
}
