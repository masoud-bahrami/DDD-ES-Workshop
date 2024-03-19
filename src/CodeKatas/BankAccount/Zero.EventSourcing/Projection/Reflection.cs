using System.Reflection;
using Zero.Domain;

namespace Zero.EventSourcing.Projection
{
    public static class Reflection
    {

        public static List<Type> ResolveChildrenOfGenericType(this Assembly assembly, Type type)
        {
            var result = new List<Type>();
            for (var i = 0; i < assembly.GetTypes().Length; i++)
            {
                if (assembly.GetTypes()[i].BaseType.GUID == type.GUID)
                    result.Add(assembly.GetTypes()[i]);
            }

            return result;
        }

        public static List<Type> ResolveChildrenOf(this Assembly assembly, Type type)
        {
            var result = new List<Type>();
            for (var i = 0; i < assembly.GetTypes().Length; i++)
            {
                if (assembly.GetTypes()[i].BaseType == type)
                    result.Add(assembly.GetTypes()[i]);
            }

            return result;
        }

        public static List<Type> InterestIn(this Type type)
        {
            var result = new List<Type>();
            var methods = type.GetMethods().Where(m => m.Name == "On"
                                                                            && m.GetParameters().Count() == 1).ToList();
            foreach (var item in methods)
            {
                var domainEventType = item.GetParameters().FirstOrDefault(IsTheBaseTypeIsADomainEvent);
                if (domainEventType != null)
                    result.Add(domainEventType.ParameterType);
            }

            return result;
        }

        private static bool IsTheBaseTypeIsADomainEvent(ParameterInfo parameterInfo)
            => IsBaseTypeIsADomainEvent(parameterInfo.ParameterType);

        private static bool IsBaseTypeIsADomainEvent(Type t)
        {
            if (t == typeof(IsADomainEvent))
                return true;
            return t.BaseType != null && IsBaseTypeIsADomainEvent(t.BaseType);
        }
        public static TOutput InvokeMethod<TOutput>(this Type type, string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException("Method name should not be null or white space");

            var methodInfo = type.GetMethods().FirstOrDefault(m => m.Name == methodName);
            if (methodInfo == null)
                throw new ArgumentNullException($"{type} has not any public method with name {methodName}");

            var typeInstance = Activator.CreateInstance(type);

            var result = methodInfo.Invoke(typeInstance, null);

            if (result is not TOutput)
                throw new ArgumentOutOfRangeException($"Can not convert {result.GetType()} to {typeof(TOutput)}");

            return ((TOutput)result);
        }
    }
}