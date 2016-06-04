using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public class DefaultExpressionInjector : GenericInjector<DefaultExpression>
    {
        public DefaultExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override ExpressionType Type
        {
            get { return ExpressionType.Default; }
        }

        public override DefaultExpression Parse()
        {   
            Type type;
            
            TryGetType(this.ConfigurationElement, out type); 
            
            return Expression.Default(type);
        }
    }
}
