namespace SIS.Framework.Attributes.Properties
{
    using System.ComponentModel.DataAnnotations;

    public class NumberRangeAttribute : ValidationAttribute
    {
        private readonly double _minValue;

        private readonly double _maxValue;

        public NumberRangeAttribute(double minValue, double maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            double valueAsNumber = (double)value;

            return valueAsNumber >= _minValue && valueAsNumber <= _maxValue;
        }
    }
}