using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MpstatsParser.Services;
using MpstatsSite.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using MpstatsSite.Helpers;
using MpstatsSite.Models;

namespace MpstatsSite.Jobs
{
    public class MpstatsJob : IHostedService, IDisposable
    {
        private Timer _timer = null;
        private MpstatsService mpstats;

        public MpstatsJob()
        {
            InitJob();
        }

        private async void InitJob()
        {
            MpstatsAPI.APIKey = "61b31b63525083.2834277861dacf32f4f8c6d74bbda6e8f2419fb0";
            mpstats = new MpstatsService();
            mpstats.Rubricators = await MpstatsAPI.GetRubricator();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(240));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            MpstatsParsedHelper.CategoryInfos.Clear();
            
            for (int i = mpstats.Rubricators.Count - 1; i > -1; i--)
            {
                try
                {
                    bool contains = false;
                    foreach (var own in mpstats.OwnCategories)
                    {
                        if (mpstats.Rubricators[i].Path.Contains(own) ||
                            mpstats.Rubricators[i].Path.Contains(own.ToLower())
                            || mpstats.Rubricators[i].Path.Contains(own.ToUpper()))
                        {
                            contains = true;
                            break;
                        }
                    }

                    if (!contains)
                        mpstats.Rubricators.RemoveAt(i);
                }
                catch (Exception ex)
                {
                    LogToTxt(ex.StackTrace);
                    LogToTxt(ex.Message);
                }
            }



            foreach (var cat in mpstats.Rubricators)
            {
                List<MarketVolume> volumes = new List<MarketVolume>();
                Console.WriteLine(cat.Path);

                double ourVol = 0;
                double totalVol = 0;
                try
                {
                    for (int i = 1; i < 31; i++)
                    {
                        var l = MpstatsAPI.GetCategorySellers(cat.Path, DateTime.Now.AddDays(i*-1), DateTime.Now.AddDays(i*-1+1));
                        var parsed = mpstats.ParseVolumeInCategory(l);
                        ourVol += parsed.OurVolume;
                        totalVol += parsed.TotalVolume;
                        volumes.Add(parsed);
                        Debug.WriteLine(DateTime.Now.AddDays(i * -1) +"  "+ DateTime.Now.AddDays(i * -1 + 1));
                    }
                }
                catch (Exception ex)
                {
                    LogToTxt(ex.StackTrace);
                    LogToTxt(ex.Message);
                }


                try
                {
                    MpstatsParsedHelper.CategoryInfos.Add(new MarketCategoryInfo()
                    {
                        Category = cat.Path,
                        VolumeForLastMonth = volumes,
                        OurPercent = ourVol / (totalVol / 100)
                    });
                }
                catch (Exception ex)
                {
                    LogToTxt(ex.StackTrace);
                    LogToTxt(ex.Message);
                }

                try
                {
                    foreach (var brand in mpstats.OwnBrands)
                    {
                        var poss = MpstatsAPI.GetBrandPositions(brand, DateTime.Now.AddDays(-30), DateTime.Now);
                        mpstats.CheckForViolators(poss);
                    }
                }
                catch (Exception ex)
                {
                    LogToTxt(ex.StackTrace);
                    LogToTxt(ex.Message);
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void LogToTxt(string log)
        {
            var pathToFile = HostingEnvironment.ApplicationPhysicalPath;
            File.AppendAllText(Path.Combine(pathToFile, "log.txt"), log+Environment.NewLine+Environment.NewLine+Environment.NewLine);
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}