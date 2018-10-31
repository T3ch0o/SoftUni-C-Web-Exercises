namespace SIS.Framework.Attributes.Action
{
    using System;
    using System.Linq;

    using SIS.Framework.Security;

    public class AuthorizeAttribute : Attribute
    {
        private readonly string[] _roles;

        public AuthorizeAttribute()
        {
        }

        public AuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        private bool IsIdentityPresent(IIdentity identity) => identity != null;

        private bool IsIdentityInRole(IIdentity identity)
        {
            if (!IsIdentityPresent(identity))
            {
                return false;
            }

            return identity.Roles.Any(identityRole => _roles.Contains(identityRole));
        }

        public bool IsAuthenticated(IIdentity identity)
        {
            if (_roles == null || !_roles.Any())
            {
                return IsIdentityPresent(identity);
            }

            return IsIdentityInRole(identity);
        }
    }
}