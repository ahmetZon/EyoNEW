using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

public static class AjaxCustomPaging
{
    public static IQueryable<T> ApplyOrdersPaging<T>(this IQueryable<T> data, int page, int pageSize)
    {
        if (pageSize > 0 && page > 0)
        {
            data = data.Skip((page - 1) * pageSize);
        }

        data = data.Take(pageSize);

        return data;
    }

    public static IEnumerable ApplyOrdersGrouping<T>(this IQueryable<T> data, IList<GroupDescriptor>
        groupDescriptors)
    {
        if (groupDescriptors != null && groupDescriptors.Any())
        {
            Func<IEnumerable<T>, IEnumerable<AggregateFunctionsGroup>> selector = null;
            foreach (var group in groupDescriptors.Reverse())
            {
                if (selector == null)
                {
                    //if (group.Member == "ShipCity")
                    //{
                    //    selector = Orders => BuildInnerGroup(Orders, o => o.ShipCity);
                    //}
                }
                else
                {
                    //if (group.Member == "ShipCity")
                    //{
                    //    selector = BuildGroup(o => o.ShipCity, selector);
                    //}
                }
            }

            return selector.Invoke(data).ToList();
        }

        return data.ToList();
    }

    private static Func<IEnumerable<T>, IEnumerable<AggregateFunctionsGroup>>
            BuildGroup<T>(Expression<Func<T, T>> groupSelector, Func<IEnumerable<T>,
            IEnumerable<AggregateFunctionsGroup>> selectorBuilder)
    {
        var tempSelector = selectorBuilder;
        return g => g.GroupBy(groupSelector.Compile())
                     .Select(c => new AggregateFunctionsGroup
                     {
                         Key = c.Key,
                         HasSubgroups = true,
                         Member = groupSelector.MemberWithoutInstance(),
                         Items = tempSelector.Invoke(c).ToList()
                     });
    }

    private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<T>
        group, Expression<Func<T, T>> groupSelector)
    {
        return group.GroupBy(groupSelector.Compile())
                .Select(i => new AggregateFunctionsGroup
                {
                    Key = i.Key,
                    Member = groupSelector.MemberWithoutInstance(),
                    Items = i.ToList()
                });
    }

    public static IQueryable<T> ApplyOrdersSorting<T>(this IQueryable<T> data,
                IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
    {
        if (groupDescriptors != null && groupDescriptors.Any())
        {
            foreach (var groupDescriptor in groupDescriptors.Reverse())
            {
                data = AddSortExpression(data, groupDescriptor.SortDirection, groupDescriptor.Member);
            }
        }

        if (sortDescriptors != null && sortDescriptors.Any())
        {
            foreach (SortDescriptor sortDescriptor in sortDescriptors)
            {
                data = AddSortExpression(data, sortDescriptor.SortDirection, sortDescriptor.Member);
            }
        }

        return data;
    }

    private static IQueryable<T> AddSortExpression<T>(IQueryable<T> data, ListSortDirection
                sortDirection, string memberName)
    {
        if (sortDirection == ListSortDirection.Ascending)
        {

            data = data.OrderBy(memberName).AsQueryable();
            //switch (memberName)
            //    {
            //        case "Id":
            //            //data = data.OrderBy(order => order.OrderID);
            //            break;
            //    }
        }
        else
        {
            data = data.OrderBy(memberName + " desc").AsQueryable();
        }
        return data;
    }

    public static IQueryable<T> ApplyOrdersFiltering<T>(this IQueryable<T> data,
       IList<IFilterDescriptor> filterDescriptors)
    {
        if (filterDescriptors.Any())
        {
            data = data.Where(ExpressionBuilder.Expression<T>(filterDescriptors, false));
        }
        return data;
    }
}
