using Castle.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionAspects
{


    //[Interceptor(typeof(AuthorizationInterceptor))]
    public class AuthorizeMe : IAuthorizeMe
    {
        
        public string IAuthed(int userId)
        {
            return "Yay! I authed";
        }

        
        public string INotAuthed(int userId)
        {
            return "you shouldn't see this. it's not authorized";
        }

    }


}
