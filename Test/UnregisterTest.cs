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
    [CollectionDefinition(nameof(UnregisterTest), DisableParallelization = true)]
    public class UnregisterTest
    {
        [Fact, Priority(0)]
        public void ContainerMustBeEmpty()
        {
            Container.Clear();
            Container.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void UnregisterType()
        {
            Container.Register(new User());
            Assert.NotNull(Container.GetRequired<User>());

            Container.Unregister<User>();
            Container.Get<User>().Should().BeNull();
            Container.Has<User>().Should().BeFalse();
        }

        [Fact]
        public void UnregisterNotRegisteredTypeShouldFail()
        {
            Assert.Throws(typeof(InvalidOperationException), () =>
            {
                Container.Unregister<DummyType>();
            });
        }

        [Fact]
        public void HasShouldReturnFalseForNotRegistered()
        {
            Container.Has<User>().Should().BeFalse();
        }

    }
}
