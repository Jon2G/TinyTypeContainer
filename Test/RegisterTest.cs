using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
            Container.IsEmpty.Should().BeTrue();
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
            Container.Get<User>().Should().NotBeNull();
            Container.Has<User>().Should().BeTrue();
            Container.GetRequired<User>().Id.Should().Be("2");
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
            Container.GetRequired<User>().Id.Should().Be("2");
            Container.Register(new User()
            {
                Email = "new.email.com",
                Id = "3",
                Name = "New user"
            });
            Container.GetRequired<User>().Id.Should().Be("3");
        }
    }
}
