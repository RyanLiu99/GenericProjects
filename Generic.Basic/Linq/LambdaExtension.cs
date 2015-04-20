using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Generic.Basic.Linq
{
    public static partial class LambdaExtension
    {
        private class SortField
        {
            public Expression Expression;
            public bool Descending = false;
            public Type Type;
        }

        public static TElement MaxElement<TElement, TData>(
          this IEnumerable<TElement> source,
          Func<TElement, TData> selector)
          where TData : IComparable<TData>
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");
            Boolean firstElement = true;
            TElement result = default(TElement);
            TData maxValue = default(TData);
            foreach (TElement element in source)
            {
                var candidate = selector(element);
                if (firstElement ||
                (candidate.CompareTo(maxValue) > 0))
                {
                    firstElement = false;
                    maxValue = candidate;
                    result = element;
                }
            }
            return result;
        }



        public static IOrderedQueryable<T> DynamicSort<T>(this IQueryable<T> source, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortOrder))
                throw new ArgumentException("sortOrder is required", "sortOrder");

            var fields = sortOrder.Split(',');
            var sortFields = new List<SortField>();
            //ParameterExpression arg = null; 
            ParameterExpression arg = Expression.Parameter(typeof(T), "x");
            for (int i = 0; i < fields.Length; i++)
            {

                var field = fields[i];
                var sf = new SortField();
                sortFields.Add(sf);
                //Handle Descending Sort Fields             
                if (field.EndsWith(" DESC", StringComparison.OrdinalIgnoreCase))
                {
                    sf.Descending = true;
                    field = field.Substring(0, (field.Length - 5));
                    //Remove " DESC"             
                }
                else if (field.EndsWith(" ASC", StringComparison.OrdinalIgnoreCase))
                {
                    sf.Descending = false; //default asc
                    field = field.Substring(0, (field.Length - 4));
                    //Remove " ASC"             
                }
                
                field = field.Trim();
                //Handle fields that have nested properties            
                var props = field.Split('.');
                sf.Type = typeof(T);
                sf.Expression = arg;// = Expression.Parameter(sf.Type, "x");
                //Create an x parameter of type T              
                foreach (string prop in props)
                {
                    PropertyInfo pi = sf.Type.GetProperty(prop);
                    sf.Expression = Expression.Property(sf.Expression, pi);
                    sf.Type = pi.PropertyType;
                }
            }
            //Now that we have the SortFields we can do the sorting        
            Expression queryExpr = source.Expression;
            string methodAsc = "OrderBy";
            string methodDesc = "OrderByDescending";
            foreach (var sf in sortFields)
            {
                LambdaExpression lambda = Expression.Lambda(sf.Expression, arg);
                queryExpr = Expression.Call(typeof(Queryable), sf.Descending ? methodDesc :
                    methodAsc, new Type[] { source.ElementType, sf.Type },
                    queryExpr, Expression.Quote(lambda));
                methodAsc = "ThenBy";
                methodDesc = "ThenByDescending";
            }
            return (IOrderedQueryable<T>)source.Provider.CreateQuery(queryExpr);
        }


        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            // DataSource control passes the sort parameter with a direction
            // if the direction is descending           
            int descIndex = propertyName.IndexOf(" DESC");
            if (descIndex >= 0)
            {
                propertyName = propertyName.Substring(0, descIndex).Trim();
            }

            if (String.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = (descIndex < 0) ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        public static Func<IEnumerable<T>, IComparable> OrderByProperty<T>(this IEnumerable<T> e, string propertyName)
        {
            var itemType = typeof(T);
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            Expression<Func<IEnumerable<T>, IComparable>> comparer = Expression.Lambda<Func<IEnumerable<T>, IComparable>>(
                Expression.Property(param, propertyName), param);

            return comparer.Compile();
        }

        public static Func<TSource, TKey> OrderByProperty<TSource, TKey>(this IEnumerable<TSource> e, string propertyName)
        {
            var itemType = typeof(TSource);
            ParameterExpression param = Expression.Parameter(typeof(TSource), "p");
            Expression<Func<TSource, TKey>> exp = Expression.Lambda<Func<TSource, TKey>>(
                Expression.Property(param, propertyName), param);

            return exp.Compile();
        }

        public static IOrderedEnumerable<T> OrderByPropertyName<T>(this IEnumerable<T> e, string propertyName)
        {
            var itemType = typeof(T);
            var prop = itemType.GetProperty(propertyName);
            if (prop == null) throw new ArgumentException("Object does not have the specified property");
            var propType = prop.PropertyType;
            var funcType = typeof(Func<,>).MakeGenericType(itemType, propType);
            var parameter = Expression.Parameter(itemType, "item");
            var memberAccess = Expression.MakeMemberAccess(parameter, prop);
            var expression = Expression.Lambda(funcType, memberAccess, parameter);
            var x = typeof(LambdaExtension).GetMethod("InvokeOrderBy",
                BindingFlags.Static | BindingFlags.NonPublic);

            return (IOrderedEnumerable<T>)x.MakeGenericMethod(itemType, propType).Invoke(null, new object[] { e, expression.Compile() });
        }

        static IOrderedEnumerable<T> InvokeOrderBy<T, U>(IEnumerable<T> e, Func<T, U> f)
        {
            return e.OrderBy(f);
        }

        public static IOrderedQueryable<T> AsOrderedQueryable<T>(this IEnumerable<T> items)
        {
            return new OrderedQueryableWrapper<T>(items);
        }

        public static Expression<Func<TEntity, IComparable>> PropertyExpression<TEntity, IComparable>(string PropertyName)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity), "p");
            return Expression.Lambda<Func<TEntity, IComparable>>(Expression.Property(param, PropertyName), param);
        }

        static MemberInfo FindPropertyOrField(Type type, string memberName, bool staticAccess)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly |
                (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
            MemberInfo[] members = type.FindMembers(MemberTypes.Property | MemberTypes.Field,
                flags, Type.FilterNameIgnoreCase, memberName);
            if (members.Length != 0) return members[0];

            return null;
        }

        public static IQueryable<T> AddEqualityCondition<T, V>(this IQueryable<T> queryable,
            string propertyName, V propertyValue)
        {
            ParameterExpression pe = Expression.Parameter(typeof(T), "p");

            IQueryable<T> x = queryable.Where<T>(
              Expression.Lambda<Func<T, bool>>(
                Expression.Equal(Expression.Property(
                  pe,
                  typeof(T).GetProperty(propertyName)),
                  Expression.Constant(propertyValue, typeof(V)),
                  false,
                  typeof(T).GetMethod("op_Equality")),
              new ParameterExpression[] { pe }));

            return (x);
        }

        public static Expression<Func<T, bool>> WhereEqual<T>(string propertyOrFieldName, object propertyOrFieldValue)
        {

            ConstantExpression const1 = Expression.Constant(propertyOrFieldValue);
            ParameterExpression item = Expression.Parameter(typeof(T), "expr");
            MemberExpression prop = LambdaExpression.PropertyOrField(item, propertyOrFieldName);
            BinaryExpression filter = Expression.Equal(prop, const1);
            var expr = Expression.Lambda<Func<T, bool>>(filter, new ParameterExpression[] { item });
            expr.Compile();

            return expr;
        }


        public static Expression<Func<T, K>> CombineErr<T, K>(Expression<Func<T, K>> a, Expression<Func<T, K>> b)
        {
            // This doesn't work
            return Expression.Lambda<Func<T, K>>

                (Expression.And(a.Body, b.Body), a.Parameters);
        }

        public static Expression<Func<T, K>> Combine<T, K>(Expression<Func<T, K>> a, Expression<Func<T, K>> b)
        {

            // This works
            ParameterExpression firstParameter = a.Parameters.First();

            Expression<Func<T, K>> b1 = Expression.Lambda<Func<T, K>>(

            Expression.Invoke(b, firstParameter), firstParameter);

            return Expression.Lambda<Func<T, K>>

                (Expression.And(a.Body, b1.Body), firstParameter);
        }

        public static Expression<Func<TSource, TResult>> GetPropertyOrFieldByName<TSource, TResult>(string propertyOrFieldName)
        {
            ParameterExpression item = Expression.Parameter(typeof(TSource), "expr");
            var pinfo = typeof(TSource).GetProperty(propertyOrFieldName);

            MemberExpression prop = LambdaExpression.PropertyOrField(item, propertyOrFieldName);
            var expr = Expression.Lambda<Func<TSource, TResult>>(prop, new ParameterExpression[] { item });
            expr.Compile();

            return expr;
        }

     

        #region comments
        //public static Expression<Func<TSource, TResult>> Or<T>(this Expression<Func<TSource, TResult>> expr1,
        //                                              Expression<Func<TSource, TResult>> expr2)
        //{
        //    var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        //    return Expression.Lambda<Func<T, TResult>>
        //          (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        //}

        //public static Expression<Func<TSource, TResult>> And<TSource>(this Expression<Func<TSource, TResult>> expr1,
        //                                                     Expression<Func<TSource, TResult>> expr2)
        //{
        //    var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        //    return Expression.Lambda<Func<TSource, TResult>>
        //          (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        //}

        //public static IQueryable<T> Like<T, V>(this IQueryable<T> queryable,
        //    string propertyName, V propertyValue)
        //{
        //    //     var method = typeof(string).GetMethod("Contains");
        //    var method = typeof(Microsoft.VisualBasic.CompilerServices.Operators).GetMethod("LikeString");
        //    Type type = typeof(T);

        //    var pinfo = type.GetProperty(propertyName);
        //    ParameterExpression pe = Expression.Parameter(typeof(T), "p");
        //    var valueExpr = Expression.Constant(propertyValue, propertyValue.GetType());
        //    var callExpr = Expression.MakeMemberAccess(pe, pinfo);
        //    Expression expr = Expression.Call(method, callExpr, valueExpr, Expression.Constant(CompareMethod.Text));
        //    IQueryable<T> x = queryable.Where<T>(
        //        Expression.Lambda<Func<T, bool>>(expr,
        //        new ParameterExpression[] { pe }));

        //    return (x);
        //}

        //public static Expression<Func<TEntity, TKey>> GroupExpression<TEntity, TKey>(IList<DynamicProperty> properties)
        //{
        //    List<Expression> expressions = new List<Expression>();
        //    ParameterExpression p = Expression.Parameter(typeof(TEntity), "");
        //    foreach (DynamicProperty prop in properties)
        //    {
        //        MemberInfo member = FindPropertyOrField(typeof(TEntity), prop.Name, false);
        //        expressions.Add(Expression.Property(p, (PropertyInfo)member));
        //    }
        //    DynamicExpression dynamic = new DynamicExpression();
        //    Type type = dynamic.CreateClass(properties);
        //    MemberBinding[] bindings = new MemberBinding[properties.Count];
        //    for (int i = 0; i < bindings.Length; i++)
        //        bindings[i] = Expression.Bind(type.GetProperty(properties[i].Name), expressions[i]);
        //    Expression exp = Expression.MemberInit(Expression.New(type), bindings);

        //    return Expression.Lambda<Func<TEntity, TKey>>(exp, p);
        //}

        //public static Expression<Func<TEntity, TKey>>[] GroupExpressionArray<TEntity, TKey>(IList<DynamicProperty> properties)
        //{
        //    List<Expression<Func<TEntity, TKey>>> array = new List<Expression<Func<TEntity, TKey>>>();
        //    List<Expression> expressions = new List<Expression>();
        //    ParameterExpression p = Expression.Parameter(typeof(TEntity), "");
        //    foreach (DynamicProperty prop in properties)
        //    {
        //        MemberInfo member = FindPropertyOrField(typeof(TEntity), prop.Name, false);
        //        expressions.Add(Expression.Property(p, (PropertyInfo)member));
        //    }
        //    DynamicExpression dynamic = new DynamicExpression();
        //    Type type = dynamic.CreateClass(properties);
        //    for (int i = 0; i < properties.Count; i++)
        //    {
        //        MemberBinding binding = Expression.Bind(type.GetProperty(properties[i].Name), expressions[i]);
        //        Expression exp = Expression.MemberInit(Expression.New(type), binding);
        //        array.Add(Expression.Lambda<Func<TEntity, TKey>>(exp, p));
        //    }
        //    return array.ToArray();
        //}

        //public static IEnumerable<T> Between<T>(
        //   this IEnumerable<T> source,
        //   Func<T, bool> endPredicate)
        //{


        //    if (source == null) throw Error.ArgumentNull("source");
        //    if (endPredicate == null) throw Error.ArgumentNull("endPredicate");
        //    return BetweenIterator<T>(source, b => true, endPredicate);
        //}


        //public static IEnumerable<T> Between<T>(
        //    this IEnumerable<T> source,
        //    Func<T, bool> startPredicate,
        //    Func<T, bool> endPredicate)
        //{


        //    if (source == null) throw Error.ArgumentNull("source");
        //    if (startPredicate == null) throw Error.ArgumentNull("startPredicate");
        //    if (endPredicate == null) throw Error.ArgumentNull("endPredicate");
        //    return BetweenIterator<T>(source, startPredicate, endPredicate);
        //}


        //static IEnumerable<T> BetweenIterator<T>(
        //    IEnumerable<T> source,
        //    Func<T, bool> startPredicate,
        //    Func<T, bool> endPredicate)
        //{


        //    bool foundStart = false;


        //    foreach (T element in source)
        //    {
        //        if (startPredicate(element))
        //            foundStart = true;


        //        if (foundStart)
        //            if (!endPredicate(element))
        //                yield return element;
        //            else
        //                yield break;
        //    }
        //}


        //public static T FetchOrCreate<T>(this Table<T> table, Expression<Func<T,bool>> where, T newValue) 
        //    where T:class   
        //{      
        //    T existing = table.SingleOrDefault(where);      
        //    if (existing != null)         
        //        return existing;      
        //    // clone the DataContext      
        //    Type dataContextType = table.Context.GetType();      
        //    string ctxConStr = table.Context.Connection.ConnectionString;      
        //    using (DataContext newDC = (DataContext)
        //        Activator.CreateInstance(dataContextType, ctxConStr))      
        //        {         
        //        Table<T> writableTable = newDC.GetTable<T>();         
        //        writableTable.InsertAllOnSubmit(newValue);         
        //        newDC.SubmitChanges();      
        //    }      
        //    return table.Single(where); // fetch on the existing context so the caching behavior is consistent   
        //}

        //  var query = from p in people.AsQueryable()
        //  select p;
        //  query = query.AddEqualityCondition("Name", "Test");..
        //  query = query.AddEqualityCondition("Age", 1);



        //  How to use it? It's simple:
        //  Table_1: some table in my DataContext with columns: Id, Name
        //  Expression<Func<Table_1, string>> le = GetPropertyOrFieldByName<Table_1, string>("Name");
        //  Expression<Func<Table_1, bool>> le1 = WhereEqual<Table_1>("Name", "Dariusz Jankowski");
        //  string result = myDataContext.Table_1s.Where(le1).Select(le); //result = "Dariusz Jankowski";





        //    public static IQueryable<T> AddLessThanCondition<T, V>(this IQueryable<T> queryable,  
        //        string propertyName, V propertyValue)
        //    {
        //        ParameterExpression pe = Expression.Parameter(typeof(T), "p");

        //        IQueryable<T> x = queryable.Where<T>(
        //          Expression.Lambda<Func<T, bool>>(
        //            Expression.LessThan(Expression.Property(
        //              pe,
        //              typeof(T).GetProperty(propertyName)),
        //              Expression.Constant(propertyValue, typeof(V)),
        //              false,
        //              typeof(T).GetMethod("op_Equality")),
        //          new ParameterExpression[] { pe }));

        //        return (x);
        //    }
        #endregion
    }
}

