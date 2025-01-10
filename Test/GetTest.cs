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
    [CollectionDefinition(nameof(GetTest), DisableParallelization = true),DefaultPriority(1)]
    public class GetTest
    {
        [Fact, Priority(0)]
        public void ContainerMustBeEmpty()
        {
            Container.Clear();
            Assert.True(Container.IsEmpty);
        }

        [Fact]
        public void GetorActivate()
        {
            var user = Container.GetorActivate<User>();
            Assert.NotNull(user);
            Assert.IsType<User>(user);
        }

        [Fact]
        public void GetByTypeArgument()
        {
            var user = Container.Get(typeof(User));
            Assert.NotNull(user);
            Assert.IsType<User>(user);
            ((User)user).Name = "Jon2G";
            ((User)user).Id = "1";
            ((User)user).Email = "dummy.email@.com";
        }

        [Fact]
        public void Get()
        {
            var user = Container.Get<User>();
            Assert.NotNull(user);
            Assert.IsType<User>(user);
        }

        [Fact]
        public void GetRequiredShouldNotReturnNullForRegistered()
        {
            var user = Container.GetRequired<User>();
            Assert.NotNull(user);
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
            Assert.NotNull(ihaveId);
            Assert.IsType<User>(ihaveId);
        }
    }
}
