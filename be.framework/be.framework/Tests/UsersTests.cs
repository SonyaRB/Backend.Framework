using NUnit.Framework;
using System.Threading.Tasks;
using System.Net;
using FluentAssertions;
using Backend.Framework.Utilities;

namespace Backend.Framework.Tests
{
    public class UsersTests : BaseTests
    {
        [Test]
        public async Task GetUsersSuccessfully()
        {
            var response = await actions.GetRequest(Properties.baseUrl + Properties.users);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetTokenSuccessfully()
        {
            var token = await actions.ObtainJWToken(Properties.authPath);
            token.Should().NotBeNull();
        }
    }
}
