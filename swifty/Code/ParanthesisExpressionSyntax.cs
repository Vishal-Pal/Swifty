using System;
using System.Collections.Generic;
using System.Linq;

namespace swifty.Code {
    sealed class ParanthesisExpressionSyntax: ExpressionSyntax {
        public ParanthesisExpressionSyntax(SyntaxToken leftParanthesis, ExpressionSyntax expr, SyntaxToken rightParanthesis) {
            LeftParanthesisToken = leftParanthesis;
            Expression = expr;
            RightParanthesisToken = rightParanthesis;
        }
        public SyntaxToken LeftParanthesisToken {get;}
        public ExpressionSyntax Expression {get;}
        public SyntaxToken RightParanthesisToken {get;}
        public override SyntaxKind Kind => SyntaxKind.ParathesisExpression;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return LeftParanthesisToken;
            yield return Expression;
            yield return RightParanthesisToken;
        }
    }
}