using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Generic.Basic.Linq
{
    internal class OrderedQueryableWrapper<T> : IOrderedQueryable<T>
    {

        #region Private Declaration(s)

        private readonly IQueryable<T> mInnerQuery;

        #endregion

        #region Constructor

        public OrderedQueryableWrapper(IEnumerable<T> items)
        {
            this.mInnerQuery = items.AsQueryable();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in this.mInnerQuery)
            {
                yield return item;
            }
        }

        #endregion

        #region IQueryable Members

        public Expression Expression
        {
            get { return this.mInnerQuery.Expression; }
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public IQueryProvider Provider
        {
            get { return this.mInnerQuery.Provider; }
        }

        #endregion
    }
}
