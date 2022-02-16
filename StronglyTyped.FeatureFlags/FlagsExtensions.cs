namespace StronglyTyped.FeatureFlags;

public static class FlagsExtensions {
    #region When enabled
    public static void IfEnabledDo(this IFlag flag, Action doThis) {
        if (flag.IsEnabled) doThis();
    }

    public static void IfEnabledDo(this IFlag flag, Action doThis, Action doThat) {
        if (flag.IsEnabled) doThis();
        else doThat();
    }

    public static T IfEnabledGetOrDefault<T>(this IFlag flag, T @this, T @default = default!) {
        return flag.IsEnabled
            ? @this
            : @default;
    }

    public static T IfEnabledGet<T>(this IFlag flag, T @this, Func<T> getThat) {
        return flag.IsEnabled
            ? @this
            : getThat();
    }

    public static T IfEnabledGetOrDefault<T>(this IFlag flag, Func<T> getThis, T @default = default!) {
        return flag.IsEnabled
            ? getThis()
            : @default;
    }

    public static T IfEnabledGet<T>(this IFlag flag, Func<T> getThis, Func<T> getThat) {
        return flag.IsEnabled
            ? getThis()
            : getThat();
    }
    #endregion

    #region When disabled
    public static void IfDisabledDo(this IFlag flag, Action doThis) {
        if (!flag.IsEnabled) doThis();
    }

    public static void IfDisabledDo(this IFlag flag, Action doThis, Action doThat) {
        if (!flag.IsEnabled) doThis();
        else doThat();
    }

    public static T IfDisabledGetOrDefault<T>(this IFlag flag, T @this, T @default = default!) {
        return !flag.IsEnabled
            ? @this
            : @default;
    }

    public static T IfDisabledGet<T>(this IFlag flag, T @this, Func<T> getThat) {
        return !flag.IsEnabled
            ? @this
            : getThat();
    }
    public static T IfDisabledGetOrDefault<T>(this IFlag flag, Func<T> getThis, T @default = default!) {
        return !flag.IsEnabled
            ? getThis()
            : @default;
    }

    public static T IfDisabledGet<T>(this IFlag flag, Func<T> getThis, Func<T> getThat) {
        return !flag.IsEnabled
            ? getThis()
            : getThat();
    }
    #endregion

    #region When enabled async
    public static Task IfEnabledDoAsync(this IFlag flag, Task doThisAsync) {
        return flag.IsEnabled
            ? doThisAsync
            : Task.CompletedTask;
    }

    public static Task IfEnabledDoAsync(this IFlag flag, Task doThisAsync, Task doThatAsync) {
        return flag.IsEnabled
            ? doThisAsync
            : doThatAsync;
    }

    public static Task<T> IfEnabledGetOrDefaultAsync<T>(this IFlag flag, Task<T> getThisAsync, T that = default!) {
        return flag.IsEnabled
            ? getThisAsync
            : Task.FromResult(that);
    }

    public static Task<T> IfEnabledGetAsync<T>(this IFlag flag, Task<T> getThisAsync, Task<T> getThatAsync) {
        return flag.IsEnabled
            ? getThisAsync
            : getThatAsync;
    }

    public static Task<T> IfEnabledGetAsync<T>(this IFlag flag, Task<T> getThisAsync, Func<Task<T>> getThatAsync) {
        return flag.IsEnabled
            ? getThisAsync
            : getThatAsync();
    }

    public static Task<T> IfEnabledGetOrDefaultAsync<T>(this IFlag flag, Func<Task<T>> getThisAsync, T that = default!) {
        return flag.IsEnabled
            ? getThisAsync()
            : Task.FromResult(that);
    }

    public static Task<T> IfEnabledGetAsync<T>(this IFlag flag, Func<Task<T>> getThisAsync, Task<T> getThatAsync) {
        return flag.IsEnabled
            ? getThisAsync()
            : getThatAsync;
    }

    public static Task<T> IfEnabledGetAsync<T>(this IFlag flag, Func<Task<T>> getThisAsync, Func<Task<T>> getThatAsync) {
        return flag.IsEnabled
            ? getThisAsync()
            : getThatAsync();
    }
    #endregion

    #region When disabled async
    public static Task IfDisabledDoAsync(this IFlag flag, Task doThisAsync) {
        return !flag.IsEnabled
            ? doThisAsync
            : Task.CompletedTask;
    }

    public static Task IfDisabledDoAsync(this IFlag flag, Task doThisAsync, Task doThatAsync) {
        return !flag.IsEnabled
            ? doThisAsync
            : doThatAsync;
    }

    public static Task<T> IfDisabledGetOrDefaultAsync<T>(this IFlag flag, Task<T> getThisAsync, T that = default!) {
        return !flag.IsEnabled
            ? getThisAsync
            : Task.FromResult(that);
    }

    public static Task<T> IfDisabledGetAsync<T>(this IFlag flag, Task<T> getThisAsync, Task<T> getThatAsync) {
        return !flag.IsEnabled
            ? getThisAsync
            : getThatAsync;
    }

    public static Task<T> IfDisabledGetAsync<T>(this IFlag flag, Task<T> getThisAsync, Func<Task<T>> getThatAsync) {
        return !flag.IsEnabled
            ? getThisAsync
            : getThatAsync();
    }

    public static Task<T> IfDisabledGetOrDefaultAsync<T>(this IFlag flag, Func<Task<T>> getThisAsync, T that = default!) {
        return !flag.IsEnabled
            ? getThisAsync()
            : Task.FromResult(that);
    }

    public static Task<T> IfDisabledGetAsync<T>(this IFlag flag, Func<Task<T>> getThisAsync, Task<T> getThatAsync) {
        return !flag.IsEnabled
            ? getThisAsync()
            : getThatAsync;
    }

    public static Task<T> IfDisabledGetAsync<T>(this IFlag flag, Func<Task<T>> getThisAsync, Func<Task<T>> getThatAsync) {
        return !flag.IsEnabled
            ? getThisAsync()
            : getThatAsync();
    }
    #endregion
}