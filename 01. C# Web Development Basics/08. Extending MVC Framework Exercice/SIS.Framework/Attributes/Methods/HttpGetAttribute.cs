namespace SIS.Framework.Attributes.Methods
{
    using System;

    using SIS.Framework.Attributes.Methods.Base;

    public class HttpGetAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            return requestMethod.Equals("GET", StringComparison.OrdinalIgnoreCase);
        }
    }
}