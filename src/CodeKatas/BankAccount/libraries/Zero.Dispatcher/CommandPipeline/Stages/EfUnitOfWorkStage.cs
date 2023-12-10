using Zero.DataBase;

namespace Zero.Dispatcher.CommandPipeline.Stages;

public class EfUnitOfWorkStage : IAmAPipelineStage
{
    private readonly IDbContextInterceptor _dbContextInterceptor;

    public EfUnitOfWorkStage(IDbContextInterceptor dbContextInterceptor) 
        => _dbContextInterceptor = dbContextInterceptor;

    public override async Task Process<T>(T command, StageContext context) 
        => await _dbContextInterceptor.Commit();
}