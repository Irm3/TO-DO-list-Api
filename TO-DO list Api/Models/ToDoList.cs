namespace TO_DO_list_Api.Models
{
    public class ToDoList
    {
        private int TodoList_id { get; set; }
        private string Name { get; set; } = null!;
        private bool Status { get; set; }
        public virtual User user { get; set; } = null!;
    }
}
