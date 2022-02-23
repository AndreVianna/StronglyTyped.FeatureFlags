﻿namespace StronglyTyped.FeatureFlags.SourceGeneration.Tests;
using static Helpers.GeneratorRunner;

[ExcludeFromCodeCoverage]
public class SourceGenerationTests {

    #region content strings
    private const string _emptyInterface = @"// <auto-generated/>
#nullable enable

using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

public interface IFeatureAccessor
{
}
";

    private const string _emptyClass = @"// <auto-generated/>
#nullable enable

using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

partial class FeatureAccessor : IFeatureAccessor
{
    private readonly IFlagsFactory _flagsFactory;

    public FeatureAccessor(IFlagsFactory flagsFactory)
    {
        _flagsFactory = flagsFactory;
    }
}
";


    private const string _interfaceWithTwoProperties = @"// <auto-generated/>
#nullable enable

using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

public interface IFeatureAccessor
{
    IFlag Feature1 { get; }
    IFlag Feature2 { get; }
}
";

    private const string _classWithTwoProperties = @"// <auto-generated/>
#nullable enable

using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

partial class FeatureAccessor : IFeatureAccessor
{
    private readonly IFlagsFactory _flagsFactory;

    public FeatureAccessor(IFlagsFactory flagsFactory)
    {
        _flagsFactory = flagsFactory;
    }
    public IFlag Feature1 => _flagsFactory.For(nameof(Feature1));
    public IFlag Feature2 => _flagsFactory.For(nameof(Feature2));
}
";
    #endregion

    [Theory]
    [InlineData("string")]
    [InlineData("String")]
    [InlineData("System.String")]
    public async Task SourceFile_WithAClass_WithFeatureFlagsAttribute_AndAPrivateStringArrayField_InitializedWithTwoFeatures_GeneratesCode_WithTwoProperties(string arrayType) {
        var code = @$"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

[FeatureFlags(nameof(_availableFeatures))]
public partial class FeatureAccessor {{
    private static readonly {arrayType}[] _availableFeatures = {{
        ""Feature1"",
        ""Feature2""
    }};
}}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(0);
        resultedCode.Length.Should().Be(2);
        resultedCode[0].SourceText.ToString().Should().Be(_interfaceWithTwoProperties);
        resultedCode[1].SourceText.ToString().Should().Be(_classWithTwoProperties);
    }

