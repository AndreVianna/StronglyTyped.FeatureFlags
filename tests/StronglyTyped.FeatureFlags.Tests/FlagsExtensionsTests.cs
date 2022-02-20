namespace StronglyTyped.FeatureFlags.Tests;

public class FlagsExtensionsTests {

    private readonly IFlag _enabledFlag = new Flag(true);
    private readonly IFlag _disabledFlag = new Flag(false);

    private static bool _doneThis;
    private static bool _doneThat;

    private readonly Action _doThis = () => _doneThis = true;
    private readonly Action _doThat = () => _doneThat = true;
    private readonly Func<Task> _doThisAsync = () => {
        _doneThis = true;
        return Task.CompletedTask;
    };
    private readonly Func<Task> _doThatAsync = () => {
        _doneThat = true;
        return Task.CompletedTask;
    };

    private const int _this = 42;
    private const int _that = 13;
    private readonly Func<int> _getThis = () => _this;
    private readonly Func<int> _getThat = () => _that;
    private readonly Func<Task<int>> _getThisAsync = () => Task.FromResult(_this);
    private readonly Func<Task<int>> _getThatAsync = () => Task.FromResult(_that);

    [Fact]
    public void ForEnabledFlag_Do_DoThis() {
        // Arrange
        _doneThis = false;

        // Act
        _enabledFlag.Do(_doThis);

        // Assert
        _doneThis.Should().BeTrue();
    }

    [Fact]
    public void ForDisabledFlag_Do_DoNotThis() {
        // Arrange
        _doneThis = false;

        // Act
        _disabledFlag.Do(_doThis);

        // Assert
        _doneThis.Should().BeFalse();
    }

    [Fact]
    public async Task ForEnabledFlag_DoAsync_DoThisAsync() {
        // Arrange
        _doneThis = false;

        // Act
        await _enabledFlag.DoAsync(_doThisAsync);

        // Assert
        _doneThis.Should().BeTrue();
    }

    [Fact]
    public async Task ForDisabledFlag_DoAsync_DoNotThisAsync() {
        // Arrange
        _doneThis = false;

        // Act
        await _disabledFlag.DoAsync(_doThisAsync);

        // Assert
        _doneThis.Should().BeFalse();
    }

    [Fact]
    public void ForEnabledFlag_Do_DoThis_NotThat() {
        // Arrange
        _doneThis = false;
        _doneThat = false;

        // Act
        _enabledFlag.Do(_doThis, _doThat);

        // Assert
        _doneThis.Should().BeTrue();
        _doneThat.Should().BeFalse();
    }


    [Fact]
    public void ForDisabledFlag_Do_DoNotThis_DoThat() {
        // Arrange
        _doneThis = false;
        _doneThat = false;

        // Act
        _disabledFlag.Do(_doThis, _doThat);

        // Assert
        _doneThis.Should().BeFalse();
        _doneThat.Should().BeTrue();
    }

    [Fact]
    public async Task ForEnabledFlag_DoAsync_DoThisAsync_NotThatAsync() {
        // Arrange
        _doneThis = false;
        _doneThat = false;

        // Act
        await _enabledFlag.DoAsync(_doThisAsync, _doThatAsync);

        // Assert
        _doneThis.Should().BeTrue();
        _doneThat.Should().BeFalse();
    }

    [Fact]
    public async Task ForDisabledFlag_DoAsync_DoNotThisAsync_DoThatAsync() {
        // Arrange
        _doneThis = false;
        _doneThat = false;

        // Act
        await _disabledFlag.DoAsync(_doThisAsync, _doThatAsync);

        // Assert
        _doneThis.Should().BeFalse();
        _doneThat.Should().BeTrue();
    }

    [Fact]
    public async Task ForEnabledFlag_DoAsync_DoThisAsync_NotThat() {
        // Arrange
        _doneThis = false;
        _doneThat = false;

        // Act
        await _enabledFlag.DoAsync(_doThisAsync, _doThat);

        // Assert
        _doneThis.Should().BeTrue();
        _doneThat.Should().BeFalse();
    }

    [Fact]
    public async Task ForDisabledFlag_DoAsync_DoNotThisAsync_DoThat() {
        // Arrange
        _doneThis = false;
        _doneThat = false;

        // Act
        await _disabledFlag.DoAsync(_doThisAsync, _doThat);

        // Assert
        _doneThis.Should().BeFalse();
        _doneThat.Should().BeTrue();
    }

    [Fact]
    public async Task ForEnabledFlag_DoAsync_DoThis_NotThatAsync() {
        // Arrange
        _doneThis = false;
        _doneThat = false;

        // Act
        await _enabledFlag.DoAsync(_doThis, _doThatAsync);

        // Assert
        _doneThis.Should().BeTrue();
        _doneThat.Should().BeFalse();
    }

    [Fact]
    public async Task ForDisabledFlag_DoAsync_DoNotThis_DoThatAsync() {
        // Arrange
        _doneThis = false;
        _doneThat = false;

        // Act
        await _disabledFlag.DoAsync(_doThis, _doThatAsync);

        // Assert
        _doneThis.Should().BeFalse();
        _doneThat.Should().BeTrue();
    }

