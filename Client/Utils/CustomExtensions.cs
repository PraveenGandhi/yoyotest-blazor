using System.Collections.Generic;
using System.Linq;

namespace YoYoTest.Client.Utils
{
    public static class CustomExtensions
    {
        public static IEnumerable<(T item, int index)> ToTuple<T>(this IEnumerable<T> self)
        {
            return self.Select((item, index) => (item, index));
        }

        public static string ToTime(this int self)
        {
            return PadZero(self / 60) + ':' + PadZero(self % 60);
        }

        public static int ToSeconds(this string self)
        {
            var timeArray = self.Split(':');
            return int.Parse(timeArray[0]) * 60 + int.Parse(timeArray[1]);
        }

        private static string PadZero(object self, int max = 2)
        {
            var num = self.ToString();
            return num.Length < max ? PadZero("0" + num, max) : num;
        }
    }
}