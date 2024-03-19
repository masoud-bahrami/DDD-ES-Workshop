using System;
using System.Reflection;

namespace Zero.EventSourcing.Versioner;

public interface IEventTransformerRegistrar
{
    IEventTransformer<T> GetTransformerOf<T>(T eventType);
    object GetTransformerOf(Type eventType);
    void Register(Type type, Type domainEventTransformerType);
    void Register(Assembly[] assemblies);
    void Register(Assembly assembly);
}


//public class EventTransformerRegistrar : IEventTransformerRegistrar
//{
//    private readonly Dictionary<Type, Type> _transformersDictionary;
//    private readonly IResolver _resolver;

//    public EventTransformerRegistrar(IResolver resolver)
//    {
//        _resolver = resolver;
//        _transformersDictionary = new();
//    }

//    public IEventTransformer<T> GetTransformerOf<T>(T eventType)
//    {
//        if (_transformersDictionary.TryGetValue(eventType.GetType(), out Type transformerType))
//        {
//            return _resolver.Resolve(transformerType) as IEventTransformer<T>;
//        }

//        throw new ArgumentOutOfRangeException($"GetTransformerOf {eventType.GetType()}");
//    }

//    public object GetTransformerOf(Type eventType)
//    {
//        if (_transformersDictionary.TryGetValue(eventType, out Type transformerType))
//        {
//            return _resolver.Resolve(transformerType);
//        }

//        throw new ArgumentOutOfRangeException($"GetTransformerOf {eventType}");
//    }

//    public void Register(Type type, Type domainEventTransformerType)
//        => _transformersDictionary[type] = domainEventTransformerType;
//}