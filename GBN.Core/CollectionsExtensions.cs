using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnivIntel.Common
{
    public static class CollectionsExtensions
    {



        //    {
        //        return first.Concat(second);
        //    }

        //    public static IEnumerable<(this IEnumerable first, params object[] second)
        //    {
        //        return first.OfType<object>().Concat(second);
        //}
        //public static IEnumerable<T> Add<T>(this IEnumerable<T> first, params T[] second)


        //public static TValue Get<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key)
        //{
        //    if (dict == null) return default(TValue);
        //    return dict.With(x => x.ContainsKey(key)) ? dict[key] : default(TValue);
        //}


        //public static TInput Tee<TInput>(this TInput input, Action<TInput> action)
        //{
        //    if (Object.Equals(input, default(TInput)))
        //        return default(TInput);

        //    action(input);
        //    return input;
        //}

        //public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> func)
        //{
        //    if (Object.Equals(input, default(TInput)))
        //        return default(TResult);

        //    return func(input);
        //}

        //public static TInput WithDefault<TInput>(this TInput input, Func<TInput> func)
        //{
        //    if (Object.Equals(input, default(TInput)))
        //        return func();

        //    return input;
        //}

        //public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> func, TResult @default)
        //{
        //    if (Object.Equals(input, default(TInput)))
        //        return @default;

        //    return func(input);
        //}

        //public static TResult If<TInput, TResult>(this TInput input, Func<TInput, bool> predicate, Func<TInput, TResult> then, Func<TInput, TResult> @else)
        //{
        //    if (input.With(predicate))
        //        return then(input);

        //    return @else(input);
        //}

        //public static void If<TInput>(this TInput input, Func<TInput, bool> predicate, Action<TInput> then, Action<TInput> @else)
        //{
        //    if (input.With(predicate))
        //        then(input);

        //    @else(input);
        //}

        //public static void If<TInput>(this TInput input, Func<TInput, bool> predicate, Action<TInput> then)
        //{
        //    if (input.With(predicate))
        //        then(input);
        //}

        //public static void Do<TInput>(this TInput input, Action<TInput> action)
        //{
        //    if (Object.Equals(input, default(TInput)))
        //        return;

        //    action(input);
        //}

        //public static IEnumerable<T> SafeUnion<T>(this IEnumerable<T> input, IEnumerable<T> other)
        //{
        //    if (input != null)
        //    {
        //        if (other != null)
        //            return input.Union(other);
        //    }
        //    return null;
        //}
    }
}
