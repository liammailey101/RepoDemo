using System.Collections.Concurrent;
using AutoMapper;
using RepoDemo.Common;
using Microsoft.EntityFrameworkCore;

namespace RepoDemo.GenericRepository.Repository
{
    public class UnitOfWork(DbContext context, ITelemetryClient telemetryClient, IMapper mapper) : IUnitOfWork
    {
        private readonly DbContext _context = context;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();
        private readonly ITelemetryClient _telemetryClient = telemetryClient;
        private readonly IMapper _mapper = mapper;
        private bool _disposed = false;

        public IGenericRepository<T> Repository<T>() where T : class
        {
            try
            {
                return (IGenericRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new GenericRepository<T>(_context, _telemetryClient, _mapper));
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                throw new RepositoryException($"An error occurred while getting the {typeof(T).Name}Repository.", ex);
            }
        }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _telemetryClient.TrackException(ex);
                throw new RepositoryException("Concurrency conflict while saving.", ex);
            }
            catch (DbUpdateException ex)
            {
                _telemetryClient.TrackException(ex);
                throw new RepositoryException("Database error while saving.", ex);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                throw new RepositoryException("Error saving changes.", ex);
            }
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await action();
                await CompleteAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _telemetryClient.TrackException(ex);
                throw new RepositoryException("Transaction failed and was rolled back.", ex);

            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}