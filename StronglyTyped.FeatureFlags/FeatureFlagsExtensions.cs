namespace StronglyTyped.FeatureFlags;

public static class FeatureFlagsExtensions {
    #region When enabled
    public static void IfEnabledDo(this IFeatureFlag flag, Action doThis) {
        if (flag.IsEnabled) doThis();
    }

    public static void IfEnabledDo(this IFeatureFlag flag, Action doThis, Action doThat) {
        if (flag.IsEnabled) doThis();
        else doThat();
    }

    public static T IfEnabledGetOrDefault<T>(this IFeatureFlag flag, T @this, T @default = default!) {
        return flag.IsEnabled
            ? @this
            : @default;
    }

    public static T IfEnabledGet<T>(this IFeatureFlag flag, T @this, Func<T> getThat) {
        return flag.IsEnabled
            ? @this
            : getThat();
    }

    public static T IfEnabledGetOrDefault<T>(this IFeatureFlag flag, Func<T> getThis, T @default = default!) {
        return flag.IsEnabled
            ? getThis()
            : @default;
    }

    public static T IfEnabledGet<T>(this IFeatureFlag flag, Func<T> getThis, Func<T> getThat) {
        return flag.IsEnabled
            ? getThis()
            : getThat();
    }
    #endregion

    #region When disabled
    public static void IfDisabledDo(this IFeatureFlag flag, Action doThis) {
        if (!flag.IsEnabled) doThis();
    }

    public static void IfDisabledDo(this IFeatureFlag flag, Action doThis, Action doThat) {
        if (!flag.IsEnabled) doThis();
        else doThat();
    }

    public static T IfDisabledGetOrDefault<T>(this IFeatureFlag flag, T @this, T @default = default!) {
        return !flag.IsEnabled
            ? @this
            : @default;
    }

    public static T IfDisabledGet<T>(this IFeatureFlag flag, T @this, Func<T> getThat) {
        return !flag.IsEnabled
            ? @this
            : getThat();
    }
    public static T IfDisabledGetOrDefault<T>(this IFeatureFlag flag, Func<T> getThis, T @default = default!) {
        return !flag.IsEnabled
            ? getThis()
            : @default;
    }

    public static T IfDisabledGet<T>(this IFeatureFlag flag, Func<T> getThis, Func<T> getThat) {
        return !flag.IsEnabled
            ? getThis()
            : getThat();
    }
    #endregion

    #region When enabled async
    public static Task IfEnabledDoAsync(this IFeatureFlag flag, Task doThisAsync) {
        return flag.IsEnabled
            ? doThisAsync
            : Task.CompletedTask;
    }

    public static Task IfEnabledDoAsync(this IFeatureFlag flag, Task doThisAsync, Task doThatAsync) {
        return flag.IsEnabled
            ? doThisAsync
            : doThatAsync;
    }

    public static Task<T> IfEnabledGetOrDefaultAsync<T>(this IFeatureFlag flag, Task<T> getThisAsync, T that = default!) {
        return flag.IsEnabled
            ? getThisAsync
            : Task.FromResult(that);
    }

    public static Task<T> IfEnabledGetAsync<T>(this IFeatureFlag flag, Task<T> getThisAsync, Task<T> getThatAsync) {
        return flag.IsEnabled
            ? getThisAsync
            : getThatAsync;
    }

    public static Task<T> IfEnabledGetAsync<T>(this IFeatureFlag flag, Task<T> getThisAsync, Func<Task<T>> getThatAsync) {
        return flag.IsEnabled
            ? getThisAsync
            : getThatAsync();
    }

    public static Task<T> IfEnabledGetOrDefaultAsync<T>(this IFeatureFlag flag, Func<Task<T>> getThisAsync, T that = default!) {
        return flag.IsEnabled
            ? getThisAsync()
            : Task.FromResult(that);
    }

    public static Task<T> IfEnabledGetAsync<T>(this IFeatureFlag flag, Func<Task<T>> getThisAsync, Task<T> getThatAsync) {
        return flag.IsEnabled
            ? getThisAsync()
            : getThatAsync;
    }

    public static Task<T> IfEnabledGetAsync<T>(this IFeatureFlag flag, Func<Task<T>> getThisAsync, Func<Task<T>> getThatAsync) {
        return flag.IsEnabled
            ? getThisAsync()
            : getThatAsync();
    }
    #endregion

    #region When disabled async
    public static Task IfDisabledDoAsync(this IFeatureFlag flag, Task doThisAsync) {
        return !flag.IsEnabled
            ? doThisAsync
            : Task.CompletedTask;
    }

    public static Task IfDisabledDoAsync(this IFeatureFlag flag, Task doThisAsync, Task doThatAsync) {
        return !flag.IsEnabled
            ? doThisAsync
            : doThatAsync;
    }

    public static Task<T> IfDisabledGetOrDefaultAsync<T>(this IFeatureFlag flag, Task<T> getThisAsync, T that = default!) {
        return !flag.IsEnabled
            ? getThisAsync
            : Task.FromResult(that);
    }

    public static Task<T> IfDisabledGetAsync<T>(this IFeatureFlag flag, Task<T> getThisAsync, Task<T> getThatAsync) {
        return !flag.IsEnabled
            ? getThisAsync
            : getThatAsync;
    }

    public static Task<T> IfDisabledGetAsync<T>(this IFeatureFlag flag, Task<T> getThisAsync, Func<Task<T>> getThatAsync) {
        return !flag.IsEnabled
            ? getThisAsync
            : getThatAsync();
    }

    public static Task<T> IfDisabledGetOrDefaultAsync<T>(this IFeatureFlag flag, Func<Task<T>> getThisAsync, T that = default!) {
        return !flag.IsEnabled
            ? getThisAsync()
            : Task.FromResult(that);
    }

    public static Task<T> IfDisabledGetAsync<T>(this IFeatureFlag flag, Func<Task<T>> getThisAsync, Task<T> getThatAsync) {
        return !flag.IsEnabled
            ? getThisAsync()
            : getThatAsync;
    }

    public static Task<T> IfDisabledGetAsync<T>(this IFeatureFlag flag, Func<Task<T>> getThisAsync, Func<Task<T>> getThatAsync) {
        return !flag.IsEnabled
            ? getThisAsync()
            : getThatAsync();
    }
    #endregion
}