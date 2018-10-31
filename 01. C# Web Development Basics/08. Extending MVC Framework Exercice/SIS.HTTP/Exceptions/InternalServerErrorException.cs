namespace SIS.HTTP.Exceptions
{
    using System;

    internal class InternalServerErrorException : Exception
    {
        public override string Message => "The Server has encountered an error.";
    }
}