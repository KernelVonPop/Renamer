using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renamer.Classes
{
    public static class Utilities
    {
        public static bool EstNombreAVirgule(string valeur)
        {
            decimal parseResultat;
            return Decimal.TryParse(valeur.Replace('.', ','), System.Globalization.NumberStyles.Currency, System.Globalization.NumberFormatInfo.CurrentInfo, out parseResultat) ||
                   Decimal.TryParse(valeur.Replace('.', ','), System.Globalization.NumberStyles.Currency, System.Globalization.NumberFormatInfo.InvariantInfo, out parseResultat);
        }

        public static bool EstPourcentage(string valeur)
        {
            decimal parseResultat;
            System.Globalization.NumberFormatInfo formatInfo = (System.Globalization.NumberFormatInfo)System.Globalization.NumberFormatInfo.InvariantInfo.Clone();
            formatInfo.CurrencySymbol = "%";
            System.Globalization.NumberFormatInfo formatInfo2 = (System.Globalization.NumberFormatInfo)System.Globalization.NumberFormatInfo.CurrentInfo.Clone();
            formatInfo2.CurrencySymbol = "%";
            return Decimal.TryParse(valeur, System.Globalization.NumberStyles.Currency, formatInfo, out parseResultat) ||
                   Decimal.TryParse(valeur, System.Globalization.NumberStyles.Currency, formatInfo2, out parseResultat);
        }

        public static double StringToDouble(string valeur)
        {
            double parseResultat;
            valeur = valeur.Replace('%', ' ').Replace('$', ' ').Trim();
            if (EstNombreAVirgule(valeur))
            {
                if (double.TryParse(valeur, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo, out parseResultat))
                    return double.Parse(valeur, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo);
                else if (double.TryParse(valeur, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out parseResultat))
                    return double.Parse(valeur, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            return -1;
        }

        
        public static decimal StringToDecimal(string valeur)
        {
            decimal parseResultat;
            valeur = valeur.Replace('%', ' ').Replace('$', ' ').Trim();
            if (EstNombreAVirgule(valeur))
            {
                if (decimal.TryParse(valeur, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo, out parseResultat))
                    return decimal.Parse(valeur, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo);
                else if (decimal.TryParse(valeur, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out parseResultat))
                    return decimal.Parse(valeur, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            return -1;
        }

        public static double Trunc(double valeur)
        {
            return Trunc(valeur, 0);
        }

        public static double Trunc(double valeur, int nbrDecimals)
        {

            if (nbrDecimals <= 0)
                return Math.Truncate(valeur);
            int decimals = 10;
            if (decimals > 1)
                decimals = (int)Math.Pow(decimals, nbrDecimals);
            double resultat;
            try
            {
                resultat = Math.Truncate(valeur * decimals) / decimals;
            }
            catch (DivideByZeroException)
            {
                resultat = valeur;
            }
            return resultat;

        }

        public static decimal Trunc(decimal valeur)
        {
            return Trunc(valeur, 0);
        }

        public static decimal Trunc(decimal valeur, int nbrDecimals)
        {
            if (nbrDecimals <= 0)
                return Math.Truncate(valeur);
            int decimals = 10;
            if (decimals > 1)
                decimals = (int)Math.Pow(decimals, nbrDecimals);
            decimal resultat;
            try
            {
                resultat = Math.Truncate(valeur * decimals) / decimals;
            }
            catch (DivideByZeroException)
            {
                resultat = valeur;
            }
            return resultat;
        }

        
        public static decimal CalculatePercentage(decimal dividende, decimal diviseur)
        {
            try
            {
                if (diviseur == 0)
                    throw new Exception();
                decimal quotient = (dividende / diviseur);
                return Trunc(quotient, 4);
            }
            catch
            {
                return 0m;
            }
        }

        public static decimal CalculateDiv(decimal pourcentage, decimal dividende)
        {
            try
            {
                decimal diviseur = (pourcentage * dividende) / 100;
                diviseur = Utilities.Trunc(diviseur, 2);
                return diviseur;
            }
            catch
            {
                return 0m;
            }
        }
    }
}
