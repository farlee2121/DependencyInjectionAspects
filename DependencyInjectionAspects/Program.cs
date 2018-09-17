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
            var container = new WindsorContainer();
            container.Register(Component.For<Runner>());
            container.Register(Component.For<AuthenticateMe>().ImplementedBy<AuthenticateMe>());
            container.Register(Component.For<AuthenticationInterceptor>().LifeStyle.Transient);

            var runner = container.Resolve<Runner>();
            runner.DoTheThing();
        }
    }

    public class Runner
    {
        private readonly AuthenticateMe authMe;

        public Runner(AuthenticateMe authMe)
        {
            this.authMe = authMe;
        }

        public void DoTheThing()
        {
            try
            {
                var result = authMe.IAuthed();
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
