namespace Contract.Abstractions;

public abstract class Entity<TKey>
{
    public TKey Id { get; set; } = default!;
}
