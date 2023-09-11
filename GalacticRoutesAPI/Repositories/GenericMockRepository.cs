using GalacticRoutesAPI.Models;

namespace GalacticRoutesAPI.Repositories
{
    public class GenericMockRepository<T> : IGenericRepository<T>
        where T : BaseModel
    {
        protected List<T> _mockData { get; }

        public GenericMockRepository()
        {
            _mockData = new List<T>();
        }

        public T Add(T model)
        {
            _mockData.Add(model);
            return model;
        }

        public T Delete(T model)
        {
            _mockData.Remove(model);
            return model;
        }

        public T[] GetAll()
        {
            return _mockData.ToArray();
        }

        public T GetById(object id)
        {
            T? entity = _mockData.FirstOrDefault(t => t.Id == (string)id);
            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            return entity;
        }

        public T Update(T model)
        {
            T entity = GetById(model.Id);
            int index = _mockData.IndexOf(entity);
            _mockData.RemoveAt(index);
            _mockData.Insert(index, model);
            return model;
        }
    }
}
