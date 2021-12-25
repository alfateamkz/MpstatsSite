using MpstatsAPIWrapper.Models.API;
using MpstatsSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramMpBot.Database;

namespace MpstatsSite.Services
{
    public class MpstatsService
    {

        public List<RubricatorItemModel> Rubricators = new List<RubricatorItemModel>();
        public List<string> OwnBusinesses = new List<string>
        {
            "Бушуев Евгений Евгеньевич",
            "Халемин Антон Владиславович"
        };
        public List<string> OwnBrands = new List<string>
        {
            "Hausland",
        };
        public List<string> OwnCategories = new List<string>
        {
            "Ершик для унитаза",
            "Органайзеры/Органайзер для хранения",
            "Красота/Аксессуары/Органайзеры и флаконы",
            //"Здоровье/Уход за полостью рта/Зубная щетка",
            "Здоровье/Уход за полостью рта",
            "Дом/Ванная/Коврики",
            "Дом/Хозяйственные товары/Товары для уборки/Салфетки для уборки",
            "Дом/Гостиная/Коробки для хранения/Коробка для хранения",
            "Дом/Хранение вещей/Шкатулки", //Шкатулка для украшений
        };

        public MarketVolume ParseVolumeInCategory(List<CategorySellerModel> sellers)
        {
            double ownRevenue = 0;
            double totalRevenue = 0;

            foreach (var ll in sellers)
            {              
                totalRevenue += (double)ll.Revenue;
                foreach (var business in OwnBusinesses)
                {
                    if (ll.Name.Contains(business)|| ll.Name.Contains(business.ToLower())
                        || ll.Name.Contains(business.ToUpper()))
                    {
                        ownRevenue += (double)ll.Revenue;
                    }
                }
            }
            double percent = ownRevenue / (totalRevenue / 100);
            if (double.IsNaN(ownRevenue)) ownRevenue = 0;
            if (double.IsNaN(percent)) percent = 0;
            if (double.IsNaN(totalRevenue)) totalRevenue = 0;

            return new MarketVolume
            {
                Date = DateTime.Now,
                OurPercent = percent,
                OurVolume = ownRevenue,
                TotalVolume = totalRevenue
            };

        }

        public void CheckForViolators(List<BrandSeller> goods)
        {
            foreach (var g in goods)
            {


                bool ourProduct = false;
                foreach (var business in OwnBusinesses)
                {
                    if (g.Seller.Contains(business) || g.Seller.Contains(business.ToLower())
                                                    || g.Seller.Contains(business.ToUpper()))
                    {
                        ourProduct = true; break;
                    }
                }
                if (!ourProduct)
                {
                    using(DatabaseConnection db = new DatabaseConnection())
                    {
                        int count = db.Violators.Where(o =>
                            o.Name == g.Seller && o.Product == g.Name && o.Brand == g.Brand && !o.IsWatched).Count();
                        if (count == 0)
                        {
                            db.Violators.Add(new Violator
                            {
                                Brand = g.Brand,
                                Date = DateTime.Now,
                                IsWatched = false,
                                Name = g.Seller,
                                Product = g.Name
                            });
                            db.SaveChanges();
                        }                  
                    }
                }
            }
           
        }
    }
}