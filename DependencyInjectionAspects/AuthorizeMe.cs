using Castle.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionAspects
{

    [Interceptor(typeof(AuthorizationInterceptor))]
    public class AuthorizeMe
    {
        public virtual string IAuthed(int userId)
        {
            return "Yay! I authed";
        }

    }


}
