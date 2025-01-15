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
    [CollectionDefinition(nameof(GetTest), DisableParallelization = true), DefaultPriority(1)]
    public class GetTest
    {
        [Fact, Priority(0)]
        public void ContainerMustBeEmpty()
        {
            Container.Clear();
            Container.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void GetorActivate()
        {
            var user = Container.GetorActivate<User>();
            user.Should().NotBeNull().And.Subject.Should().BeOfType<User>();
        }

        [Fact]
        public void GetByTypeArgument()
        {
            var user = Container.Get(typeof(User));
            user.Should().NotBeNull().And.Subject.Should().BeOfType<User>();
            ((User)user).Name = "Jon2G";
            ((User)user).Id = "1";
            ((User)user).Email = "dummy.email@.com";
        }

        [Fact]
        public void Get()
        {
            var user = Container.Get<User>();
            user.Should().NotBeNull().And.Subject.Should().BeOfType<User>();
        }

        [Fact]
        public void GetRequiredShouldNotReturnNullForRegistered()
        {
            var user = Container.GetRequired<User>();
            user.Should().NotBeNull().And.Subject.Should().BeOfType<User>();
        }

        [Fact]
        public void GetRequiredShouldFailIfNotRegistered()
        {
            Assert.Throws(typeof(InvalidOperationException), () =>
            {
                Container.GetRequired<DummyType>();
            });
        }

        [Fact]
        public void GetByInterface()
        {
            var ihaveId = Container.Get<IHaveId>();
            ihaveId.Should().NotBeNull().And.Subject.Should().BeOfType<User>();
        }
    }
}
