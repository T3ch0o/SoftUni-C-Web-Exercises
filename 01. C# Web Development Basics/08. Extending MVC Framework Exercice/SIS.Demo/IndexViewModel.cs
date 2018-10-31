namespace SIS.Demo
{
    using SIS.Framework.Attributes.Properties;

    internal class IndexViewModel
    {
        [NumberRange(5, 12)]
        public double Id { get; set; }

        [Regex(@"^[a-zA-Z]+$")]
        public string Username { get; set; }
    }
}