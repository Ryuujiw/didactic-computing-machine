namespace Crustz.Ecommerce.Helpers;

public static class ZoneHelper
{
    public static string ToZone(string flatRateFromZapiet)
    {
        if (flatRateFromZapiet == "Flat Rate DBZ#101850")
        {
            return "Zone A";
        }
        if (flatRateFromZapiet == "Flat Rate DBZ#101851")
        {
            return "Zone B";
        }
        if (flatRateFromZapiet == "Flat Rate DBZ#101852")
        {
            return "Zone C";
        }
        if (flatRateFromZapiet == "Flat Rate DBZ#101853")
        {
            return "Zone D";
        }
        if (flatRateFromZapiet == "Flat Rate DBZ#101854")
        {
            return "Zone E";
        }
        
        if (flatRateFromZapiet == "Flat Rate DBZ#101855")
        {
            return "Zone F";
        }
        
        return flatRateFromZapiet;
    }
}