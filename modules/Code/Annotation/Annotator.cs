using System;
using swifty.Code.Syntaxt;
using System.Collections.Generic;

namespace swifty.Code.Annotation {    
    sealed class Annotator {
        private readonly DiagnosisHandler _diagnostics = new DiagnosisHandler();
        public DiagnosisHandler Diagnostics => _diagnostics;
        public AnnotatedExpression AnnotateExpression(ExpressionSyntax syntax) {
            switch(syntax.Kind) {
                case SyntaxKind.BinaryExpression: return AnnotateBinaryExpression((BinaryExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression: return AnnotateUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.LiteralExpression: return AnnotateLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.ParathesisExpression: return AnnotateExpression(((ParanthesisExpressionSyntax)syntax).Expression);
                default: throw new Exception($"Unexpected Syntax {syntax.Kind}");
            }
        }
        public AnnotatedExpression AnnotateBinaryExpression(BinaryExpressionSyntax syntax) {
            var annotateLeft = AnnotateExpression(syntax.Left);
            var annotateRight = AnnotateExpression(syntax.Right);
            var annotateOperatorKind = AnnotatedBinaryOperator.Annotate(syntax.OperatorToken.Kind, annotateLeft.Type, annotateRight.Type);
            if (annotateOperatorKind==null) {
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, annotateLeft.Type,syntax.OperatorToken.Text, annotateRight.Type);
                return annotateRight;
            }
            return new AnnotatedBinaryExpression(annotateLeft, annotateOperatorKind, annotateRight);
        }
        public AnnotatedExpression AnnotateUnaryExpression(UnaryExpressionSyntax syntax) {
            var annotateOperand = AnnotateExpression(syntax.Operand);
            var annotateOperatorKind = AnnotatedUnaryOperator.Annotate(syntax.OperatorToken.Kind, annotateOperand.Type);
            if (annotateOperatorKind==null) {
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, annotateOperand.Type);
                return annotateOperand;
            }
            return new AnnotatedUnaryExpression(annotateOperatorKind, annotateOperand);
        }
        public AnnotatedExpression AnnotateLiteralExpression(LiteralExpressionSyntax syntax) {
            var value = syntax.Value ?? 0;
            return new AnnotatedLiteralExpression(value);
        }
    }
}