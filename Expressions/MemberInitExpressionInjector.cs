using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;
using NJection.Extensions;

namespace NJection.Expressions
{
    public class MemberInitExpressionInjector : GenericInjector<MemberInitExpression>
    {
        private const string _members = "members";
        private const string _expression = "expression";

        public override ExpressionType Type
        {
            get { return ExpressionType.MemberInit; }
        }

        public MemberInitExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) :
            base(parent, parentBlock, configurationElement) { }

        public override MemberInitExpression Parse()
        {
            NewExpression expression = null;
            IEnumerable<MemberBinding> memberAssignments = null;
            XElement membersElement = this.ConfigurationElement.Element(_members);
            XElement expressionElement = this.ConfigurationElement.Element(_expression);

            if (expressionElement == null)
                throw new ArgumentNullException(_expression);

            if (!TryResolve(expressionElement, out expression))
                throw new ArgumentTypeException(typeof(NewExpression), _expression);

            if (membersElement == null || !membersElement.HasElements)
                throw new ArgumentNullException(_members);

            memberAssignments = from memberElement in membersElement.Elements(_expression)
                                select MemberBindingVisitor.Visit(this, this.ParentBlock, memberElement, expression.Type);

            return Expression.MemberInit(expression, memberAssignments);
        }

        private bool TryResolve(XElement expressionElement, out NewExpression newExpression)
        {
            newExpression = this.Resolve<NewExpression>(expressionElement);
            return newExpression.NodeType == ExpressionType.New;
        }
    }
}
