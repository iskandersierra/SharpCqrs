using System.IO;
using System.Text;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SharpCqrs.Infrastructure
{
    [Binding]
    public sealed class JsonPortableModelBinderSteps
    {
        [Given(@"A JSON Portable model binder")]
        public void GivenAJSONPortableModelBinder()
        {
            new JsonPortableModelBinder().TestValue();
        }
        [Given(@"A json content")]
        public void GivenAJsonContent()
        {
            var json = @"{ ""firstName"": ""John"", ""lastName"": ""Doe"" }";
            json.TestValue("json");
        }
        [Given(@"An encoding of ""(.*)""")]
        public void GivenAnEncodingOf(string webName)
        {
            Encoding.GetEncoding(webName).TestValue();
        }
        [Given(@"The encoded binary content of the json")]
        public void GivenTheEncodedBinaryContentOfTheJson()
        {
            Encoding encoding = ScenarioContext.Current.Get<Encoding>();
            string json = ScenarioContext.Current.Get<string>(nameof(json));
            var binary = encoding.GetBytes(json);
            binary.TestValue(nameof(binary));
        }

        [When(@"The binary content is deserialized")]
        public void WhenTheJsonContentIsDeserialized()
        {
            var binder = ScenarioContext.Current.Get<JsonPortableModelBinder>();
            Encoding encoding = ScenarioContext.Current.Get<Encoding>();
            string json = ScenarioContext.Current.Get<string>(nameof(json));
            byte[] binary = ScenarioContext.Current.Get<byte[]>(nameof(binary));
            using (var stream = new MemoryStream(binary, writable: false))
            {
                var instance = binder.Bind(new PortableBindingContext
                {
                    Content = stream,
                    Encoding = encoding,
                    Type = typeof(PersonData),
                });
                instance.TestValue(nameof(instance));
            }
        }

        [Then(@"The bound object is the expected one")]
        public void ThenTheBoundObjectIsTheExpectedOne()
        {
            object instance = ScenarioContext.Current.Get<object>(nameof(instance));

            instance.Should().NotBeNull();
            instance.Should().BeOfType<PersonData>();
            var person = (PersonData)instance;
            person.FirstName.Should().Be("John");
            person.LastName.Should().Be("Doe");
        }

        public class PersonData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
