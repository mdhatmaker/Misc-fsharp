/*
 * Created by Ryan Hill, Copyright July 2013
 * 
 *  This file is part of QuandlDotNet package, to demonstrate basic access using QuandlDotNet classes.
 * 
 *  QuandlDotNet is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  QuandlDotNet is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with QuandlDotNet.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using QuandlDotNet;

namespace QuandlDemo
{
    class QuandlDemo
    {
        static void Main(string[] args)
        {
            Quandl myQuandl = new Quandl();
            
            // Add the required settings to pull down data:
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings.Add("collapse", "weekly");
            settings.Add("trim_start", "2010-02-01");
            settings.Add("trim_end", "2010-04-28");
            settings.Add("transformation", "normalize");
            settings.Add("sort_order", "asc");

            // Fetch:
            IList<CsvFinancialFormat> data = myQuandl.GetData<CsvFinancialFormat>("YAHOO/MX_IBM", settings); //"GOOG/NYSE_IBM"

            // Debug Purposes Only
            foreach (CsvFinancialFormat tick in data)
            {
                //Console.WriteLine(tick.Time.ToShortDateString() + " H: " + tick.High);
                Console.WriteLine(tick.InputString);
            }
            //Pause
            Console.ReadKey();
        }
    }


    /// <summary>
    /// Example user defined class data format for CSV quandl request
    /// </summary>
    public class CsvFinancialFormat
    {
        public DateTime Time = new DateTime();
        public Decimal Open = 0;
        public Decimal High = 0;
        public Decimal Low = 0;
        public Decimal Close = 0;
        public Decimal Volume = 0;
        public string InputString = "";


        /// <summary>
        /// Create our new generic data type:
        /// </summary>
        /// <param name="csvLine"></param>
        public CsvFinancialFormat(string csvLine)
        {
            InputString = csvLine;
            try
            {
                string[] values = csvLine.Split(',');
                if (values.Length >= 6)
                {
                    Time = Convert.ToDateTime(values[0]);
                    try
                    {
                        Volume = Convert.ToDecimal(values[5]);
                        // Catch formatting issues with regards to days in which no trades occur
                        // Doesn't work for YAHOO, only GOOG
                        if (Volume > 0)
                        {
                            Open = Convert.ToDecimal(values[1]);
                            High = Convert.ToDecimal(values[2]);
                            Low = Convert.ToDecimal(values[3]);
                            Close = Convert.ToDecimal(values[4]);
                        }
                        else
                        {
                            // No trades occured, make all open/high/low == close
                            Open = Convert.ToDecimal(values[4]);
                            High = Convert.ToDecimal(values[4]);
                            Low = Convert.ToDecimal(values[4]);
                            Close = Convert.ToDecimal(values[4]);
                        }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("Missing Data:" + csvLine);
                    }
                }
            }
            catch (Exception err)
            {
                //Write the titles out:
                Console.WriteLine("Er:" + csvLine);
            }
        }
    }
}
