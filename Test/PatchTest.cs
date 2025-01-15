using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Test.Types;
using TinyTypeContainer;
using Xunit.Priority;

namespace Test
{
    [CollectionDefinition(nameof(PatchTest), DisableParallelization = true), DefaultPriority(1)]
    public class PatchTest
    {
        [Fact, Priority(0)]
        public void ContainerMustBeEmpty()
        {
            Container.Clear();
            Container.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void Patch()
        {
            Container.Register(new User()
            {
                Email = "user.email.com",
                Id = "0",
                Name = "user"
            });
            string patchEmail = "patched.email@.com";
            var user = Container.Patch<User>((user) =>
            {
                user.Should().NotBeNull();
                user.Email = patchEmail;
                return user;
            });
            user.Should().NotBeNull();
            user.Should().BeOfType<User>();

            user = Container.Get<User>();
            user.Should().NotBeNull();
            user.Should().BeOfType<User>();
            user.Email.Should().Be(patchEmail);

        }
    }
}
