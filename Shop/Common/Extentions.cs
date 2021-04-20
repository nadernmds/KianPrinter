using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace System
{
    public static class Extentions
    {
        public static IEnumerable<T> ToMergedList<T>(this IEnumerable<IEnumerable<T>> items)
        {
            List<T> res = new List<T>();
            foreach (var item in items)
            {
                res.AddRange(item);
            }
            return res;
        }
        public static string ToNormalString(this string text)
        {
            return System.Text.RegularExpressions.Regex.Replace(text, @"<(.|\n)*?>", " ");
        }
        public static IEnumerable<T> Random<T>(this IEnumerable<T> list, int count)
        {


            return list.OrderBy(c => Guid.NewGuid()).Take(count);
        }
        public static decimal CalculateOff(this Shop.Models.Product product)
        {
            var s = product.Offers.FirstOrDefault(c => c.startDate < DateTime.Now && c.endDate > DateTime.Now);
            if (s != null)
            {
                return s.Product.price - ((s.offPercent ?? 0) * (s.Product.price) / 100) - (s.price ?? 0);
            }
            else
            {
                return product.price;
            }
        }
        public static bool HasOffer(this Shop.Models.Product product)
        {
            var s = product.Offers.FirstOrDefault(c => c.Product.existingCount > 0 && c.startDate < DateTime.Now && c.endDate > DateTime.Now);
            return s != null;

        }
        public static Shop.Models.Offer GetOffer(this Shop.Models.Product product)
        {
            return product.Offers.FirstOrDefault(c => c.Product.existingCount > 0 && c.startDate < DateTime.Now && c.endDate > DateTime.Now);


        }
        public static string ToJsArray(this List<int> phrase)
        {
            string i = "[";
            foreach (var item in phrase)
            {
                i += item + ",";
            }
            i += "]";

            return i;
        }

        public static string toPersianDate(this DateTime? date)
        {
            if (!date.HasValue)
                return "";
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(date.Value)}/{pc.GetMonth(date.Value)}/{pc.GetDayOfMonth(date.Value)} {pc.GetHour(date.Value)}:{pc.GetMinute(date.Value)}";
        }
    }
}