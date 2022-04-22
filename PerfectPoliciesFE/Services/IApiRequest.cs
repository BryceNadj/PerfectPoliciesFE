using System.Collections.Generic;

namespace PerfectPoliciesFE.Services
{
    public interface IApiRequest<T>
    {
        public List<T> GetAll(string controllerName);
        public T GetSingle(string controllerName, int id);
        public T Create(string controllerName, T entity);
        public T Edit(string controllerName, T entity, int id);
        public void Delete(string controllerName, int id);
        public List<T> GetAllForParentId(string controllerName, string endpointName, int id);
    }
}
