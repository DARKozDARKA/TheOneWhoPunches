using System;

namespace CodeBase.Tools
{
    public static class Extensions
    {
        public static T With<T>(this T self, Action<T> set) where T : class
        {
            set.Invoke(self);
            return self;
        }

        public static T With<T>(this T self, Action<T> apply, Func<bool> when) where T : class
        {
            if (when())
                apply?.Invoke(self);

            return self;
        }

        public static T With<T>(this T self, Action<T> apply, bool when) where T : class
        {
            if (when)
                apply?.Invoke(self);

            return self;
        }
    }
}