using Castle.Core;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyInjectionAspects
{

    public class AuthorizationInterceptor : IInterceptor
    {



        internal bool IsAuthorizedForRole(int userId, IEnumerable<AuthRoles> roles)
        {

            return roles.Contains(AuthRoles.Normal);
        }

        public void Intercept(IInvocation invocation)
        {
            // reliably consuming a userId would be awkward, could use an attribute to specify when auth
            // param is different

            //TODO: use attributes to set permissions and get user from args 
            int userId = (int)invocation.Arguments[0];
            IEnumerable<AuthRoles> roles = GetTargetAllowedRoles(invocation);
            if(IsAuthorizedForRole(userId, roles))
            {
                invocation.Proceed();
            }
            else
            {
                throw new Exception("oh man, not authed");
            }
        }

        private IEnumerable<AuthRoles> GetTargetAllowedRoles(IInvocation invocation)
        {
            IEnumerable<AuthorizeRolesAttribute> attributes = invocation.MethodInvocationTarget.GetCustomAttributes(true).OfType<AuthorizeRolesAttribute>();
            IEnumerable<AuthRoles> allowedRoles = attributes.SelectMany(attr => attr.AllowedRoles);
            return allowedRoles;
        }
    }
}
