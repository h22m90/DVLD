using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class Country
    {
        public string CountryName { get; set; }
        public int CountryID { get; set; }
        public Country()
        {

        }
        public Country(int CountryID,string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }
        public static DataTable GetAllCountries()
        {
            return CountryData.GetAllCountries();
        }
        public static Country Find(int CountryID)
        {
            string CountryName = "";
            if (CountryData.GetCountryNameByID(CountryID, ref CountryName))
                return new Country(CountryID, CountryName);
            else
                return null;
        }
        public static Country Find(string CountryName)
        {
            int CountryID = -1;
            if (CountryData.GetCountryByName(CountryName, ref CountryID))
                return new Country(CountryID, CountryName);
            else
                return null;
        }
    }
}