#region comments
//string[] companies = { "Consolidated Messenger", "Alpine Ski House", "Southridge Video", "City Power & Light",
//                   "Coho Winery", "Wide World Importers", "Graphic Design Institute", "Adventure Works",
//                   "Humongous Insurance", "Woodgrove Bank", "Margie's Travel", "Northwind Traders",
//                   "Blue Yonder Airlines", "Trey Research", "The Phone Company",
//                   "Wingtip Toys", "Lucerne Publishing", "Fourth Coffee" };

//// The IQueryable data to query.
//IQueryable<String> queryableData = companies.AsQueryable<string>();

//// Compose the expression tree that represents the parameter to the predicate.
//ParameterExpression pe = Expression.Parameter(typeof(string), "company");

//// ***** Where(company => (company.ToLower() == "coho winery" || company.Length > 16)) *****
//// Create an expression tree that represents the expression 'company.ToLower() == "coho winery"'.
//Expression left = Expression.Call(pe, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
//Expression right = Expression.Constant("coho winery");
//Expression e1 = Expression.Equal(left, right);

//// Create an expression tree that represents the expression 'company.Length > 16'.
//left = Expression.Property(pe, typeof(string).GetProperty("Length"));
//right = Expression.Constant(16, typeof(int));
//Expression e2 = Expression.GreaterThan(left, right);

