using Microsoft.Extensions.DependencyInjection;

using StronglyTyped.FeatureFlags;

using System;

using Xunit;

namespace StronglyTyped.FeatureFlags.Tests {
    public class FeatureFlagsFactoryTests {
        [Fact]
        public void AddProvider_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var factory = new FeatureFlagsFactory(TODO);

            // Act
            factory.AddProvider();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void For_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var factory = new FeatureFlagsFactory(TODO);
            string name = null;

            // Act
            var result = factory.For(
                name);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Dispose_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var factory = new FeatureFlagsFactory(TODO);

            // Act
            factory.Dispose();

            // Assert
            Assert.True(false);
        }
    }
}
