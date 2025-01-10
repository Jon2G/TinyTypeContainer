using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Types;
using TinyTypeContainer;
using Xunit.Priority;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Test
{
    [CollectionDefinition(nameof(RegisterTest), DisableParallelization = true), DefaultPriority(1)]
    public class RegisterTest
    {
        [Fact, Priority(0)]
        public void ContainerMustBeEmpty()
        {
            Container.Clear();
            Assert.True(Container.IsEmpty);
        }

        [Fact]
        public void RegisterType()
        {
            Container.Register<User>(new User()
            {
                Email = "new.email.com",
                Id = "2",
                Name = "New user"
            });
            Assert.NotNull(Container.Get<User>());
            Assert.True(Container.Has<User>());
            Assert.Equal(Container.GetRequired<User>().Id, "2");
        }

        [Fact]
        public void RegisterTypeTwiceShouldOverwrite()
        {
            Container.Register<User>(new User()
            {
                Email = "user.email.com",
                Id = "2",
                Name = "user"
            });
            Assert.Equal("2", Container.GetRequired<User>().Id);
            Container.Register(new User()
            {
                Email = "new.email.com",
                Id = "3",
                Name = "New user"
            });
            Assert.Equal("3", Container.GetRequired<User>().Id);
        }
    }
}
