using System;

namespace Ef.Cosmos
{
    public interface IEntity
    {
        Guid EntityId { get; }
    }
}
