using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Assert.True(Container.IsEmpty);
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
                Assert.NotNull(user);
                user.Email = patchEmail;
                return user;
            });
            Assert.NotNull(user);
            Assert.IsType<User>(user);

            user = Container.Get<User>();
            Assert.NotNull(user);
            Assert.IsType<User>(user);
            Assert.Equal(user.Email, patchEmail);

        }
    }
}
