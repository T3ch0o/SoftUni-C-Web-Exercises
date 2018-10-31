namespace SIS.Framework.Attributes.Methods
{
    using System;

    using SIS.Framework.Attributes.Methods.Base;

    public class HttpPostAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            return requestMethod.Equals("POST", StringComparison.OrdinalIgnoreCase);
        }
    }
}