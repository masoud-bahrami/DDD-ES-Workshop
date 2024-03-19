//using System.Linq;
//using System.Reflection;
//using Microsoft.Extensions.DependencyInjection;
//using Quantum.Configurator;
//using Quantum.EventSourcing.Projection;
//using Quantum.EventSourcing.Subscriber;

//namespace Quantum.EventSourcing.SqlServerProjector
//{
//    public static class ConfigureProjectorsExtensions
//    {
//        public static ConfigureProjectorsBuilder ConfigureProjectors(this QuantumServiceCollection collection)
//            => new(collection);
//    }

//    public class ConfigureProjectorsBuilder
//    {
//        public readonly QuantumServiceCollection _quantumServiceCollection;
//        private Assembly _assembly;
//        private Assembly _ledgerAssembly;

//        public ConfigureProjectorsBuilder(QuantumServiceCollection quantumServiceCollection)
//            => _quantumServiceCollection = quantumServiceCollection;

//        public ConfigureProjectorsBuilder RegisterProjectorsInAssembly(Assembly assembly)
//        {
//            RegisterProjectors(assembly);
//            _assembly = assembly;
//            return this;
//        }

//        private void RegisterProjectors(Assembly assembly)
//        {
//            foreach (var type in assembly.GetTypes().Where(t => t.BaseType == typeof(ImAProjector)))
//            {
//                _quantumServiceCollection.Collection.AddScoped(type);
//            }
//        }

//        public ConfigureProjectorsBuilder FillLedgerUsing(Assembly assembly)
//        {
//            _ledgerAssembly = assembly;

//            IProjectorsLedger ledger = new ProjectorsLedger(assembly);
//            _quantumServiceCollection.Collection.AddSingleton(ledger);
//            return this;
//        }

//        public ConfigureProjectorsBuilder AddDocumentStore<T>(ServiceLifetime serviceLifetime)
//            where T : class, IDocumentStore
//        {
//            _quantumServiceCollection.Collection.Add(new ServiceDescriptor(typeof(IDocumentStore), typeof(T), serviceLifetime));

//            return this;
//        }
        
//        public ConfigureProjectorsBuilder AddCatchupSubscriberAsSingleton<T>()
//            where T : class, ICatchUpSubscriber
//        {
//            _quantumServiceCollection.Collection.AddSingleton<ICatchUpSubscriber, T>();
//            return this;
//        }

//        public ConfigureProjectorsBuilder RegisterDeDuplicator<T>(ServiceLifetime serviceLifetime)
//            where T : class, IDeDuplicator
//        {
//            _quantumServiceCollection.Collection.Add(new ServiceDescriptor(typeof(IDeDuplicator), typeof(T), serviceLifetime));
//            return this;
//        }

//        public QuantumServiceCollection and()
//            => _quantumServiceCollection;
//    }
//}