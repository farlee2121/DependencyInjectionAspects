using Castle.Core;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionAspects
{
    public class AuthorizationInterceptor : IInterceptor
    {

        public void Authorize(IInvocation invocation)
        {
            Console.WriteLine("Well, it ran");
        }

        public void Intercept(IInvocation invocation)
        {
            Authorize(invocation);
            invocation.Proceed();
        }
    }
}
