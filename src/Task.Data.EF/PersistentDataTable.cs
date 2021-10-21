using Microsoft.EntityFrameworkCore;
using Shared.Core.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TaskManager.Data.EF
{
    public class PersistentDataTable<T> : IDataTable<T> where T : class
    {
        private readonly DbSet<T> _data;
        public PersistentDataTable(DbSet<T> data)
        {
            _data = data;
        }

        public void Add(T item)
        {
            _data.Add(item);
        }

        public void Remove(T item)
        {
            _data.Remove(item);
        }

        public IQueryable<T> AsNoTracking()
        {
            return _data.AsQueryable();
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
