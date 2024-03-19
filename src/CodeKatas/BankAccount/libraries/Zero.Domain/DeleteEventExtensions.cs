namespace Zero.Domain;

public static class DeleteEventExtensions
{
    public static bool IsADeleteDomainEventEvent(this string domainEventQualifiedName)
        => !string.IsNullOrWhiteSpace(domainEventQualifiedName) 
           && domainEventQualifiedName.IsChildOf<DeleteEvent>();
}

public static class TypeExtensions
    {
        public static bool IsOfType(this object obj, Type type)
            => obj != null && obj.GetType() == type;

        public static bool IsOfType<T>(this object obj)
            => obj != null && obj.GetType() == typeof(T);

        public static bool IsTheBaseTypeOf(this Type baseType, Type childType)
            => childType.BaseType == baseType;

        public static bool IsChildOf<TParentType>(this string qualifiedTypeName)
        {
            var type = Type.GetType(qualifiedTypeName);

            if (type == null)
                throw new ResolveTypeException(qualifiedTypeName);

            return type?.BaseType == typeof(TParentType);
        }
    }
