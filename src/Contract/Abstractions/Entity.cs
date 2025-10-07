using System;
namespace Contract.Abstractions;


public abstract class Entity<TKey> where TKey : notnull
{
    public TKey Id { get; set; } = default!;
}
public abstract class Entity : Entity<Guid>
{

}
