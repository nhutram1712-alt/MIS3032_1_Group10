using Xunit;

namespace SmartMaintenance.Tests.Integration;

[CollectionDefinition("Integration")]
public sealed class IntegrationCollection : ICollectionFixture<CustomWebApplicationFactory>
{
}
