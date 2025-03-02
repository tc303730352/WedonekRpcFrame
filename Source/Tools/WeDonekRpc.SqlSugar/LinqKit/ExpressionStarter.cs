using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace WeDonekRpc.SqlSugar.LinqKit
{
    public class ExpressionStarter<T, T1>
    {
        internal ExpressionStarter () : this(false) { }

        internal ExpressionStarter ( bool defaultExpression )
        {
            if ( defaultExpression )
                DefaultExpression = ( a, b ) => true;
            else
                DefaultExpression = ( a, b ) => false;
        }

        internal ExpressionStarter ( Expression<Func<T, T1, bool>> exp ) : this(false)
        {
            _predicate = exp;
        }

        private Expression<Func<T, T1, bool>> Predicate => IsStarted || !UseDefaultExpression ? _predicate : DefaultExpression;

        private Expression<Func<T, T1, bool>> _predicate;
        public bool IsStarted => _predicate != null;

        public bool UseDefaultExpression => DefaultExpression != null;

        public Expression<Func<T, T1, bool>> DefaultExpression { get; set; }

        public Expression<Func<T, T1, bool>> Start ( Expression<Func<T, T1, bool>> exp )
        {
            if ( IsStarted )
                throw new Exception("Predicate cannot be started again.");

            return _predicate = exp;
        }

        public Expression<Func<T, T1, bool>> Or ( Expression<Func<T, T1, bool>> expr2 )
        {
            return IsStarted ? _predicate = Predicate.Or(expr2) : Start(expr2);
        }

        public Expression<Func<T, T1, bool>> And ( Expression<Func<T, T1, bool>> expr2 )
        {
            return IsStarted ? _predicate = Predicate.And(expr2) : Start(expr2);
        }

        public Expression<Func<T, T1, bool>> Not ()
        {
            if ( IsStarted )
            {
                _predicate = Predicate.Not();
            }
            else
            {
                Start(( a, b ) => false);
            }
            return _predicate;
        }

        public override string ToString ()
        {
            return Predicate == null ? null : Predicate.ToString();
        }

        /// <summary>
        /// Allows this object to be implicitely converted to an Expression{Func{T, bool}}.
        /// </summary>
        /// <param name="right"></param>
        public static implicit operator Expression<Func<T, T1, bool>> ( ExpressionStarter<T, T1> right )
        {
            return right == null ? null : right.Predicate;
        }

        /// <summary>
        /// Allows this object to be implicitely converted to an Expression{Func{T, bool}}.
        /// </summary>
        /// <param name="right"></param>
        public static implicit operator Func<T, T1, bool> ( ExpressionStarter<T, T1> right )
        {
            return right == null ? null : right.IsStarted || right.UseDefaultExpression ? right.Predicate.Compile() : null;
        }

        /// <summary>
        /// Allows this object to be implicitely converted to an Expression{Func{T, bool}}.
        /// </summary>
        /// <param name="right"></param>
        public static implicit operator ExpressionStarter<T, T1> ( Expression<Func<T, T1, bool>> right )
        {
            return right == null ? null : new ExpressionStarter<T, T1>(right);
        }

        public Func<T, T1, bool> Compile () { return Predicate.Compile(); }

        public Expression Body => Predicate.Body;


        public ExpressionType NodeType => Predicate.NodeType;

        /// <summary></summary>
        public ReadOnlyCollection<ParameterExpression> Parameters => Predicate.Parameters;

        /// <summary></summary>
        public Type Type => Predicate.Type;


        /// <summary></summary>
        public string Name => Predicate.Name;

        /// <summary></summary>
        public Type ReturnType => Predicate.ReturnType;

        /// <summary></summary>
        public bool TailCall => Predicate.TailCall;


        public virtual bool CanReduce => Predicate.CanReduce;
    }
}
