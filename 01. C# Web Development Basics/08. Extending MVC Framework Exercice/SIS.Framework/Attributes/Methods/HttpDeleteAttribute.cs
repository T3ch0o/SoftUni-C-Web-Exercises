namespace SIS.Framework.Attributes.Methods
{
    using System;

    using SIS.Framework.Attributes.Methods.Base;

    internal class HttpDeleteAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            return requestMethod.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
        }
    }
}