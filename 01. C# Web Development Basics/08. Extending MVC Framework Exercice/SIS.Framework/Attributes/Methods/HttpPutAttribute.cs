namespace SIS.Framework.Attributes.Methods
{
    using System;

    using SIS.Framework.Attributes.Methods.Base;

    internal class HttpPutAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            return requestMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase);
        }
    }
}