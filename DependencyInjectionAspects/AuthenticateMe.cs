using Castle.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionAspects
{

    [Interceptor(typeof())]
    public class AuthenticateMe
    {
        public string IAuthed()
        {
            return "Yay! I authed";
        }

    }


}
