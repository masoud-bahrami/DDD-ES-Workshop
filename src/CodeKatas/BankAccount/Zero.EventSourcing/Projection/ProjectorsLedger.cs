using System.Reflection;

namespace Zero.EventSourcing.Projection
{
    public class ProjectorsLedger : IProjectorsLedger
    {
        private readonly Assembly _assembly;
        private readonly Dictionary<Type, List<Type>> _result;
        public ProjectorsLedger(Assembly assembly)
        {
            _assembly = assembly;
            _result = new Dictionary<Type, List<Type>>();
            Resolve();
        }

        protected virtual void Resolve()
        {

            var projectorTypes = _assembly.ResolveChildrenOf(typeof(ImAProjector));

            foreach (var projectorType in projectorTypes)
            {
                List<Type> types = projectorType.InterestIn();

                foreach (var type in types)
                {
                    if (_result.TryGetValue(type, out List<Type> projectors))
                    {
                        projectors.Add(projectorType);
                    }
                    else
                        _result[type] = new List<Type> { projectorType };
                }
            }
        }

        public List<Type> WhoAreInterestedIn(Type type)
        {
            if (_result.TryGetValue(type, out List<Type> result))
                return result;

            return EmptyLitOfTypes();
        }

        private List<Type> EmptyLitOfTypes() => new List<Type>();
    }
}