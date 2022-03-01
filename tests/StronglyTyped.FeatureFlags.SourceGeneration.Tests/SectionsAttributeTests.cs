﻿namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

[ExcludeFromCodeCoverage]
public class SectionsAttributeTests {
    [Fact]
    public void SectionsAttribute_IsCreated() {
        // Arrange
        var attribute = new SectionsAttribute();

        // Assert
        attribute.Should().NotBeNull();
    }
}