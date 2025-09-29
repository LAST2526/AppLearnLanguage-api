using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Last02.Commons;
using Last02.Data.UnitOfWork;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Last02.Services.Implement
{
    public class BaseService : DisposableObject, IBaseService
    {
        protected IConfiguration? _configuration;
        protected IHttpContextAccessor? _httpContextAccessor;
        protected IUnitOfWork? UnitOfWork { get; set; }

        private BaseService()
        {

        }

        public static SuccessResponse SuccessResponse()
        {
            return new SuccessResponse();
        }

        public static SuccessResponse SuccessResponse(dynamic data)
        {
            return new SuccessResponse(data);
        }

        protected BaseService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        protected BaseService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.UnitOfWork = unitOfWork;
            this._httpContextAccessor = httpContextAccessor;
        }

        protected BaseService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            UnitOfWork = unitOfWork;
            this._configuration = configuration;
        }

        #region Dispose
        private bool _disposed;

        public override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    UnitOfWork = null;
                }
                _disposed = true;
            }
        }
        #endregion

        public IQueryable<T>? OrderByDynamic<T>(IQueryable<T> source, string propertyName, bool ascending)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            var result = typeof(Queryable).GetMethods()
                .First(method => method.Name == methodName
                                 && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, [source, lambda]);
            if (result == null)
            {
                return null;
            }
            return (IQueryable<T>)result;
        }
    }
}
