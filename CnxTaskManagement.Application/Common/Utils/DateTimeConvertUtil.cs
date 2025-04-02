using System.Reflection;
using System;
using System.Collections.Generic;
using System.Collections;

namespace CnxTaskManagement.Application.Common.Utils
{
    public class DateTimeConvertUtil
    {
        public static void ConvertDateTimesToUtc(object obj)
        {
            if (obj == null)
                return;

            // Check if the input is a List of objects
            if (obj is IEnumerable<object> objList)
            {
                foreach (var item in objList)
                {
                    if (item != null)
                        ConvertObjectDateTimesToUtc(item);
                }
            }
            else
            {
                // Handle a single object
                ConvertObjectDateTimesToUtc(obj);
            }
        }

        private static void ConvertObjectDateTimesToUtc(object obj)
        {
            Type type = obj.GetType();

            // Iterate through all properties of the object
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Check if the property is writable
                if (!property.CanWrite)
                    continue;

                // Check if the property is of type DateTime
                if (property.PropertyType == typeof(DateTime))
                {
                    if (property.GetValue(obj) is DateTime currentValue)
                    {
                        // Convert to local time if the Kind is UTC
                        if (currentValue.Kind != DateTimeKind.Utc)
                        {
                            DateTime utcValue = currentValue.ToUniversalTime();
                            property.SetValue(obj, utcValue);
                        }
                    }
                }
                // Check if the property is of type Nullable<DateTime> (DateTime?)
                else if (property.PropertyType == typeof(DateTime?))
                {
                    DateTime? currentValue = (DateTime?)property.GetValue(obj);

                    if (currentValue.HasValue && currentValue.Value.Kind != DateTimeKind.Utc)
                    {
                        DateTime utcValue = currentValue.Value.ToUniversalTime();
                        property.SetValue(obj, utcValue);
                    }
                }
            }
        }

        public static void ConvertDateTimesToLocal(object obj)
        {
            if (obj == null)
                return;

            ConvertObjectDateTimesToLocal(obj);
        }

        private static void ConvertObjectDateTimesToLocal(object obj)
        {
            if (obj == null)
                return;

            Type type = obj.GetType();

            // Handle IEnumerable (lists, arrays, etc.)
            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
            {
                foreach (var item in (IEnumerable)obj)
                {
                    ConvertObjectDateTimesToLocal(item);
                }
            }
            else
            {
                // Handle a single object
                foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    // Check if the property is writable
                    if (!property.CanWrite)
                        continue;

                    // Handle DateTime properties
                    if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime currentValue = (DateTime)property.GetValue(obj);

                        // Convert to local time if the Kind is UTC
                        if (currentValue.Kind == DateTimeKind.Utc)
                        {
                            DateTime localValue = currentValue.ToLocalTime();
                            property.SetValue(obj, localValue);
                        }
                    }
                    // Handle Nullable<DateTime> properties
                    else if (property.PropertyType == typeof(DateTime?))
                    {
                        DateTime? currentValue = (DateTime?)property.GetValue(obj);

                        if (currentValue.HasValue && currentValue.Value.Kind == DateTimeKind.Utc)
                        {
                            DateTime localValue = currentValue.Value.ToLocalTime();
                            property.SetValue(obj, localValue);
                        }
                    }
                    // Handle nested objects or collections
                    else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    {
                        var nestedObject = property.GetValue(obj);
                        ConvertObjectDateTimesToLocal(nestedObject);
                    }
                }
            }
        }
    }
}
