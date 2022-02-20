namespace StronglyTyped.FeatureFlags.Tests;

public class FlagsExtensionsTests {

    private readonly IFlag _enabledFlag = new Flag(true);
    private readonly IFlag _disabledFlag = new Flag(false);

    [Fact]
    public void ForEnabledFlag_Do_DoThis() {
        // Arrange
        var doneThis = false;

        // Act
        _enabledFlag.Do(() => doneThis = true);

        // Assert
        doneThis.Should().BeTrue();
    }

    [Fact]
    public void ForDisabledFlag_Do_DoNotThis() {
        // Arrange
        var doneThis = false;

        // Act
        _disabledFlag.Do(() => doneThis = true);

        // Assert
        doneThis.Should().BeFalse();
    }

    [Fact]
    public async Task ForEnabledFlag_DoAsync_DoThisAsync() {
        // Arrange
        var doneThis = false;

        // Act
        await _enabledFlag.DoAsync(() => {
            doneThis = true;
            return Task.CompletedTask;
        });

        // Assert
        doneThis.Should().BeTrue();
    }

    [Fact]
    public async Task ForDisabledFlag_DoAsync_DoNotThisAsync() {
        // Arrange
        var doneThis = false;

        // Act
        await _disabledFlag.DoAsync(() => {
            doneThis = true;
            return Task.CompletedTask;
        });

        // Assert
        doneThis.Should().BeFalse();
    }

    [Fact]
    public void ForEnabledFlag_Do_DoThis_NotThat() {
        // Arrange
        var doneThis = false;
        var doneThat = false;

        // Act
        _enabledFlag.Do(() => doneThis = true, () => doneThat = true);

        // Assert
        doneThis.Should().BeTrue();
        doneThat.Should().BeFalse();
    }


    [Fact]
    public void ForDisabledFlag_Do_DoNotThis_DoThat() {
        // Arrange
        var doneThis = false;
        var doneThat = false;

        // Act
        _disabledFlag.Do(() => doneThis = true, () => doneThat = true);

        // Assert
        doneThis.Should().BeFalse();
        doneThat.Should().BeTrue();
    }

    [Fact]
    public async Task ForEnabledFlag_DoAsync_DoThisAsync_NotThatAsync() {
        // Arrange
        var doneThis = false;
        var doneThat = false;

        // Act
        await _enabledFlag.DoAsync(() => {
            doneThis = true;
            return Task.CompletedTask;
        }, () => {
            doneThat = true;
            return Task.CompletedTask;
        });

        // Assert
        doneThis.Should().BeTrue();
        doneThat.Should().BeFalse();
    }

    [Fact]
    public async Task ForDisabledFlag_DoAsync_DoNotThisAsync_DoThatAsync() {
        // Arrange
        var doneThis = false;
        var doneThat = false;

        // Act
        await _disabledFlag.DoAsync(() => {
            doneThis = true;
            return Task.CompletedTask;
        }, () => {
            doneThat = true;
            return Task.CompletedTask;
        });

        // Assert
        doneThis.Should().BeFalse();
        doneThat.Should().BeTrue();
    }

    [Fact]
    public async Task ForEnabledFlag_DoAsync_DoThisAsync_NotThat() {
        // Arrange
        var doneThis = false;
        var doneThat = false;

        // Act
        await _enabledFlag.DoAsync(() => {
            doneThis = true;
            return Task.CompletedTask;
        }, () => doneThat = true);

        // Assert
        doneThis.Should().BeTrue();
        doneThat.Should().BeFalse();
    }

    [Fact]
    public async Task ForDisabledFlag_DoAsync_DoNotThisAsync_DoThat() {
        // Arrange
        var doneThis = false;
        var doneThat = false;

        // Act
        await _disabledFlag.DoAsync(() => {
            doneThis = true;
            return Task.CompletedTask;
        }, () => doneThat = true);

        // Assert
        doneThis.Should().BeFalse();
        doneThat.Should().BeTrue();
    }

    [Fact]
    public async Task ForEnabledFlag_DoAsync_DoThis_NotThatAsync() {
        // Arrange
        var doneThis = false;
        var doneThat = false;

        // Act
        await _enabledFlag.DoAsync(() => doneThis = true, () => {
            doneThat = true;
            return Task.CompletedTask;
        });

        // Assert
        doneThis.Should().BeTrue();
        doneThat.Should().BeFalse();
    }

