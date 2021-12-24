using MpstatsAPIWrapper.Exceptions;
using MpstatsAPIWrapper.Models.API;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace MpstatsParser.Services
{
    public static class MpstatsAPI
    {
        static RestRequest request;
        static IRestResponse response;
        static JsonSerializer serializer = new JsonSerializer();
        public static string APIKey { get; set; }

        public async static Task<List<RubricatorItemModel>> GetRubricator()
        {
            dynamic json = new
            {
                startRow = 1,
                endRow = 5000,
                sortModel = new dynamic[0],
                filterModel = new dynamic[0],
            };

            if (!string.IsNullOrEmpty(APIKey))
            {
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/categories");
                request = new RestRequest(Method.GET);
                SetHeaders(request);
                client.UserAgent =  "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 OPR/78.0.4093.184 (Edition utorrent)";

                //request.AddJsonBody(json);
                //request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                //  request.AddParameter("application/json; charser=utf-8", JsonConvert.SerializeObject(new { startRow = 1, endRow = 2}), ParameterType.RequestBody);
                response = client.Execute(request);
                
                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
         
                    while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
             
                dynamic resp = JsonConvert.DeserializeObject(response.Content);
                List<RubricatorItemModel> items = new List<RubricatorItemModel>();
                foreach (var i in resp)
                {
                    RubricatorItemModel item = new RubricatorItemModel
                    {
                        Name = i.name,
                        Path = i.path,
                        Url = i.url,
                    };
                    items.Add(item);
                }
         
                return items;
            }
            else
            {
                throw new NoAPIKeyException();
            }
        }

        public static List<SubcategoryModel> GetSubcategoryInfo(string path, DateTime d1, DateTime d2)
        {
            dynamic json = new
            {
                startRow = 1,
                endRow = 5000,
                sortModel = new dynamic[0],
                filterModel = new dynamic[0],
            };

            if (!string.IsNullOrEmpty(APIKey))
            {
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/category/subcategories" +
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d2={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={path}");
                request = new RestRequest(Method.GET);
                SetHeaders(request);
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 OPR/78.0.4093.184 (Edition utorrent)";


                //request.AddJsonBody(json);
                //request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                // request.AddHeader("content-length", "196");
                response = client.Execute(request);

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
                while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
                List<SubcategoryModel> items = new List<SubcategoryModel>();
                try
                {
                    dynamic resp = JsonConvert.DeserializeObject(response.Content);
                     
                    if (resp != null)
                    {
                        bool isFirstElem = true;
                        foreach (var i in resp)
                        {
                            if (isFirstElem) { isFirstElem = false; continue; }
                            SubcategoryModel item = new SubcategoryModel
                            {
                                Name = i.name,
                                ItemsNumber = (int?)i.items,
                                CommentsAverage = (double?)i.comments,
                                Rating = (double?)i.rating,
                                Revenue = (double?)i.revenue,
                                SalesNumber = (int?)i.sales
                            };
                            items.Add(item);
                        }
                    }
                   
                }
                catch
                {

                }
                
                // System.Windows.MessageBox.Show(response.StatusCode.ToString());
                return items;
            }
            else
            {
                throw new NoAPIKeyException();
            }
        }

        public static List<CategorySellerModel> GetCategorySellers(string path, DateTime d1, DateTime d2)
        {
            dynamic json = new
            {
                startRow = 1,
                endRow = 5000,
                sortModel = new dynamic[0],
                filterModel = new dynamic[0],
            };

            if (!string.IsNullOrEmpty(APIKey))
            {
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/category/sellers" +
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d2={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={path}");
    
                request = new RestRequest(Method.GET);
                SetHeaders(request);
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 OPR/78.0.4093.184 (Edition utorrent)";


                //request.AddJsonBody(json);
                //request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                response = client.Execute(request);

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
                while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
                dynamic resp = JsonConvert.DeserializeObject(response.Content);
                List<CategorySellerModel> items = new List<CategorySellerModel>();
                if (resp != null)
                {
                   
                    foreach (var i in resp)
                    {
                        CategorySellerModel item = new CategorySellerModel
                        {
                            Name = i.name,
                            CommentsAverage = i.comments,
                            ItemsNumber = i.items,
                            SalesNumber = i.sales,
                            Position = i.position,
                            Rating = i.rating,
                            Revenue = i.revenue
                        };
                        items.Add(item);
                    }
                }
                
                //System.Windows.MessageBox.Show(items.Sum(f => f.Revenue).ToString());
                return items;
            }
            else
            {
                throw new NoAPIKeyException();
            }
        }

        public static List<CategoryProductModel> GetCategoryProducts(string path, DateTime d1, DateTime d2,double fromSKU=0)
        {

            if (!string.IsNullOrEmpty(APIKey))
            {
                dynamic sales = new
                {
                    filterType = "number",
                    type = "greaterThan",
                    filter = fromSKU
                };
                
                
               
                dynamic json = new
                {
                    startRow = 1,
                    endRow = 5000,
                    sortModel = new dynamic[0],
                    filterModel = new
                    {
                        sales = new
                        { 
                            filterType = "number",
                            type = "greaterThan",
                            filter = fromSKU,
                            filterTo = int.MaxValue
                        } 
                }            
                };

              

                RestClient client = new RestClient("https://mpstats.io/api/wb/get/category" +
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d2={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={path}");
                request = new RestRequest(Method.POST);
                SetHeaders(request);
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 OPR/78.0.4093.184 (Edition utorrent)";


                request.AddJsonBody(json);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);


                response = client.Execute(request);
              

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
                while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
                dynamic resp = JsonConvert.DeserializeObject(response.Content);
                List<CategoryProductModel> items = new List<CategoryProductModel>();
                
              
                if (resp != null)
                {    
                    try
                    {
                        foreach (var i in resp.data)
                        {
                            CategoryProductModel item = new CategoryProductModel
                            {
                                Id = i.id,
                                Name = i.name,
                                Brand = i.brand,
                                Seller = i.seller,
                                Category = i.category,
                                Color = i.color,
                                Balance = i.balance,
                                Comments = i.comments,
                                Rating = i.rating,
                                FinalPrice = i.final_price,
                                FinalPriceMax = i.final_price_max,
                                FinalPriceMin = i.final_price_min,
                                FinalPriceAverage = i.final_price_average,
                                BasicSale = i.basic_sale,
                                BasicPrice = i.basic_price,
                                Sales = i.sales,
                                Revenue = i.revenue,
                                RevenuePotential = i.revenue_potential,
                                LostProfit = i.lost_profit,
                                DaysInStock = i.days_in_stock,
                                DaysWithSales = i.days_with_sales,
                                AverageIfInStock = i.average_if_in_stock,
                                Thumb = i.thumb,
                                ClientPrice = i.client_price,
                                ClientSale = i.client_sale,
                                PromoSale = i.promo_sale,
                                StartPrice = i.start_price
                            };
                            items.Add(item);
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    
                    }
                  
                }
                //     System.Windows.MessageBox.Show(response.StatusCode.ToString());
                return items;
            }
            else
            {
                throw new NoAPIKeyException();
            }
        }

        public static List<CategoryTrendModel> GetCategoryTrends(string path, DateTime d1, DateTime d2)
        {
            dynamic json = new
            {
                startRow = 1,
                endRow = 5000,
                sortModel = new dynamic[0],
                filterModel = new dynamic[0],
            };

            if (!string.IsNullOrEmpty(APIKey))
            {
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/category/trends" +
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d2={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={path}");
                request = new RestRequest(Method.GET);
                SetHeaders(request);
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 OPR/78.0.4093.184 (Edition utorrent)";

                //request.AddJsonBody(json);
                //request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                response = client.Execute(request);

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
         
                while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
                dynamic resp = JsonConvert.DeserializeObject(response.Content);
                List<CategoryTrendModel> items = new List<CategoryTrendModel>();

                if (resp != null)
                {
                    foreach (var i in resp)
                    {
                        CategoryTrendModel item = new CategoryTrendModel
                        {
                            Sales = i.sales,
                            ProductRevenue = i.product_revenue,
                            Items = i.items,
                            Brands = i.brands,
                            Sellers = i.sellers,
                            Revenue = i.revenue,
                            ProductWithSells = i.items_with_sells
                        };
                        DateTime week = default;
                        if (DateTime.TryParse(i.week.ToString(), out week))
                        {
                            item.Week = week;
                        }
                        
                        items.Add(item);
                    }
                }
                  
                return items;
            }
            else
            {
                throw new NoAPIKeyException();
            }
        }

        public static List<BrandSeller> GetBrandPositions(string brand, DateTime d1, DateTime d2)
        {
            dynamic json = new
            {
                startRow = 1,
                endRow = 5000,
                sortModel = new dynamic[0], 
                filterModel = new dynamic[0],
            };

            if (!string.IsNullOrEmpty(APIKey))
            {
                RestClient client = new RestClient("https://mpstats.io/api/wb/get/brand" +
                   $"?d1={d1.ToString("yyyy-MM-dd")}&d2={d2.ToString("yyyy-MM-dd")}" +
                   $"&path={brand}");

                request = new RestRequest(Method.POST);
                SetHeaders(request);

                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
           "(KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36 OPR/78.0.4093.184 (Edition utorrent)";
                request.AddJsonBody(json);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                response = client.Execute(request);

                StatusCodeChecker.CheckStatusCode((int)response.StatusCode);
                while (((int)response.StatusCode) == 202)
                {
                    response = client.Execute(request);
                }
                dynamic resp = JsonConvert.DeserializeObject(response.Content);
                List<BrandSeller> items = new List<BrandSeller>();
                if (resp != null)
                {

                    foreach (var i in resp.data)
                    {
                        BrandSeller item = new BrandSeller
                        {
                            Name = i.name,
                            Comments = i.comments,
                            Color = i.color,
                            DaysInStock = i.days_in_stock,
                            Seller = i.seller,
                            Brand = i.brand,
                            ID = i.id,
                            Balance = i.balance,
                            Category = i.category,
                            FinalPrice = i.final_price,
                            Sales = i.sales,
                            Rating = i.rating,
                            Revenue = i.revenue
                        };
                        items.Add(item);
                    }
                }

                //System.Windows.MessageBox.Show(items.Sum(f => f.Revenue).ToString());
                return items;
            }
            else
            {
                throw new NoAPIKeyException();
            }
        }


        private static void SetHeaders(IRestRequest request)
        {
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*" +
                "/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("sec-ch-ua", @"Not A; Brand"";v=""99"", ""Chromium"";v=""92"", ""Opera"";v=""78");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("Sec-Fetch-Dest", "document");
            request.AddHeader("Sec-Fetch-Mode", "navigate");
            request.AddHeader("Sec-Fetch-Site", "none");
            request.AddHeader("Sec-Fetch-User", "?1");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Host", "mpstats.io");
            request.AddHeader("X-Mpstats-TOKEN", APIKey);
       
        }
    }
}
