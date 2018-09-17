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
            container.Register(Component.For<Runner>());

            //container.Register(Component.For<AuthorizationInterceptor>().ImplementedBy<AuthorizationInterceptor>().LifeStyle.Transient,
            //    Component.For<AuthorizeMe>().ImplementedBy<AuthorizeMe>()
            //    .Interceptors(InterceptorReference.ForType<AuthorizationInterceptor>()).Anywhere,
            //    Component.For<Runner>());

            var runner = container.Resolve<Runner>();
            runner.DoTheThing();
        }
    }

    public class Runner
    {
        private readonly AuthorizeMe authMe;

        private readonly int UserId = 0;

        public Runner(AuthorizeMe authMe)
        {
            this.authMe = authMe;
        }

        public void DoTheThing()
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
            Console.ReadKey();
            
        }
    }
}
