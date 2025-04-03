namespace TodoApiNovo.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsDone { get; set; }

        // FK
        public int UserId { get; set; }
    }
}