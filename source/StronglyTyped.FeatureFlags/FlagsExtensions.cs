namespace StronglyTyped.FeatureFlags;

public static class FlagsExtensions {
    #region When enabled
    public static void Do(this IFeatureState flag, Action doThis) {
        if (flag.IsEnabled) doThis();
    }

    public static void Do(this IFeatureState flag, Action doThis, Action doThat) {
        if (flag.IsEnabled) doThis();
        else doThat();
    }

    public static T GetOrDefault<T>(this IFeatureState flag, T @this) {
        return flag.IsEnabled
            ? @this
            : default!;
    }

    public static T GetOrDefault<T>(this IFeatureState flag, Func<T> getThis) {
        return flag.IsEnabled
            ? getThis()
            : default!;
    }

    public static T Get<T>(this IFeatureState flag, T @this, T that) {
        return flag.IsEnabled
            ? @this
            : that;
    }

    public static T Get<T>(this IFeatureState flag, T @this, Func<T> getThat) {
        return flag.IsEnabled
            ? @this
            : getThat();
    }

    public static T Get<T>(this IFeatureState flag, Func<T> getThis, T that) {
        return flag.IsEnabled
            ? getThis()
            : that;
    }

    public static T Get<T>(this IFeatureState flag, Func<T> getThis, Func<T> getThat) {
        return flag.IsEnabled
            ? getThis()
            : getThat();
    }
    #endregion

    #region When enabled async
    public static Task DoAsync(this IFeatureState flag, Func<Task> doThisAsync) {
        return flag.IsEnabled
            ? doThisAsync()
            : Task.CompletedTask;
    }

    public static Task DoAsync(this IFeatureState flag, Func<Task> doThisAsync, Func<Task> doThatAsync) {
        return flag.IsEnabled
            ? doThisAsync()
            : doThatAsync();
    }

    public static Task DoAsync(this IFeatureState flag, Action doThis, Func<Task> doThatAsync) {
        return flag.IsEnabled
            ? Task.Run(doThis)
            : doThatAsync();
    }

    public static Task DoAsync(this IFeatureState flag, Func<Task> doThisAsync, Action doThat) {
        return flag.IsEnabled
            ? doThisAsync()
            : Task.Run(doThat);
    }

    public static Task<T> GetOrDefaultAsync<T>(this IFeatureState flag, Func<Task<T>> getThisAsync) {
        return flag.IsEnabled
            ? getThisAsync()
            : Task.FromResult(default(T)!);
    }

    public static Task<T> GetAsync<T>(this IFeatureState flag, T @this, Func<Task<T>> getThatAsync) {
        return flag.IsEnabled
            ? Task.FromResult(@this)
            : getThatAsync();
    }

    public static Task<T> GetAsync<T>(this IFeatureState flag, Func<T> getThis, Func<Task<T>> getThatAsync) {
        return flag.IsEnabled
            ? Task.Run(getThis)
            : getThatAsync();
    }

    public static Task<T> GetAsync<T>(this IFeatureState flag, Func<Task<T>> getThisAsync, T that) {
        return flag.IsEnabled
            ? getThisAsync()
            : Task.FromResult(that);
    }

    public static Task<T> GetAsync<T>(this IFeatureState flag, Func<Task<T>> getThisAsync, Func<T> getThat) {
        return flag.IsEnabled
            ? getThisAsync()
            : Task.Run(getThat);
    }

    public static Task<T> GetAsync<T>(this IFeatureState flag, Func<Task<T>> getThisAsync, Func<Task<T>> getThatAsync) {
        return flag.IsEnabled
            ? getThisAsync()
            : getThatAsync();
    }
    #endregion
}