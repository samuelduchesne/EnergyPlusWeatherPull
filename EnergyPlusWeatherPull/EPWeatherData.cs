﻿using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;
using System.Reflection;

namespace EnergyPlusWeatherPull
{
    public class EPWeatherData
    {
        public Location Location { get; set; }
        public List<HourlyWeatherData> HourlyWeatherData { get; set; }
        public WeatherFileStats WeatherFileStats { get; set; }

        public EPWeatherData()
        {
            HourlyWeatherData = new List<HourlyWeatherData>();
            
        }

        public void GetRawData(string filepath)
        {
            try
            {
                using (TextReader tr = File.OpenText(filepath))
                {
                    var parser = new CsvParser(tr);
                    var row = parser.Read();
                    Location location = new Location();
                    location.CityName = row[1];
                    location.StateName = row[2];
                    location.Country = row[3];
                    location.WeatherSource = row[4];
                    location.WMOID = row[5];
                    location.Latitude = Convert.ToDecimal(row[6]);
                    location.Longitude = Convert.ToDecimal(row[7]);
                    location.TimeZoneOffset = Convert.ToDecimal(row[8]);
                    location.Elevation = Convert.ToDecimal(row[9]);
                    this.Location = location;
                    Console.WriteLine("Grabbed location: {0}", location.CityName + "," + location.StateName + "," + location.Country);
                    
                    
                }
                using(TextReader tr2 = File.OpenText(filepath))
                {
                    int linect = 0;
                    var csv = new CsvReader(tr2);
                    while (csv.Read())
                    {
                        //get the ground temperatures
                        if (linect == 2)
                        {

                        }
                        //get the hourly data
                        if (linect > 6)
                        {
                            HourlyWeatherData epw = new HourlyWeatherData();
                            epw.Year = csv.GetField<int>(0);
                            epw.Month = csv.GetField<int>(1);
                            epw.Day = csv.GetField<int>(2);
                            epw.Hour = csv.GetField<int>(3);
                            epw.Minute = csv.GetField<int>(4);
                            epw.UncertaintyFlag = csv.GetField<string>(5);
                            epw.DB = csv.GetField<decimal>(6);
                            epw.WB = csv.GetField<decimal>(7);
                            epw.RH = csv.GetField<decimal>(8);
                            epw.Pressure = csv.GetField<decimal>(9);
                            epw.HorRadiation = csv.GetField<decimal>(10);
                            epw.NormalRadiation = csv.GetField<decimal>(11);
                            epw.SkyRadiation = csv.GetField<decimal>(12);
                            epw.GHorRadiation = csv.GetField<decimal>(13);
                            epw.DirectNormalRadiation = csv.GetField<decimal>(14);
                            epw.DiffuseHorizontalRadiation = csv.GetField<decimal>(15);
                            epw.GHorIllumination = csv.GetField<decimal>(16);
                            epw.DirectNormalIllumination = csv.GetField<decimal>(17);
                            epw.DiffuseHorizontalIllumination = csv.GetField<decimal>(18);
                            epw.ZenithIllumination = csv.GetField<decimal>(19);
                            epw.WindDirection = csv.GetField<decimal>(20);
                            epw.WindSpeed = csv.GetField<decimal>(21);
                            epw.TotalSkyCover = csv.GetField<decimal>(22);
                            epw.OpaqSkyCover = csv.GetField<decimal>(23);
                            epw.Visibility = csv.GetField<decimal>(24);
                            epw.FieldCeilHeight = csv.GetField<decimal>(25);
                            epw.WeatherObserv = csv.GetField<decimal>(26);
                            epw.WeatherCodes = csv.GetField<decimal>(27);
                            epw.PrecipitationWater = csv.GetField<decimal>(28);
                            epw.AerosolOptical = csv.GetField<decimal>(29);
                            epw.SnowDepth = csv.GetField<decimal>(30);
                            epw.DaysSinceSnow = csv.GetField<decimal>(31);
                            this.HourlyWeatherData.Add(epw);
                            Console.WriteLine("Success grabbing data for {0}", epw.Year + "-" + epw.Month + "-" + epw.Day + " " + "Hour " + epw.Hour);
                        }
                        linect++;
                    }
                }
                Console.WriteLine("Done grabbing E+ weather data file.");
                //test to make sure everything went smoothly
            }
            catch(Exception e)
            {
                throw;
            }
            
        }
        public void GetWeatherStats(params string[] interests)
        {
            //always refresh the weather stats
            this.WeatherFileStats = new WeatherFileStats();
            if (interests.Count() == 0) { 
                interests = new string[2];
                interests[0] = "DB";
                interests[1] = "WB";
            }
            
            for (int m = 1; m <= 12; m++)
            {
                switch (m)
                {
                    case 1:
                        //month of data
                        var Jans = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Jans,"January",m,31,interests);
                        Console.WriteLine("Completed January Weather File Statistical Summary.");
                        break;
                    case 2:
                        var Febs = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Febs,"February",m,28,interests);
                        Console.WriteLine("Completed February Weather File Statistical Summary.");
                        break;
                    case 3:
                        var Mars = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Mars,"March",m,31,interests);
                        Console.WriteLine("Completed March Weather File Statistical Summary.");
                        break;
                    case 4:
                        var Aprs = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Aprs,"April",m,30,interests);
                        Console.WriteLine("Completed April Weather File Statistical Summary.");
                        break;
                    case 5:
                        var Mays = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Mays,"May",m,31,interests);
                        Console.WriteLine("Completed May Weather File Statistical Summary.");
                        break;
                    case 6:
                        var Juns = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Juns,"June",m,30,interests);
                        Console.WriteLine("Completed June Weather File Statistical Summary.");
                        break;
                    case 7:
                        var Juls = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Juls,"July",m,31,interests);
                        Console.WriteLine("Completed July Weather File Statistical Summary.");
                        break;
                    case 8:
                        var Augs = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Augs,"August",m,31,interests);
                        Console.WriteLine("Completed August Weather File Statistical Summary.");
                        break;
                    case 9:
                        var Septs = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Septs,"September",m,30,interests);
                        Console.WriteLine("Completed September Weather File Statistical Summary.");
                        break;
                    case 10:
                        var Octs = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Octs,"October",m,31,interests);
                        Console.WriteLine("Completed October Weather File Statistical Summary.");
                        break;
                    case 11:
                        var Novs = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Novs,"November",m,30,interests);
                        Console.WriteLine("Completed November Weather File Statistical Summary.");
                        break;
                    case 12:
                        var Decs = this.HourlyWeatherData.FindAll(x => x.Month == m);
                        this.WeatherFileStats.SetMonthlyWeatherFileStats(Decs,"December",m,31,interests);
                        Console.WriteLine("Completed December Weather File Statistical Summary.");
                        break;
                }
            }
            Console.WriteLine("Done with Weather File Statistical Summaries for All Months.");
        }
    }

    public class HourlyWeatherData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        //more location based time awareness?
        public string UncertaintyFlag { get; set; }
        public decimal DB { get; set; }
        public decimal WB { get; set; }
        public decimal RH { get; set; }
        public decimal Pressure { get; set; }
        public decimal HorRadiation { get; set; }
        public decimal NormalRadiation { get; set; }
        public decimal SkyRadiation { get; set; }
        public decimal GHorRadiation { get; set; }
        public decimal DirectNormalRadiation { get; set; }
        public decimal DiffuseHorizontalRadiation { get; set; }
        public decimal GHorIllumination { get; set; }
        public decimal DirectNormalIllumination { get; set; }
        public decimal DiffuseHorizontalIllumination { get; set; }
        public decimal ZenithIllumination { get; set; }
        public decimal WindDirection { get; set; }
        public decimal WindSpeed { get; set; }
        public decimal TotalSkyCover { get; set; }
        public decimal OpaqSkyCover { get; set; }
        public decimal Visibility { get; set; }
        public decimal FieldCeilHeight { get; set; }
        public decimal WeatherObserv { get; set; }
        public decimal WeatherCodes { get; set; }
        public decimal PrecipitationWater { get; set; }
        public decimal AerosolOptical { get; set; }
        public decimal SnowDepth { get; set; }
        public decimal DaysSinceSnow { get; set; }
    }

    public class Location
    {
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string Country { get; set; }
        public string WeatherSource { get; set; }
        public string WMOID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal TimeZoneOffset { get; set; }
        public decimal Elevation { get; set; }
    }

    public class MonthlySummaries
    {

    }

    public class WeatherFileStats
    {
        public List<Monthly> Monthly { get; set; }

        public WeatherFileStats()
        {
            this.Monthly = new List<Monthly>();
        }

        public void SetMonthlyWeatherFileStats(List<HourlyWeatherData> avails, string monthName, int monthNumber, int numDays, string[] interests)
        {
            Monthly m = new Monthly();
            m.SimpleName = monthName;
            m.Id = monthNumber;
            m.ExpectedValCount = 24 * numDays;
            m.ActualValCount = avails.Count();
            //loop through desired properties of interest and gather monthly statistics
            for (int s = 0; s < interests.Count(); s++)
            {
                string interest = interests[s];
                List<double> values = new List<double>();
                foreach (HourlyWeatherData wd in avails)
                {
                    values.Add(Convert.ToDouble(wd.GetType().GetProperty(interest).GetValue(wd, null))); //reflection
                }
                PropertyInfo p = m.GetType().GetProperty(interest);
                var stats = new DescriptiveStatistics(values);
                p.SetValue(m, stats, null);
                if (s == interests.Count() - 1) { this.Monthly.Add(m); }
                
            }
        }
    }

    public class Monthly
    {
        public string SimpleName { get; set; }
        public int Id { get; set; }
        public int ExpectedValCount { get; set; }
        public int ActualValCount { get; set; }
        public DescriptiveStatistics DB { get; set; }
        public DescriptiveStatistics WB { get; set; }
        public DescriptiveStatistics RH { get; set; }
        public DescriptiveStatistics Pressure { get; set; }
        public DescriptiveStatistics HorRadiation { get; set; }
        public DescriptiveStatistics NormalRadiation { get; set; }
        public DescriptiveStatistics SkyRadiation { get; set; }
        public DescriptiveStatistics GHorRadiation { get; set; }
        public DescriptiveStatistics DirectNormalRadiation { get; set; }
        public DescriptiveStatistics DiffuseHorizontalRadiation { get; set; }
        public DescriptiveStatistics GHorIllumination { get; set; }
        public DescriptiveStatistics DirectNormalIllumination { get; set; }
        public DescriptiveStatistics DiffuseHorizontalIllumination { get; set; }
        public DescriptiveStatistics ZenithIllumination { get; set; }
        public DescriptiveStatistics WindDirection { get; set; }
        public DescriptiveStatistics WindSpeed { get; set; }
        public DescriptiveStatistics TotalSkyCover { get; set; }
        public DescriptiveStatistics OpaqSkyCover { get; set; }
        public DescriptiveStatistics Visibility { get; set; }
        public DescriptiveStatistics FieldCeilHeight { get; set; }
        public DescriptiveStatistics WeatherObserv { get; set; }
        public DescriptiveStatistics WeatherCodes { get; set; }
        public DescriptiveStatistics PrecipitationWater { get; set; }
        public DescriptiveStatistics AerosolOptical { get; set; }
        public DescriptiveStatistics SnowDepth { get; set; }
        public DescriptiveStatistics DaysSinceSnow { get; set; }
    }
    
}
