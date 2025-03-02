using System.Linq.Expressions;

namespace WeDonekRpc.SqlSugar.LinqKit
{
    public static class LinqKitExtend
    {
        private class RebindParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public RebindParameterVisitor ( ParameterExpression oldParameter, ParameterExpression newParameter )
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter ( ParameterExpression node )
            {
                if ( node == _oldParameter )
                {
                    return _newParameter;
                }

                return base.VisitParameter(node);
            }
        }
        public static ExpressionStarter<T, T1> New<T, T1> ( Expression<Func<T, T1, bool>> expression )
        {
            return new ExpressionStarter<T, T1>(expression);
        }
        public static ExpressionStarter<T, T1> New<T, T1> ()
        {
            return new ExpressionStarter<T, T1>();
        }
        public static Expression<Func<T, T1, bool>> Or<T, T1> ( this Expression<Func<T, T1, bool>> expr1, Expression<Func<T, T1, bool>> expr2 )
        {
            Expression body = new RebindParameterVisitor(expr2.Parameters[0], expr1.Parameters[0]).Visit(expr2.Body);
            return Expression.Lambda<Func<T, T1, bool>>(Expression.OrElse(expr1.Body, body), expr1.Parameters);
        }

        public static Expression<Func<T, T1, bool>> And<T, T1> ( this Expression<Func<T, T1, bool>> expr1, Expression<Func<T, T1, bool>> expr2 )
        {
            Expression body = new RebindParameterVisitor(expr2.Parameters[0], expr1.Parameters[0]).Visit(expr2.Body);
            return Expression.Lambda<Func<T, T1, bool>>(Expression.AndAlso(expr1.Body, body), expr1.Parameters);
        }
        public static Expression<Func<T, T1, bool>> Not<T, T1> ( this Expression<Func<T, T1, bool>> expr )
          => Expression.Lambda<Func<T, T1, bool>>(Expression.Not(expr.Body), expr.Parameters);
    }
}
