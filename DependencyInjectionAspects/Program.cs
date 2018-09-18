using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;

namespace DependencyInjectionAspects
{
    class Program
    {
        static void Main(string[] args)
        {
            // windsor docs https://github.com/castleproject/Windsor/blob/master/docs/README.md

            // windsor AOP tutorial: https://lukemerrett.com/aop-in-castle-windsor/
            var container = new WindsorContainer();
            container.Register(Component.For<AuthorizationInterceptor>().LifeStyle.Transient);
            container.Register(Component.For<AuthorizeMe>().ImplementedBy<AuthorizeMe>());
            container.Register(Component.For<TestRunner>());

            //container.Register(Component.For<AuthorizationInterceptor>().ImplementedBy<AuthorizationInterceptor>().LifeStyle.Transient,
            //    Component.For<AuthorizeMe>().ImplementedBy<AuthorizeMe>()
            //    .Interceptors(InterceptorReference.ForType<AuthorizationInterceptor>()).Anywhere,
            //    Component.For<Runner>());

            var runner = container.Resolve<TestRunner>();

            runner.RunTests();
            
        }
    }

    public class TestRunner
    {
        private readonly AuthorizeMe authMe;

        private readonly int UserId = 0;

        public void RunTests()
        {
            AuthorizedRoleTest();
            UnauthorizedRoleTest();
            Console.ReadKey();
        }

        public TestRunner(AuthorizeMe authMe)
        {
            this.authMe = authMe;
        }

        private void AuthorizedRoleTest()
        {
            try
            {
                var result = authMe.IAuthed(UserId);
                Console.WriteLine(result);
            }
            catch (Exception)
            {
                Console.WriteLine("oops");
                throw;
            }
            
            
        }

        private void UnauthorizedRoleTest()
        {
            try
            {
                var result = authMe.INotAuthed(UserId);
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
