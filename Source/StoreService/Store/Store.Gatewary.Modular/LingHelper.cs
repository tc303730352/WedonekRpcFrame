namespace Store.Gatewary.Modular
{
    internal static class LingHelper
    {
        public static string FormatVerNum (this long verNum)
        {
            string ver = verNum.ToString();
            if (ver.Length <= 3)
            {
                return "0.0." + ver;
            }
            else if (ver.Length <= 6)
            {
                return "0." + ver.Insert(3, ".");
            }
            else
            {
                return ver.Insert(3, ".").Insert(7, ".");
            }
        }

    }
}
