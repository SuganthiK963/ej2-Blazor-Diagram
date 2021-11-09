using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// Interface for validator.
    /// </summary>
    /// <exclude/>
    public interface IValidator
    {
        bool IsValid(object value, object arguments);
    }

    /// <summary>
    /// Validation context that shares the current validated field details.
    /// </summary>
    /// <exclude/>
    public class ValidationContext
    {
        public string FieldName { get; set; }

        public object ValidationRules { get; set; }
    }

    /// <summary>
    /// Validation result of the currently done validation.
    /// </summary>
    /// <exclude/>
    public class ValidationResult
    {
        public string FieldName { get; set; }

        public bool IsValid { get; set; }

        public string Rule { get; set; }

        public string Message { get; set; }
    }

    /// <summary>
    /// Validator that handles column validation.
    /// </summary>
    /// <exclude/>
    internal class Validator
    {
        private IDictionary<string, IValidator> validatorOfRules = new Dictionary<string, IValidator>()
        {
            { "required", new RequiredValidator() },
            { "rangeLength", new StringLengthValidator() },
            { "range", new RangeValidator() },
            { "minLength", new MinLengthValidator() },
            { "maxLength", new MaxLengthValidator() },
            { "regex", new RegexValidator() },
            { "email", new EmailAddressValidator() },
            { "number", new NumberValidator() },
            { "max", new MaxValidator() },
            { "min", new MinValidator() }
        };

        internal string[] availableRules
        {
            get
            {
                return validatorOfRules.Keys.ToArray();
            }
        }

        private IDictionary<string, string> defaultMessages = new Dictionary<string, string>()
        {
            { "required", "This field is required" },
            { "rangeLength", "Please enter a value between {0} and {1} characters long." },
            { "range", "Please enter a value between {0} and {1}." },
            { "minLength", "Please enter at least {0} characters." },
            { "maxLength", "Please enter no more than {0} characters." },
            { "regex", "Please enter a correct value." },
            { "email", "Please enter a valid email address" },
            { "number", "Please enter a valid number." },
            { "max", "Please enter a value less than or equal to {0}." },
            { "min", "Please enter a value greater than or equal to {0}." }
        };

        public void TryValidate(object value, Type propertyType, in ValidationContext context, in ValidationResult result)
        {
            if (context.ValidationRules == null)
            {
                result.IsValid = true;
            }
            else
            {
                ValueTuple<string, object>[] rules = GetRules(context.ValidationRules, propertyType);

                if (!rules.Any())
                {
                    result.IsValid = true;
                } // When no rule is found return true.

                foreach (ValueTuple<string, object> rule in rules)
                {
                    bool isValid = GetValidator(rule.Item1).IsValid(value, rule.Item2);
                    result.IsValid = isValid;
                    if (!result.IsValid)
                    {
                        result.Rule = rule.Item1;
                        result.Message = GetMessage(rule.Item1, context.ValidationRules, result.FieldName);
                        return;
                    }
                }
            }
        }

        private ValueTuple<string, object>[] GetRules(object validationRule, Type propertyType)
        {
            List<ValueTuple<string, object>> rules = new List<ValueTuple<string, object>>();
            Type valType = validationRule.GetType();
            bool isDictionary = validationRule is IDictionary<string, object>;
            IDictionary<string, object> _vRule = null;
            if (isDictionary)
            {
                _vRule = (IDictionary<string, object>)validationRule;
            }

            foreach (string rule in availableRules)
            {
                if (isDictionary)
                {
                    if (_vRule.ContainsKey(rule))
                    {
                        if (rule == "required")
                        {
                            rules.Add((rule, propertyType));
                        }
                        else
                        {
                            rules.Add((rule, _vRule[rule]));
                        }
                    }
                }
                else if (valType.GetProperty(rule) != null)
                {
                    if (rule == "required")
                    {
                        rules.Add((rule, propertyType));
                    }
                    else
                    {
                        rules.Add((rule, valType.GetProperty(rule).GetValue(validationRule)));
                    }
                }
            }

            return rules.ToArray();
        }

        private IValidator GetValidator(string rule)
        {
            IValidator validator;
            if (validatorOfRules.TryGetValue(rule, out validator))
            {
                return validator;
            }
            else
            {
                throw new ArgumentException($"Valitor for rule {rule} is not found");
            }
        }

        private static object[] ToObjectArray(object arg)
        {
            List<object> arr = new List<object>();
            foreach (var item in arg as IEnumerable)
            {
                arr.Add(item);
            }

            return arr.ToArray();
        }

        private string GetMessage(string rule, object validationRule, string fieldName)
        {
            object ruleValue;
            string message = null;
            Type valType = validationRule.GetType();
            PropertyInfo prop = valType.GetProperty(rule);
            bool isDictionary = validationRule is IDictionary<string, object>;
            bool hasMessages = false;
            IDictionary<string, object> _vRule = null;
            IDictionary<string, object> _messages = null;
            if (isDictionary)
            {
                _vRule = (IDictionary<string, object>)validationRule;
                object _msg = null;
                if (_vRule.TryGetValue("messages", out _msg))
                {
                    hasMessages = true;
                    _messages = (IDictionary<string, object>)_msg;
                }
            }

            if (prop != null || (isDictionary && _vRule.ContainsKey(rule)))
            {
                ruleValue = isDictionary ? _vRule[rule] : prop.GetValue(validationRule);
                if (ruleValue.GetType().IsArray)
                {
                    object[] arr = ToObjectArray(ruleValue);
                    if (arr != null)
                    {
                        message = arr[arr.Length - 1] as string ??
                            (hasMessages && _messages.ContainsKey(rule) ? _messages[rule]?.ToString() : defaultMessages[rule]);
                        message = string.Format(CultureInfo.CurrentCulture, message, arr);
                    }
                    else
                    {
                        message = hasMessages && _messages.ContainsKey(rule) ? _messages[rule]?.ToString() : defaultMessages[rule];
                        message = string.Format(CultureInfo.CurrentCulture, message, arr);
                    }
                }
                else
                {
                    message = hasMessages && _messages.ContainsKey(rule) ? _messages[rule]?.ToString() : defaultMessages[rule];
                    message = rule.Equals("email", StringComparison.Ordinal) ? string.Format(CultureInfo.CurrentCulture, message, fieldName) 
                        : string.Format(CultureInfo.CurrentCulture, message, ruleValue); // passing Field Name for Email default error message
                }
            }

            return message;
        }
    }

    /// <summary>
    /// Class that performs required field validation.
    /// </summary>
    /// <exclude/>
    public class RequiredValidator : IValidator
    {
        public bool IsValid(object value, object arguments)
        {
            // Type propertyType = arguments as Type;
            if (value == null)
            {
                return false;
            }

            string val = value as string;
            if (val != null && val.Trim().Length == 0)
            {
                return false;
            }

            // if (value != null && !propertyType.IsAssignableFrom(typeof(Nullable<>)))
            // {
            //    return false;
            // }
            return true;
        }
    }

    /// <summary>
    /// Class that performs email validation.
    /// </summary>
    /// <exclude/>
    public class EmailAddressValidator : IValidator
    {
        public bool IsValid(object value, object arguments)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;

            int atCount = 0;

            foreach (char c in valueAsString)
            {
                if (c == '@')
                {
                    atCount++;
                }
            }

            return valueAsString != null
            && atCount == 1
            && valueAsString[0] != '@'
            && valueAsString[valueAsString.Length - 1] != '@';
        }
    }

    /// <summary>
    /// Class that performs max length validation.
    /// </summary>
    /// <exclude/>
    public class MaxLengthValidator : IValidator
    {
        public bool IsValid(object value, object arguments)
        {
            int Length = Convert.ToInt32(arguments, CultureInfo.InvariantCulture);
            var length = 0;

            // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
            if (value == null)
            {
                return true;
            }
            else
            {
                var str = value as string;
                if (str != null)
                {
                    length = str.Length;
                }
                else
                {
                    // We expect a cast exception if a non-{string|array} property was passed in.
                    length = ((Array)value).Length;
                }
            }

            return length <= Length;
        }
    }

    /// <summary>
    /// Class that performs min length validation.
    /// </summary>
    /// <exclude/>
    public class MinLengthValidator : IValidator
    {
        public bool IsValid(object value, object arguments)
        {
            int Length = Convert.ToInt32(arguments, CultureInfo.InvariantCulture);
            var length = 0;

            // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
            if (value == null)
            {
                return true;
            }
            else
            {
                var str = value as string;
                if (str != null)
                {
                    length = str.Length;
                }
                else
                {
                    // We expect a cast exception if a non-{string|array} property was passed in.
                    length = ((Array)value).Length;
                }
            }

            return length >= Length;
        }
    }

    /// <summary>
    /// Class that performs range length or string length validation.
    /// </summary>
    /// <exclude/>
    public class StringLengthValidator : IValidator
    {
        public bool IsValid(object value, object arguments)
        {
            int length = value == null ? 0 : ((string)value).Length;
            double[] range = SfBaseUtils.ToDoubleArray(arguments ?? null);
            return value == null || (Convert.ToDouble(length) >= range[0] && Convert.ToDouble(length) <= range[1]);
        }
    }

    /// <summary>
    /// Class that performs range validation.
    /// </summary>
    /// <exclude/>
    public class RangeValidator : IValidator
    {
        public bool IsValid(object value, object arguments)
        {
            double[] range = SfBaseUtils.ToDoubleArray(arguments ?? null);
            return value == null || (Convert.ToDouble(value, CultureInfo.InvariantCulture) >= range[0] 
                && Convert.ToDouble(value, CultureInfo.InvariantCulture) <= range[1]);
        }
    }

    /// <summary>
    /// Class that performs regex validation.
    /// </summary>
    /// <exclude/>
    public class RegexValidator : IValidator
    {
        public bool IsValid(object value, object arguments)
        {
            if (value == null)
            {
                return true;
            }

            string pattern = arguments as string;
            string valueAsString = value.ToString();
            Regex _regex = new Regex(pattern);
            Match m = _regex.Match(valueAsString);
            return (m.Success && m.Index == 0 & m.Value.Length == valueAsString.Length);
        }
    }

    /// <summary>
    /// Class that performs number validation.
    /// </summary>
    /// <exclude/>
    public class NumberValidator : IValidator
    {
        public bool IsValid(object value, object arguments)
        {
            double result;
            if (value == null)
            {
                return true;
            }

            try
            {
                result = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Class that performs max value validation.
    /// </summary>
    /// <exclude/>
    public class MaxValidator : IValidator
    {
        public bool IsValid(object value, object arguments)
        {
            double result;
            double max = Convert.ToDouble(arguments, CultureInfo.InvariantCulture);
            if (value == null)
            {
                return true;
            }

            try
            {
                result = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                return result <= max;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Class that performs min value validation.
    /// </summary>
    /// <exclude/>
    public class MinValidator : IValidator
    {
        public bool IsValid(object value, object arguments)
        {
            double result;
            double min = Convert.ToDouble(arguments, CultureInfo.InvariantCulture);
            if (value == null)
            {
                return true;
            }

            try
            {
                result = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                return result >= min;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Provides validation rules and message customization.
    /// </summary>
    public class ValidationRuleBase
    {
        /// <summary>
        /// Validates the given value is present or not null.
        /// </summary>
        public bool? Required { get; set; }

        /// <summary>
        /// Validates that given string is in range length given.
        /// </summary>
        public double[] RangeLength { get; set; }

        /// <summary>
        /// Validates that given value is within range.
        /// </summary>
        public double[] Range { get; set; }

        /// <summary>
        /// Validates that given value length is greater than minlength value.
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// Validates that given value length is lesser than maxlength value.
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Validates that given value matches the given regex.
        /// </summary>
        public string RegexPattern { get; set; }

        /// <summary>
        /// Validates that given value is an e-mail.
        /// </summary>
        public bool? Email { get; set; }

        /// <summary>
        /// Validates that given value is a number.
        /// </summary>
        public bool? Number { get; set; }

        /// <summary>
        /// Validates that given value is greater than min value.
        /// </summary>
        public int? Min { get; set; }

        /// <summary>
        /// Validates that given value is lesser than max value.
        /// </summary>
        public int? Max { get; set; }

        /// <summary>
        /// Gets or sets the error messages for the validation rules.
        /// </summary>
        public IDictionary<string, object> Messages { get; set; } = new Dictionary<string, object>();

        internal object ToDictionary()
        {
            IDictionary<string, object> _rules = new Dictionary<string, object>();

            if (Required.HasValue)
            {
                _rules.Add("required", Required);
            }

            if (RangeLength != null)
            {
                _rules.Add("rangeLength", RangeLength);
            }

            if (Range != null)
            {
                _rules.Add("range", Range);
            }

            if (MinLength.HasValue)
            {
                _rules.Add("minLength", MinLength);
            }

            if (MaxLength.HasValue)
            {
                _rules.Add("maxLength", MaxLength);
            }

            if (Number.HasValue)
            {
                _rules.Add("number", Number);
            }

            if (Email != null)
            {
                _rules.Add("email", Email);
            }

            if (Min.HasValue)
            {
                _rules.Add("min", Min);
            }

            if (Max.HasValue)
            {
                _rules.Add("max", Max);
            }

            if (RegexPattern != null)
            {
                _rules.Add("regex", RegexPattern);
            }

            if (Messages != null && Messages.Any())
            {
                _rules.Add("messages", Messages);
            }

            return _rules;
        }

        internal static ValidationRuleBase ToInstance(IDictionary<string, object> dictionary)
        {
            ValidationRuleBase _rules = new ValidationRuleBase();

            if (dictionary.ContainsKey("required"))
            {
                _rules.Required = true;
            }

            if (dictionary.ContainsKey("rangeLength"))
            {
                _rules.RangeLength = Syncfusion.Blazor.Internal.SfBaseUtils.ToDoubleArray(dictionary["rangeLength"]);
            }

            if (dictionary.ContainsKey("range"))
            {
                _rules.Range = Syncfusion.Blazor.Internal.SfBaseUtils.ToDoubleArray(dictionary["range"]);
            }

            if (dictionary.ContainsKey("minLength"))
            {
                _rules.MinLength = (int?)dictionary["minLength"];
            }

            if (dictionary.ContainsKey("maxLength"))
            {
                _rules.MaxLength = (int?)dictionary["maxLength"];
            }

            if (dictionary.ContainsKey("number"))
            {
                _rules.Number = true;
            }

            if (dictionary.ContainsKey("email"))
            {
                _rules.Email = (bool?)dictionary["email"];
            }

            if (dictionary.ContainsKey("min"))
            {
                _rules.Min = (int?)dictionary["min"];
            }

            if (dictionary.ContainsKey("max"))
            {
                _rules.Max = (int?)dictionary["max"];
            }

            if (dictionary.ContainsKey("regex"))
            {
                _rules.RegexPattern = dictionary["regex"]?.ToString();
            }

            return _rules;
        }
    }
}
