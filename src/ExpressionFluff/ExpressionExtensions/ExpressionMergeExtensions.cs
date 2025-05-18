﻿namespace ExpressionFluff.ExpressionExtensions;

using ExpressionFluff.Exceptions.Generic;
using ExpressionFluff.ExpressionExtensions.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

public static class ExpressionMergeExtensions
{
    /// <summary>
    /// Allows you two merge two expressions of similar types together
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="other"></param>
    /// <param name="useAndNotOr"></param>
    /// <returns></returns>
    /// <exception cref="ParameterCountMismatchException"></exception>
    public static Expression<Func<TSource, bool>> Merge<TSource>(this Expression<Func<TSource, bool>> source, Expression<Func<TSource, bool>> other, bool useAndNotOr = true)
    {
        var sourceParameters = source.Parameters;
        var otherParameters = other.Parameters;

        if (sourceParameters.Count() != otherParameters.Count())
            throw new ParameterCountMismatchException(sourceParameters.Count(), otherParameters.Count());

        var aliasMap = CreateAliasMappings(sourceParameters, otherParameters);
        var otherBody = FindAndUpdate(other.Body, aliasMap);

        var mergedBody = Expression.AndAlso(source.Body, otherBody);
        return source.Update(mergedBody, sourceParameters);
    }

    private static Expression FindAndUpdate(Expression body, ReadOnlyDictionary<string, ParameterExpression> aliasMap)
    {
        var result = body;

        if (body is BinaryExpression binary)
        {
            result = UpdateBinary(binary, aliasMap);
        }
        else if (body is ParameterExpression parameter)
        {
            result = UpdateParameter(parameter, aliasMap);
        }
        else if (body is MemberExpression member)
        {
            result = UpdateMember(member, aliasMap);
        }
        else if (body is MethodCallExpression method)
        {
            return UpdateCall(aliasMap, method);
        }

        return result;
    }

    private static Expression UpdateBinary(BinaryExpression binary, ReadOnlyDictionary<string, ParameterExpression> aliasMap)
    {
        Expression result;
        var left = binary.Left;
        var right = binary.Right;
        left = FindAndUpdate(left, aliasMap);
        right = FindAndUpdate(right, aliasMap);
        result = binary.NewExpression(left, right);
        return result;
    }

    private static Expression UpdateCall(ReadOnlyDictionary<string, ParameterExpression> aliasMap, MethodCallExpression method)
    {
        return method.Update(null, method.Arguments.Select(a => UpdateMember(a as MemberExpression, aliasMap)));
    }

    private static Expression UpdateMember(MemberExpression expression, ReadOnlyDictionary<string, ParameterExpression> aliasMap)
    {
        if (expression.Expression is ParameterExpression parameter)
        {
            return expression.Update(UpdateParameter(parameter, aliasMap));
        }
        else if (expression.Expression is MemberExpression member && member.Expression is ParameterExpression memberParameter)
        {
            var propertyReferemce = Expression.MakeMemberAccess(UpdateParameter(memberParameter, aliasMap), member.Member);
            return expression.Update(propertyReferemce);
        }
        return expression;
    }

    static ParameterExpression UpdateParameter(ParameterExpression parameter, ReadOnlyDictionary<string, ParameterExpression> aliasMap)
    {
        var hasNewAlias = aliasMap.TryGetValue(parameter.Name, out var newAlias);
        return hasNewAlias
            ? newAlias
            : parameter;
    }

    private static ReadOnlyDictionary<string, ParameterExpression> CreateAliasMappings(ReadOnlyCollection<ParameterExpression> sourceParameters, ReadOnlyCollection<ParameterExpression> otherParameters)
    {
        var zipped = sourceParameters.Zip(otherParameters, (l, r) => (First: l, Second: r));

        var signatureMatch = zipped
            .All(parameter => parameter.First.Type.Equals(parameter.Second.Type));

        if (!signatureMatch)
        {
            var firstMismatch = zipped.First(o => !(o.First?.Type?.Equals(o.Second?.Type) ?? false));
            throw new ParameterTypeMismatchException(firstMismatch.First?.GetType(), firstMismatch.Second?.GetType());
        }

        return new ReadOnlyDictionary<string, ParameterExpression>(zipped.ToDictionary(o => o.Second.Name, o => o.First));
    }
}
