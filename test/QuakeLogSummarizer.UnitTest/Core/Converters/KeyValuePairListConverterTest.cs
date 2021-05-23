using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoFixture;
using FluentAssertions;
using QuakeLogSummarizer.Core.Converters;
using Xunit;

namespace QuakeLogSummarizer.UnitTest.Core.Converters
{
    public sealed class KeyValuePairListConverterTest
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly Fixture _fixture;

        public KeyValuePairListConverterTest()
        {
            this._fixture = new Fixture();

            this._jsonSerializerOptions = new JsonSerializerOptions();
            this._jsonSerializerOptions.WriteIndented = false;
            this._jsonSerializerOptions.Converters.Add(new KeyValuePairListConverter());

        }

        [Fact]
        private void Serialize_When_ListIsEmpty_Should_SerializeEmptyDictionaryJson()
        {
            // Arrange
            IEnumerable<(string, int)> keyValuePairList = new List<(string, int)>();

            // Act
            string actual = JsonSerializer.Serialize(keyValuePairList, this._jsonSerializerOptions);

            // Assert
            IDictionary<string, int> actualDictionary = JsonSerializer.Deserialize<IDictionary<string, int>>(actual);
            actualDictionary.Should().BeEmpty();
        }

        [Fact]
        private void Serialize_When_ListContainsElements_Should_SerializeAsDictionaryJson()
        {
            // Arrange
            IEnumerable<(string Key, int Value)> keyValuePairList = this._fixture.CreateMany<(string, int)>();

            // Act
            string actual = JsonSerializer.Serialize(keyValuePairList, this._jsonSerializerOptions);

            // Assert
            IDictionary<string, int> actualDictionary = JsonSerializer.Deserialize<IDictionary<string, int>>(actual);
            IEnumerable<KeyValuePair<string, int>> expectedKeyValuePairList = keyValuePairList.Select(p => new KeyValuePair<string, int>(p.Key, p.Value));

            actualDictionary.Should().Contain(expectedKeyValuePairList);
        }
    }
}
