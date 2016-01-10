namespace Logbook.Shared.Entities.Budgets
{
    public class Budget : AggregateRoot
    {
        public string Name { get; set; }
        public string ForUserId { get; set; }
    }
}