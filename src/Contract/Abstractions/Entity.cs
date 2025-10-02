namespace Contract.Abstractions;

public class Entity<TKey>
{
    public required TKey Id { get; set; }
}
