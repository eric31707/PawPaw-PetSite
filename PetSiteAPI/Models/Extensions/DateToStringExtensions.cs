namespace PetSiteAPI.Models.Extensions
{

    public static class DateToStringExtensions
    {
        /// <summary>
        /// 傳入DateTime日期，回傳"2023-3-9(四)"格式的字串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime date)
        {
            string datestring = date.ToString("yyyy-MM-dd(ddd)");
            datestring = datestring.Replace("週", "");
            return datestring;
        }
        public static string ToDateTimeStringAll(this DateTime date)
        {
            string datestring = date.ToString("yyyy-MM-dd(ddd)HH:mm");
            datestring = datestring.Replace("週", "");
            return datestring;
        }
    }
}
