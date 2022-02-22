namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
internal class ProcessSpy {
    private readonly IList<string> _calls = new List<string>();

    public void RegisterCall(string call) => _calls.Add(call);
    public IEnumerable<string> GetCalls() => _calls;
    public void ClearCalls() => _calls.Clear();
}