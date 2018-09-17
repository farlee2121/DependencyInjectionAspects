using Castle.Core;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionAspects
{
    class AuthenticationInterceptor : StandardInterceptor
    {
        public AuthenticationInterceptor()
        {
            Authenticate();
        }

        public void Authenticate()
        {
            Console.WriteLine("Well, it ran");
        }
    }
}
