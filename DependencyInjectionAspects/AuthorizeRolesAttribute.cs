using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionAspects
{

    public enum AuthRoles
    {
        Normal,
        Super
    }
    public class AuthorizeRolesAttribute : Attribute
    {
        public AuthRoles[] AllowedRoles { get; set; }

        public AuthorizeRolesAttribute(params AuthRoles[] allowedRoles)
        {
            AllowedRoles = allowedRoles;
        }
    }
}
