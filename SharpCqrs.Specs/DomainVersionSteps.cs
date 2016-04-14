using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SharpCqrs
{
    [Binding]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class DomainVersionSteps
    {
        [Given(@"A new major version is created with (.*)")]
        public void GivenANewMajorVersionIsCreatedWith(int major)
        {
            new DomainVersion(major).TestValue();
        }
        [Given(@"A new major\.minor version is created with (.*) and (.*)")]
        public void GivenANewMajor_MinorVersionIsCreatedWithAnd(int major, int minor)
        {
            new DomainVersion(major, minor).TestValue();
        }
        [Given(@"A new major\.minor\.revision version is created with (.*), (.*) and (.*)")]
        public void GivenANewMajor_Minor_RevisionVersionIsCreatedWithAnd(int major, int minor, int revision)
        {
            new DomainVersion(major, minor, revision).TestValue();
        }
        [Given(@"A new major\.minor\.revision\.build version is created with (.*), (.*), (.*) and (.*)")]
        public void GivenANewMajor_Minor_Revision_BuildVersionIsCreatedWithAnd(int major, int minor, int revision, int build)
        {
            new DomainVersion(major, minor, revision, build).TestValue();
        }
        [Given(@"A new major\.minor\.details version is created with (.*), (.*), ""(.*)"" and ""(.*)""")]
        public void GivenANewMajor_Minor_DetailsVersionIsCreatedWithAnd(int major, int minor, string details1, string details2)
        {
            new DomainVersion(major, minor, details1, details2).TestValue();
        }
        [Given(@"A new details version is created with ""(.*)"", ""(.*)"" and ""(.*)""")]
        public void GivenANewMajor_Minor_DetailsVersionIsCreatedWithAnd(string details1, string details2, string details3)
        {
            new DomainVersion(details1, details2, details3).TestValue();
        }
        [Given(@"A string ""(.*)"" is parsed as a version")]
        public void GivenAStringIsParsedAsAVersion(string printed)
        {
            SpecUtils.TestValueProtected(() => DomainVersion.Parse(printed));
        }
        [Given(@"A string ""(.*)"" is tried to be parsed as a version")]
        public void GivenAStringIsTriedToBeParsedAsAVersion(string printed)
        {
            DomainVersion version;
            bool succeed = DomainVersion.TryParse(printed, out version);

            succeed.TestValue(nameof(succeed));
            if (succeed) version.TestValue();
        }
        [Given(@"A version ""(.*)"" is parsed and stored in ""(.*)""")]
        public void GivenAVersionIsParsedAndStoredIn(string version, string variable)
        {
            SpecUtils.TestValue(() => DomainVersion.Parse(version), variable);
        }

        [When(@"The version is printed")]
        public void WhenTheVersionIsPrinted()
        {
            var version = ScenarioContext.Current.Get<DomainVersion>();
            var printed = version.ToString();
            printed.TestValue(nameof(printed));
        }

        [Then(@"The printed version looks like ""(.*)""")]
        public void ThenThePrintedVersionLooksLike(string like)
        {
            string printed = ScenarioContext.Current.Get<string>(nameof(printed));

            printed.Should().Be(like);
        }

        [Then(@"The major version looks like ""(.*)""")]
        public void ThenTheMajorVersionLooksLike(string major)
        {
            var version = ScenarioContext.Current.Get<DomainVersion>();
            version.MajorVersion.Should().Be(major);
        }
        [Then(@"The minor version looks like ""(.*)""")]
        public void ThenTheMinorVersionLooksLike(string minor)
        {
            var version = ScenarioContext.Current.Get<DomainVersion>();
            version.MinorVersion.Should().Be(minor);
        }

        [Then(@"The revision version looks like ""(.*)""")]
        public void ThenTheRevisionVersionLooksLike(string revision)
        {
            var version = ScenarioContext.Current.Get<DomainVersion>();
            version.RevisionNumber.Should().Be(revision);
        }

        [Then(@"The build version looks like ""(.*)""")]
        public void ThenTheBuildVersionLooksLike(string build)
        {
            var version = ScenarioContext.Current.Get<DomainVersion>();
            version.BuildNumber.Should().Be(build);
        }
        [Then(@"The parsing attempt succeed")]
        public void ThenTheParsingAttemptSucceed()
        {
            bool succeed = ScenarioContext.Current.Get<bool>(nameof(succeed));

            succeed.Should().BeTrue("because the version had a correct format");
        }
        [Then(@"The parsing attempt failed")]
        public void ThenTheParsingAttemptFailed()
        {
            bool succeed = ScenarioContext.Current.Get<bool>(nameof(succeed));

            succeed.Should().BeFalse("because the version had a wrong format");
        }
        [Then(@"The parsing succeed")]
        public void ThenTheParsingSucceed()
        {
            SpecUtils.NoExceptionExpected();
        }
        [Then(@"The parsing failed")]
        public void ThenTheParsingFailed()
        {
            SpecUtils.ExceptionExpected<FormatException>();
        }
        [Then(@"Version ""(.*)"" compared with ""(.*)"" gives ""(.*)""")]
        public void ThenVersionComparedWithGives(string v1, string v2, int comparison)
        {
            var version1 = ScenarioContext.Current.Get<DomainVersion>(v1);
            var version2 = ScenarioContext.Current.Get<DomainVersion>(v2);
            var comp = version1.CompareTo(version2);
            comp.Should().Be(comparison, "because it should be the result of the comparison");
        }
        [Then(@"Version ""(.*)"" equals ""(.*)"" gives ""(.*)""")]
        public void ThenVersionEqualsGives(string v1, string v2, bool equals)
        {
            var version1 = ScenarioContext.Current.Get<DomainVersion>(v1);
            var version2 = ScenarioContext.Current.Get<DomainVersion>(v2);
            var result = version1.Equals(version2);
            result.Should().Be(equals, "because it should be the result of the equality");
        }
        [Then(@"Version ""(.*)"" and ""(.*)"" hash codes are ""(.*)""")]
        public void ThenVersionAndHashCodesAre(string v1, string v2, bool equals)
        {
            if (equals)
            {
                var version1 = ScenarioContext.Current.Get<DomainVersion>(v1);
                var version2 = ScenarioContext.Current.Get<DomainVersion>(v2);
                var same = version1.GetHashCode() == version2.GetHashCode();
                same.Should().BeTrue("because it should be the result of the hash code comparison");
            }
        }
        [Then(@"Version ""(.*)"" eq ""(.*)"" gives ""(.*)""")]
        public void ThenVersionEqGives(string v1, string v2, bool expected)
        {
            var version1 = ScenarioContext.Current.Get<DomainVersion>(v1);
            var version2 = ScenarioContext.Current.Get<DomainVersion>(v2);
            var result = version1 == version2;
            result.Should().Be(expected, "because it should be the result of v1 == v2");
        }

        [Then(@"Version ""(.*)"" ne ""(.*)"" gives ""(.*)""")]
        public void ThenVersionNeGives(string v1, string v2, bool expected)
        {
            var version1 = ScenarioContext.Current.Get<DomainVersion>(v1);
            var version2 = ScenarioContext.Current.Get<DomainVersion>(v2);
            var result = version1 != version2;
            result.Should().Be(expected, "because it should be the result of v1 != v2");
        }

        [Then(@"Version ""(.*)"" lt ""(.*)"" gives ""(.*)""")]
        public void ThenVersionLtGives(string v1, string v2, bool expected)
        {
            var version1 = ScenarioContext.Current.Get<DomainVersion>(v1);
            var version2 = ScenarioContext.Current.Get<DomainVersion>(v2);
            var result = version1 < version2;
            result.Should().Be(expected, "because it should be the result of v1 < v2");
        }

        [Then(@"Version ""(.*)"" gt ""(.*)"" gives ""(.*)""")]
        public void ThenVersionGtGives(string v1, string v2, bool expected)
        {
            var version1 = ScenarioContext.Current.Get<DomainVersion>(v1);
            var version2 = ScenarioContext.Current.Get<DomainVersion>(v2);
            var result = version1 > version2;
            result.Should().Be(expected, "because it should be the result of v1 > v2");
        }

        [Then(@"Version ""(.*)"" le ""(.*)"" gives ""(.*)""")]
        public void ThenVersionLeGives(string v1, string v2, bool expected)
        {
            var version1 = ScenarioContext.Current.Get<DomainVersion>(v1);
            var version2 = ScenarioContext.Current.Get<DomainVersion>(v2);
            var result = version1 <= version2;
            result.Should().Be(expected, "because it should be the result of v1 <= v2");
        }

        [Then(@"Version ""(.*)"" ge ""(.*)"" gives ""(.*)""")]
        public void ThenVersionGeGives(string v1, string v2, bool expected)
        {
            var version1 = ScenarioContext.Current.Get<DomainVersion>(v1);
            var version2 = ScenarioContext.Current.Get<DomainVersion>(v2);
            var result = version1 >= version2;
            result.Should().Be(expected, "because it should be the result of v1 >= v2");
        }
    }
}
