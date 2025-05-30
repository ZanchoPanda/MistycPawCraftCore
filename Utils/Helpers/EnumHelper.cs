using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MistycPawCraftCore.Utils.Helpers
{

    public static class EnumHelper
    {
        public static List<EnumItem<T>> GetValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(val => new EnumItem<T> { Value = val })
                .ToList();
        }
    }

    public class EnumItem<T>
    {
        public T Value { get; set; }
        public string Description => GetDescription();

        private string GetDescription()
        {
            string key = $"{typeof(T).Name}_{Value}";
            var localized = Application.Current.TryFindResource(key);
            return localized?.ToString() ?? Value.ToString();
        }
    }
}
