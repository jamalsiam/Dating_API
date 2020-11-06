using System;

namespace Api.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            var toDay = DateTime.Today;
            var age = toDay.Year - dob.Year;
            if (dob.Date > toDay.AddYears(-age))
                age--;

            return age;
        }
    }
}