//// Combine the expression trees to create an expression tree that represents the
//// expression '(company.ToLower() == "coho winery" || company.Length > 16)'.
//Expression predicateBody = Expression.OrElse(e1, e2);

//// Create an expression tree that represents the expression
//// 'queryableData.Where(company => (company.ToLower() == "coho winery" || company.Length > 16))'
//MethodCallExpression whereCallExpression = Expression.Call(
//    typeof(Queryable),
//    "Where",
//    new Type[] { queryableData.ElementType },
//    queryableData.Expression,
//    Expression.Lambda<Func<string, bool>>(predicateBody, new ParameterExpression[] { pe }));
//// ***** End Where *****

//// ***** OrderBy(company => company) *****
//// Create an expression tree that represents the expression
//// 'whereCallExpression.OrderBy(company => company)'
//MethodCallExpression orderByCallExpression = Expression.Call(
//    typeof(Queryable),
//    "OrderBy",
//    new Type[] { queryableData.ElementType, queryableData.ElementType },
//    whereCallExpression,
//    Expression.Lambda<Func<string, string>>(pe, new ParameterExpression[] { pe }));
//// ***** End OrderBy *****

//// Create an executable query from the expression tree.
//IQueryable<string> results = queryableData.Provider.CreateQuery<string>(orderByCallExpression);

//// Enumerate the results.
//foreach (string company in results)
//    Console.WriteLine(company);

///*  This code produces the following output:

//    Blue Yonder Airlines
//    City Power & Light
//    Coho Winery
//    Consolidated Messenger
//    Graphic Design Institute
//    Humongous Insurance
//    Lucerne Publishing
//    Northwind Traders
//    The Phone Company
//    Wide World Importers
//*/


#endregion