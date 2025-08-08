using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagement.Domain.Interfaces;

namespace WarehouseManagement.Infrastructure.Data.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction; // состояние текущей операции с нашей БД

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction == null)
                    _transaction = await _context.Database.BeginTransactionAsync();

                int result = await _context.SaveChangesAsync(cancellationToken);

                await _transaction.CommitAsync();
                return result;
            }
            catch
            {
                await RollbackAsync(); // откатываемся при ошибки фиксации транзакции
                throw;
            }
            finally
            {
                _transaction?.Dispose(); // в любом случае нам необходимо чистить за собой
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if(_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
                _transaction = null;
            }

            _context.ChangeTracker.Clear();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