    [Fact]
    public async Task ForDisabledFlag_DoAsync_DoNotThis_DoThatAsync() {
        // Arrange
        var doneThis = false;
        var doneThat = false;

        // Act
        await _disabledFlag.DoAsync(() => doneThis = true, () => {
            doneThat = true;
            return Task.CompletedTask;
        });

        // Assert
        doneThis.Should().BeFalse();
        doneThat.Should().BeTrue();
    }

    [Fact]
    public void ForEnabledFlag_GetOrDefault_GetThis() {
        // Act
        var result = _enabledFlag.GetOrDefault(42);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void ForDisabledFlag_GetOrDefault_GetDefault() {
        // Act
        var result = _disabledFlag.GetOrDefault(42);

        // Assert
        result.Should().Be(0);
    }


    [Fact]
    public void ForEnabledFlag_Get_GetThis() {
        // Act
        var result = _enabledFlag.Get(42, 13);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void ForDisabledFlag_Get_GetThat() {
        // Act
        var result = _disabledFlag.Get(42, 13);

        // Assert
        result.Should().Be(13);
    }

    [Fact]
    public void ForEnabledFlag_GetOrDefault_WithThisFunc_GetThis() {
        // Act
        var result = _enabledFlag.GetOrDefault(() => 42);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void ForDisabledFlag_GetOrDefault_WithThisFunc_GetDefault() {
        // Act
        var result = _disabledFlag.GetOrDefault(() => 42);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void ForEnabledFlag_Get_WithThisFunc_GetThis() {
        // Act
        var result = _enabledFlag.Get(() => 42, 13);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void ForDisabledFlag_Get_WithThisFunc_GetThat() {
        // Act
        var result = _disabledFlag.Get(() => 42, 13);

        // Assert
        result.Should().Be(13);
    }

    [Fact]
    public void ForEnabledFlag_Get_WithThatFunc_GetThis() {
        // Act
        var result = _enabledFlag.Get(42, () => 13);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void ForDisabledFlag_Get_WithThatFunc_GetThat() {
        // Act
        var result = _disabledFlag.Get(42, () => 13);

        // Assert
        result.Should().Be(13);
    }

    [Fact]
    public void ForEnabledFlag_Get_WithThisFunc_AndThatFunc_GetThis() {
        // Act
        var result = _enabledFlag.Get(() => 42, () => 13);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void ForDisabledFlag_Get_WithThisFunc_AndThatFunc_GetThat() {
        // Act
        var result = _disabledFlag.Get(() => 42, () => 13);

        // Assert
        result.Should().Be(13);
    }

    [Fact]
    public async Task ForEnabledFlag_GetOrDefaultAsync_GetThisAsync() {
        // Act
        var result = await _enabledFlag.GetOrDefaultAsync(() => Task.FromResult(42));

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task ForDisabledFlag_GetOrDefaultAsync_GetDefault() {
        // Act
        var result = await _disabledFlag.GetOrDefaultAsync(() => Task.FromResult(42));

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ForEnabledFlag_GetAsync_GetThisAsync() {
        // Act
        var result = await _enabledFlag.GetAsync(() => Task.FromResult(42), 13);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task ForDisabledFlag_GetOrDefaultAsync_GetThat() {
        // Act
        var result = await _disabledFlag.GetAsync(() => Task.FromResult(42), 13);

        // Assert
        result.Should().Be(13);
    }

    [Fact]
    public async Task ForEnabledFlag_GetAsync_WithThatAsync_GetThisAsync() {
        // Act
        var result = await _enabledFlag.GetAsync(42, () => Task.FromResult(13));

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task ForDisabledFlag_GetAsync_WithThatAsync_GetThatAsync() {
        // Act
        var result = await _disabledFlag.GetAsync(42, () => Task.FromResult(13));

        // Assert
        result.Should().Be(13);
    }

    [Fact]
    public async Task ForEnabledFlag_GetAsync_WithThisAsync_AndThatAsync_GetThisAsync() {
        // Act
        var result = await _enabledFlag.GetAsync(() => Task.FromResult(42), () => Task.FromResult(13));

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task ForDisabledFlag_GetAsync_WithThisAsync_AndThatAsync_GetThatAsync() {
        // Act
        var result = await _disabledFlag.GetAsync(() => Task.FromResult(42), () => Task.FromResult(13));

        // Assert
        result.Should().Be(13);
    }
}