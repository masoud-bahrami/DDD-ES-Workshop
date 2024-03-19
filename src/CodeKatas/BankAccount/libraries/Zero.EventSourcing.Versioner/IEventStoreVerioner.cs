using System.Threading.Tasks;
using Zero.Domain;

namespace Zero.EventSourcing.Versioner;

public interface IEventStoreVerioner
{

    Task CopyAndReplaceEventStream(IsAnIdentity from, IsAnIdentity to, bool deleteOldStream = false);
    Task CopyTransformAndReplaceEventStream(IsAnIdentity from, IsAnIdentity to, bool deleteOldStream);
}