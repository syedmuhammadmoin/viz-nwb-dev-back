﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class ModelBuilderExtensions
    {
        static readonly MethodInfo SetQueryFilterMethod = typeof(ModelBuilderExtensions)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));

        public static void SetQueryFilterOnAllEntities<TEntityInterface>(
            this ModelBuilder builder,
            Expression<Func<TEntityInterface, bool>> filterExpression)
        {
            foreach (var type in builder.Model.GetEntityTypes()
                .Where(t => t.BaseType == null)
                .Select(t => t.ClrType)
                .Where(t => typeof(TEntityInterface).IsAssignableFrom(t)))
            {
                builder.SetEntityQueryFilter(
                    type,
                    filterExpression);
            }
        }

        static void SetEntityQueryFilter<TEntityInterface>(
            this ModelBuilder builder,
            Type entityType,
            Expression<Func<TEntityInterface, bool>> filterExpression)
        {
            SetQueryFilterMethod
                .MakeGenericMethod(entityType, typeof(TEntityInterface))
               .Invoke(null, new object[] { builder, filterExpression });
        }

        static void SetQueryFilter<TEntity, TEntityInterface>(
            this ModelBuilder builder,
            Expression<Func<TEntityInterface, bool>> filterExpression)
                where TEntityInterface : class
                where TEntity : class, TEntityInterface
        {
            var concreteExpression = filterExpression
                .Convert<TEntityInterface, TEntity>();
            builder.Entity<TEntity>()
                .AppendQueryFilter(concreteExpression);
        }

        // CREDIT: This comment by magiak on GitHub https://github.com/dotnet/efcore/issues/10275#issuecomment-785916356
        static void AppendQueryFilter<T>(this EntityTypeBuilder entityTypeBuilder, Expression<Func<T, bool>> expression)
            where T : class
        {
            var parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);

            var expressionFilter = ReplacingExpressionVisitor.Replace(
                expression.Parameters.Single(), parameterType, expression.Body);

            if (entityTypeBuilder.Metadata.GetQueryFilter() != null)
            {
                var currentQueryFilter = entityTypeBuilder.Metadata.GetQueryFilter();
                var currentExpressionFilter = ReplacingExpressionVisitor.Replace(
                    currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);
                expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
            }

            var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
            entityTypeBuilder.HasQueryFilter(lambdaExpression);
        }

    }
}
