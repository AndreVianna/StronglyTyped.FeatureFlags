namespace StronglyTyped.FeatureFlags.TestConsumer;

public partial class TestFeatures {
    private static readonly string[] _availableFeatures = {
        "Feature1",
        "Feature2",
        nameof(TestSubFeatures),
    };
}

public partial class TestSubFeatures {
    private static readonly string[] _availableFeatures = {
        "Feature3",
        "Feature4",
    };
}

public interface ITestFeatures {
    IFeatureState Feature1 { get; }
    IFeatureState Feature2 { get; }
    ITestSubFeatures TestSubFeatures { get; }
}
public interface ITestSubFeatures {
    IFeatureState Feature3 { get; }
    IFeatureState Feature4 { get; }
}

partial class TestFeatures : ITestFeatures {
    private readonly IFeatureAccessor _featureAccessor;
    private readonly string _parentPath = string.Empty;

    public TestFeatures(IFeatureAccessor featureAccessor) {
        _featureAccessor = featureAccessor;
        TestSubFeatures = new TestSubFeatures(_featureAccessor);
    }
    public IFeatureState Feature1 => _featureAccessor.For($"{_parentPath}{nameof(Feature1)}");
    public IFeatureState Feature2 => _featureAccessor.For($"{_parentPath}{nameof(Feature2)}");

    public ITestSubFeatures TestSubFeatures { get; }
}

partial class TestSubFeatures : ITestSubFeatures {
    private readonly IFeatureAccessor _featureAccessor;
    private readonly string _parentPath = $"{nameof(TestFeatures)}:";

    public TestSubFeatures(IFeatureAccessor featureAccessor) {
        _featureAccessor = featureAccessor;
    }
    public IFeatureState Feature3 => _featureAccessor.For($"{_parentPath}{nameof(Feature3)}");
    public IFeatureState Feature4 => _featureAccessor.For($"{_parentPath}{nameof(Feature3)}");
}
