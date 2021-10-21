using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Shared.Core.Persistence
{
    public class NonPersistentDataTable<T> : IDataTable<T> where T : class
    {
        private readonly List<T> _data;

        public NonPersistentDataTable()
        {
            _data = new List<T>();
        }

        public NonPersistentDataTable(List<T> data)
        {
            _data = data;
        }

        public IQueryable<T> AsNoTracking()
        {
            return _data.AsQueryable();
        }

        public void Add(T item)
        {
            _data.Add(item);
        }

        public void Remove(T item)
        {
            _data.Remove(item);
        }

        #region Implementation of IQueryable<Field>
        public Type ElementType
        {
            get
            {
                return _data.AsQueryable().ElementType;
            }
        }

        public Expression Expression
        {
            get
            {
                return _data.AsQueryable().Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _data.AsQueryable().Provider;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.AsQueryable().GetEnumerator();
        }
        #endregion
    }
}
