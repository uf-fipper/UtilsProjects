namespace ResultUnion.ResultExtensions;

public static partial class ResultExtension
{
    /// <summary>
    /// alias for MapOr
    /// </summary>
    /// <remarks>
    /// 使用此方法时要注意一种歧义的情况
    /// 请尽量使用<see cref="Result{T,E}.MapOr{U}"/>或<see cref="Result{T,E}.MapOrElse{U}"/>
    /// <code>
    /// var result = Result.Err&lt;int, string>("error");
    /// // 此时value1是Func&lt;object, string>，调用的是MapOr的重载
    /// var value1 = result.Map&lt;int, string, object>((object e) => "this is string", x => "a");
    /// // 此时value2是"this is string"，调用的是MapOrElse的重载
    /// var value2 = result.Map&lt;int, string, object>((string e) => "this is string", x => "a");
    /// // 此时value3是"this is string"，调用的是MapOrElse的重载
    /// var value3 = result.Map&lt;int, string, object>(e => "this is string", x => "a");
    /// </code>
    /// </remarks>
    public static U Map<T, E, U>(this Result<T, E> result, U @default, Func<T, U> func) =>
        result.MapOr(@default, func);

    /// <summary>
    /// alias for MapOrElse
    /// </summary>
    /// <remarks>
    /// 使用此方法时要注意一种歧义的情况
    /// 请尽量使用<see cref="Result{T,E}.MapOr{U}"/>或<see cref="Result{T,E}.MapOrElse{U}"/>
    /// <code>
    /// var result = Result.Err&lt;int, string>("error");
    /// // 此时value1是Func&lt;object, string>，调用的是MapOr的重载
    /// var value1 = result.Map&lt;int, string, object>((object e) => "this is string", x => "a");
    /// // 此时value2是"this is string"，调用的是MapOrElse的重载
    /// var value2 = result.Map&lt;int, string, object>((string e) => "this is string", x => "a");
    /// // 此时value3是"this is string"，调用的是MapOrElse的重载
    /// var value3 = result.Map&lt;int, string, object>(e => "this is string", x => "a");
    /// </code>
    /// </remarks>
    public static U Map<T, E, U>(this Result<T, E> result, Func<E, U> fe, Func<T, U> ft) =>
        result.MapOrElse(fe, ft);
}
