using System;

namespace Zero.EventSourcing.SqlServerProjector
{
    [Serializable]
    internal class EntityNotFoundException : Exception
    {
        public Type Type;

        public EntityNotFoundException(Type type)
        : base($"{type} is not found in Db")
        {
            Type = type;
        }

    }
}