using System;
using System.Collections.Generic;
using System.Reflection;
using Zero.EventSourcing.Projection;

namespace Zero.EventSourcing.Versioner;

public class EventTransformerRegistrar : IEventTransformerRegistrar
{
    private readonly Dictionary<Type, Type> _transformersDictionary;
    private readonly IServiceProvider _resolver;

    public EventTransformerRegistrar(IServiceProvider resolver)
    {
        _resolver = resolver;
        _transformersDictionary = new();
    }
    public void Register(Type type, Type domainEventTransformerType)
        => _transformersDictionary[type] = domainEventTransformerType;

    public void Register(Dictionary<Type, Type> dictionary)
    {
        foreach (var type in dictionary)
            Register(type.Key, type.Value);
    }

    public void Register(Assembly assembly)
    {
        var projectorTypes 
            = assembly.ResolveChildrenOfGenericType(typeof(IDomainEventTransformer<>));

        foreach (var transformerType in projectorTypes)
        {
            var eventType = transformerType.BaseType.GenericTypeArguments[0];
            Register(eventType, transformerType);
        }
    }

    public void Register(params Assembly[] assembly)
    {
        foreach (var ass in assembly)
        {
            Register(ass);
        }
    }

    public IEventTransformer<T> GetTransformerOf<T>(T eventType)
    {
        if (_transformersDictionary.TryGetValue(eventType.GetType(), out Type transformerType))
        {
            return _resolver.GetService(transformerType) as IEventTransformer<T>;
        }


        throw new ArgumentOutOfRangeException($"GetTransformerOf {eventType.GetType()}");
    }

    public object GetTransformerOf(Type eventType)
    {
        if (_transformersDictionary.TryGetValue(eventType, out var transformerType))
        {
            return _resolver.GetService(transformerType);
        }

        return NullDomainTransformer.New();
    }

}