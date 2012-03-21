using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public class ConditionalExpressionInjector : GenericInjector<ConditionalExpression>
    {
        private const string _test = "test";
        private const string _ifTrue = "ifTrue";
        private const string _ifFalse = "ifFalse";
        private const string _expressions = "expressions";

        public override ExpressionType Type
        {
            get { return ExpressionType.Conditional; }
        }

        public ConditionalExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override ConditionalExpression Parse()
        {
            Expression test = null, ifTrue = null;
            XElement testElement = ConfigurationElement.Element(_test);
            XElement ifTrueElement = ConfigurationElement.Element(_ifTrue);
            XElement ifFalseElement = ConfigurationElement.Element(_ifFalse);
                
            if (testElement == null || !testElement.HasElements)
                throw new ArgumentNullException(_test);
    
            if (ifTrueElement == null)
                throw new ArgumentNullException(_ifTrue);

            test = this.Resolve<Expression>(testElement.FirstNode as XElement);
            ifTrue = this.Resolve<Expression>(ifTrueElement.FirstNode as XElement);

            if (ifFalseElement != null && ifFalseElement.HasElements)
            {
                Expression ifFalse = this.Resolve<Expression>(ifFalseElement.FirstNode as XElement);
                return Expression.IfThenElse(test, ifTrue, ifFalse);
            }

            return Expression.IfThen(test, ifTrue);
        }
    }
}