    [Fact]
    public void ForEnabledFlag_GetOrDefault_GetThis() {
        // Act
        var result = _enabledFlag.GetOrDefault(_this);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public void ForDisabledFlag_GetOrDefault_GetDefault() {
        // Act
        var result = _disabledFlag.GetOrDefault(_this);

        // Assert
        result.Should().Be(0);
    }


    [Fact]
    public void ForEnabledFlag_Get_GetThis() {
        // Act
        var result = _enabledFlag.Get(_this, _that);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public void ForDisabledFlag_Get_GetThat() {
        // Act
        var result = _disabledFlag.Get(_this, _that);

        // Assert
        result.Should().Be(_that);
    }

    [Fact]
    public void ForEnabledFlag_GetOrDefault_WithThisFunc_GetThis() {
        // Act
        var result = _enabledFlag.GetOrDefault(_getThis);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public void ForDisabledFlag_GetOrDefault_WithThisFunc_GetDefault() {
        // Act
        var result = _disabledFlag.GetOrDefault(_getThis);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void ForEnabledFlag_Get_WithThisFunc_GetThis() {
        // Act
        var result = _enabledFlag.Get(_getThis, _that);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public void ForDisabledFlag_Get_WithThisFunc_GetThat() {
        // Act
        var result = _disabledFlag.Get(_getThis, _that);

        // Assert
        result.Should().Be(_that);
    }

    [Fact]
    public void ForEnabledFlag_Get_WithThatFunc_GetThis() {
        // Act
        var result = _enabledFlag.Get(_this, _getThat);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public void ForDisabledFlag_Get_WithThatFunc_GetThat() {
        // Act
        var result = _disabledFlag.Get(_this, _getThat);

        // Assert
        result.Should().Be(_that);
    }

    [Fact]
    public void ForEnabledFlag_Get_WithThisFunc_AndThatFunc_GetThis() {
        // Act
        var result = _enabledFlag.Get(_getThis, _getThat);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public void ForDisabledFlag_Get_WithThisFunc_AndThatFunc_GetThat() {
        // Act
        var result = _disabledFlag.Get(_getThis, _getThat);

        // Assert
        result.Should().Be(_that);
    }

    [Fact]
    public async Task ForEnabledFlag_GetOrDefaultAsync_GetThisAsync() {
        // Act
        var result = await _enabledFlag.GetOrDefaultAsync(_getThisAsync);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public async Task ForDisabledFlag_GetOrDefaultAsync_GetDefault() {
        // Act
        var result = await _disabledFlag.GetOrDefaultAsync(_getThisAsync);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ForEnabledFlag_GetAsync_GetThisAsync() {
        // Act
        var result = await _enabledFlag.GetAsync(_getThisAsync, _that);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public async Task ForDisabledFlag_GetOrDefaultAsync_GetThat() {
        // Act
        var result = await _disabledFlag.GetAsync(_getThisAsync, _that);

        // Assert
        result.Should().Be(_that);
    }

    [Fact]
    public async Task ForEnabledFlag_GetAsync_WithThatAsync_GetThisAsync() {
        // Act
        var result = await _enabledFlag.GetAsync(_this, _getThatAsync);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public async Task ForDisabledFlag_GetAsync_WithThatAsync_GetThatAsync() {
        // Act
        var result = await _disabledFlag.GetAsync(_this, _getThatAsync);

        // Assert
        result.Should().Be(_that);
    }

    [Fact]
    public async Task ForEnabledFlag_GetAsync_WithThis_AndThatAsync_GetThisAsync() {
        // Act
        var result = await _enabledFlag.GetAsync(_getThis, _getThatAsync);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public async Task ForDisabledFlag_GetAsync_WithThis_AndThatAsync_GetThatAsync() {
        // Act
        var result = await _disabledFlag.GetAsync(_getThis, _getThatAsync);

        // Assert
        result.Should().Be(_that);
    }

    [Fact]
    public async Task ForEnabledFlag_GetAsync_WithThisAsync_AndThat_GetThisAsync() {
        // Act
        var result = await _enabledFlag.GetAsync(_getThisAsync, _getThat);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public async Task ForDisabledFlag_GetAsync_WithThisAsync_AndThat_GetThatAsync() {
        // Act
        var result = await _disabledFlag.GetAsync(_getThisAsync, _getThat);

        // Assert
        result.Should().Be(_that);
    }

    [Fact]
    public async Task ForEnabledFlag_GetAsync_WithThisAsync_AndThatAsync_GetThisAsync() {
        // Act
        var result = await _enabledFlag.GetAsync(_getThisAsync, _getThatAsync);

        // Assert
        result.Should().Be(_this);
    }

    [Fact]
    public async Task ForDisabledFlag_GetAsync_WithThisAsync_AndThatAsync_GetThatAsync() {
        // Act
        var result = await _disabledFlag.GetAsync(_getThisAsync, _getThatAsync);

        // Assert
        result.Should().Be(_that);
    }
}