    [Fact]
    public async Task SourceFile_WithOldNamespacesStyle_AndValidClass_GeneratesCode_WithTwoProperties() {
        const string code = @"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests {
    [FeatureFlags(nameof(_availableFeatures))]
    public partial class FeatureAccessor {
        private static readonly string[] _availableFeatures = {
            ""Feature1"",
            ""Feature2""
        };
    }
}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(0);
        resultedCode.Length.Should().Be(2);
        resultedCode[0].SourceText.ToString().Should().Be(_interfaceWithTwoProperties);
        resultedCode[1].SourceText.ToString().Should().Be(_classWithTwoProperties);
    }

    [Fact]
    public async Task SourceFile_WithoutNamespace_DoesNotGenerateCode() {
        const string code = @"
using StronglyTyped.FeatureFlags;

[FeatureFlags(nameof(_availableFeatures))]
public partial class FeatureAccessor {
    private static readonly string[] _availableFeatures = {
        ""Feature1"",
        ""Feature2""
    };
}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(0);
        resultedCode.Length.Should().Be(0);
    }

    [Fact]
    public async Task SourceFile_WithAClassWithNoAttributes_DoesNotGenerateCode() {
        const string code = @"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

public partial class FeatureAccessor {
    private static readonly string[] _availableFeatures = {
        ""Feature1"",
        ""Feature2""
    };
}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(0);
        resultedCode.Length.Should().Be(0);
    }

    [Fact]
    public async Task SourceFile_WithAClass_WithoutFeatureFlagsAttribute_DoesNotGenerateCode() {
        const string code = @"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

[Obsolete]
public partial class FeatureAccessor {
    private static readonly string[] _availableFeatures = {
        ""Feature1"",
        ""Feature2""
    };
}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(0);
        resultedCode.Length.Should().Be(0);
    }

    [Fact]
    public async Task SourceFile_WithAValidClass_WithAProperty_GeneratesCode_WithNoProperties() {
        const string code = @"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

[FeatureFlags(""_availableFeatures"")]
public partial class FeatureAccessor {
    public static string[] AvailableFeatures { get; }
}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(1);
        diagnostics[0].Descriptor.Description.ToString().Should().Contain("A field with name '_availableFeatures' was not found.");
        resultedCode.Length.Should().Be(0);
    }

    [Fact]
    public async Task SourceFile_WithAValidClass_WithAPublicField_GeneratesCode_WithNoProperties() {
        const string code = @"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

[FeatureFlags(nameof(_availableFeatures))]
public partial class FeatureAccessor {
    public static readonly string[] _availableFeatures = {
        ""Feature1"",
        ""Feature2""
    };
}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(1);
        diagnostics[0].Descriptor.Description.ToString().Should().Contain("The '_availableFeatures' must be private.");
        resultedCode.Length.Should().Be(0);
    }

    [Fact]
    public async Task SourceFile_WithAValidClass_WithNotArrayField_GeneratesCode_WithNoProperties() {
        const string code = @"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

[FeatureFlags(nameof(_availableFeatures))]
public partial class FeatureAccessor {
    private static string _availableFeatures;
}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(1);
        diagnostics[0].Descriptor.Description.ToString().Should().Contain("The '_availableFeatures' field must be a string array.");
        resultedCode.Length.Should().Be(0);
    }

    [Fact]
    public async Task SourceFile_WithAValidClass_WithNoInitializer_GeneratesCode_WithNoProperties() {
        const string code = @"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

[FeatureFlags(nameof(_availableFeatures))]
public partial class FeatureAccessor {{
    private static readonly string[] _availableFeatures;
}}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(0);
        resultedCode.Length.Should().Be(2);
        resultedCode[0].SourceText.ToString().Should().Be(_emptyInterface);
        resultedCode[1].SourceText.ToString().Should().Be(_emptyClass);
    }

    [Fact]
    public async Task SourceFile_WithAValidClass_WithAnArrayOfAnInvalidType_GeneratesCode_WithNoProperties() {
        const string code = @"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

[FeatureFlags(nameof(_availableFeatures))]
public partial class FeatureAccessor {
    private static readonly int[] _availableFeatures = {
        1,
        2
    };
}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(1);
        diagnostics[0].Descriptor.Description.ToString().Should().Contain("The '_availableFeatures' field must be a string array.");
        resultedCode.Length.Should().Be(0);
    }

    [Fact]
    public async Task SourceFile_WithAValidClass_WithAnEmptyStringArray_GeneratesCode_WithNoProperties() {
        const string code = @"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

[FeatureFlags(nameof(_availableFeatures))]
public partial class FeatureAccessor {
    private static readonly String[] _availableFeatures = {
    };
}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(0);
        resultedCode.Length.Should().Be(2);
        resultedCode[0].SourceText.ToString().Should().Be(_emptyInterface);
        resultedCode[1].SourceText.ToString().Should().Be(_emptyClass);
    }

    [Fact]
    public async Task SourceFile_WithAValidClass_WithNonLiteralValue_GeneratesCode_WithOnlyLiteralValues() {
        const string code = @"
using StronglyTyped.FeatureFlags;

namespace SourceGeneration.Tests;

[FeatureFlags(nameof(_availableFeatures))]
public partial class FeatureAccessor {
    private const string _feature3 = ""Feature3"";

    private static readonly String[] _availableFeatures = {
         ""Feature1"",
        ""Feature2""
       _feature3,
       $""Feature{4}"" 
    };
}
";

        var (diagnostics, resultedCode) = await RunAsync(code);

        diagnostics.Length.Should().Be(0);
        resultedCode.Length.Should().Be(2);
        resultedCode[0].SourceText.ToString().Should().Be(_interfaceWithTwoProperties);
        resultedCode[1].SourceText.ToString().Should().Be(_classWithTwoProperties);
    }
}
