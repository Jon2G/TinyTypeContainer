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
    [CollectionDefinition(nameof(UnregisterTest), DisableParallelization = true)]
    public class UnregisterTest
    {
        [Fact, Priority(0)]
        public void ContainerMustBeEmpty()
        {
            Container.Clear();
            Assert.True(Container.IsEmpty);
        }

        [Fact]
        public void UnregisterType()
        {
            Container.Register(new User());
            Assert.NotNull(Container.GetRequired<User>());

            Container.Unregister<User>();
            Assert.Null(Container.Get<User>());
            Assert.False(Container.Has<User>());
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
            Assert.False(Container.Has<DummyType>());
        }

    }
}
