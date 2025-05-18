namespace ExpressionFluff.ExpressionExtensions.Base;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;


public static class BinaryExpressionExtension
{
    public static Expression NewExpression(this BinaryExpression expression, Expression left, Expression right)
    {
        return expression.NodeType.ToString() switch
        {
            nameof(Expression.Add) => Expression.Add(left, right),
            nameof(Expression.AddAssign) => Expression.AddAssign(left, right),
            nameof(Expression.AddChecked) => Expression.AddChecked(left, right),
            nameof(Expression.And) => Expression.And(left, right),
            nameof(Expression.AndAlso) => Expression.AndAlso(left, right),
            nameof(Expression.AndAssign) => Expression.AndAssign(left, right),
            nameof(Expression.ArrayIndex) => Expression.ArrayIndex(left, right),
            nameof(Expression.Assign) => Expression.Assign(left, right),
            nameof(Expression.AddAssignChecked) => Expression.AddAssignChecked(left, right),
            nameof(Expression.Coalesce) => Expression.Coalesce(left, right),
            nameof(Expression.Divide) => Expression.Divide(left, right),
            nameof(Expression.DivideAssign) => Expression.DivideAssign(left, right),
            nameof(Expression.Equal) => Expression.Equal(left, right),
            nameof(Expression.ExclusiveOr) => Expression.ExclusiveOr(left, right),
            nameof(Expression.ExclusiveOrAssign) => Expression.ExclusiveOrAssign(left, right),
            nameof(Expression.GreaterThan) => Expression.GreaterThan(left, right),
            nameof(Expression.GreaterThanOrEqual) => Expression.GreaterThanOrEqual(left, right),
            nameof(Expression.LeftShift) => Expression.LeftShift(left, right),
            nameof(Expression.LeftShiftAssign) => Expression.LeftShiftAssign(left, right),
            nameof(Expression.LessThan) => Expression.LessThan(left, right),
            nameof(Expression.LessThanOrEqual) => Expression.LessThanOrEqual(left, right),
            nameof(Expression.Modulo) => Expression.Modulo(left, right),
            nameof(Expression.ModuloAssign) => Expression.ModuloAssign(left, right),
            nameof(Expression.Multiply) => Expression.Multiply(left, right),
            nameof(Expression.MultiplyAssign) => Expression.MultiplyAssign(left, right),
            nameof(Expression.MultiplyChecked) => Expression.MultiplyChecked(left, right),
            nameof(Expression.MultiplyAssignChecked) => Expression.MultiplyAssignChecked(left, right),
            nameof(Expression.NotEqual) => Expression.NotEqual(left, right),
            nameof(Expression.Or) => Expression.Or(left, right),
            nameof(Expression.OrAssign) => Expression.OrAssign(left, right),
            nameof(Expression.OrElse) => Expression.OrElse(left, right),
            nameof(Expression.PostDecrementAssign) => Expression.PostDecrementAssign(left ?? right),
            nameof(Expression.PostIncrementAssign) => Expression.PostIncrementAssign(left ?? right),
            nameof(Expression.Power) => Expression.Power(left, right),
            nameof(Expression.PowerAssign) => Expression.PowerAssign(left, right),
            nameof(Expression.PreDecrementAssign) => Expression.PreDecrementAssign(left ?? right),
            nameof(Expression.PreIncrementAssign) => Expression.PreIncrementAssign(left ?? right),
            nameof(Expression.RightShift) => Expression.RightShift(left, right),
            nameof(Expression.RightShiftAssign) => Expression.RightShiftAssign(left, right),
            nameof(Expression.Subtract) => Expression.Subtract(left, right),
            nameof(Expression.SubtractAssign) => Expression.SubtractAssign(left, right),
            nameof(Expression.SubtractChecked) => Expression.SubtractChecked(left, right),
            nameof(Expression.SubtractAssignChecked) => Expression.SubtractAssignChecked(left, right),
            _ => expression
        };
    }
}
