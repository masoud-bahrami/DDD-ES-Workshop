namespace Zero.Domain
{
    public class ResolveTypeException : Exception
    {
        public string QualifiedTypeName { get; }

        public ResolveTypeException(string qualifiedTypeName) : base($"Can not resolve {qualifiedTypeName}. Check the qualified name!")
            => QualifiedTypeName = qualifiedTypeName;
    }
}