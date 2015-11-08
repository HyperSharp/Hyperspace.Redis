using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Hyperspace
{
    [DebuggerStepThrough]
    internal static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName] [NotNull] string parameterName, [NotNull] string propertyName)
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, "parameterName");
                NotEmpty(propertyName, "propertyName");
                throw new ArgumentException($"The property '{propertyName}' of the argument '{parameterName}' cannot be null.");
            }
            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static IReadOnlyList<T> NotEmpty<T>(IReadOnlyList<T> value, [InvokerParameterName] [NotNull] string parameterName)
        {
            NotNull(value, parameterName);
            if (value.Count == 0)
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentException($"The collection argument '{parameterName}' must contain at least one element.");
            }
            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotEmpty(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            Exception e = null;
            if (ReferenceEquals(value, null))
            {
                e = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException($"The string argument '{parameterName}' cannot be empty.");
            }
            if (e != null)
            {
                NotEmpty(parameterName, "parameterName");
                throw e;
            }
            return value;
        }

        public static string NullButNotEmpty(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (!ReferenceEquals(value, null) && value.Length == 0)
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentException($"The string argument '{parameterName}' cannot be empty.");
            }
            return value;
        }

        public static IReadOnlyList<T> HasNoNulls<T>(IReadOnlyList<T> value, [InvokerParameterName] [NotNull] string parameterName)
            where T : class
        {
            NotNull(value, parameterName);
            if (value.Any(e => e == null))
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentException(parameterName);
            }
            return value;
        }

        public static T IsDefined<T>(T value, [InvokerParameterName] [NotNull] string parameterName)
            where T : struct
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentException($"The value provided for argument '{parameterName}' must be a valid value of enum type '{typeof(T)}'.");
            }

            return value;
        }

    }
}