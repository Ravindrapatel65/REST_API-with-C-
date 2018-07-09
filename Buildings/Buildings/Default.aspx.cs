using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Buildings.Model;
using System.Web.Script.Serialization;

namespace Buildings
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Building[] buildings;
            AnalyticsData[] analyticsDataList;

            try
            {
                HttpWebRequest request = WebRequest.Create("http://jobs.mapsted.com/api/Values/GetBuildingData") as HttpWebRequest;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK) throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                    response.StatusDescription));
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string buildingResponse = sr.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    buildings = js.Deserialize<Building[]>(buildingResponse);

                }


                HttpWebRequest request2 = WebRequest.Create("http://jobs.mapsted.com/api/Values/GetAnalyticsData") as HttpWebRequest;

                using (HttpWebResponse response = request2.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK) throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                    response.StatusDescription));
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string buildingResponse = sr.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    analyticsDataList = js.Deserialize<AnalyticsData[]>(buildingResponse);
                    
                }

                double totalSamsungCost = calculateTotalCostForAManufacturer("Samsung", analyticsDataList);
                int noOftimes = calculateTimesOfAnItemIsPurchased(47, analyticsDataList);
                double totalPurchaseCostForItemCategory7 = calculatePurchaseCostForAnItemCategory(7, analyticsDataList);
                double totalPurchaseCostInOntario = calculateTotalPurchaseCostInAState("Ontario", analyticsDataList, buildings);
                double totalPurchaseCostInUS = calculateTotalPurchaseCostInACountry("United States", analyticsDataList, buildings);
                Building buildingWithLargestPurchase = findHighestPurchaseCostBuilding(analyticsDataList, buildings);

                lbl_Samsung.Text = totalSamsungCost.ToString();
                lbl_Item47.Text = noOftimes.ToString();
                lbl_ItemCategory7.Text = totalPurchaseCostForItemCategory7.ToString();
                lbl_Ontario.Text = totalPurchaseCostInOntario.ToString();
                lbl_US.Text = totalPurchaseCostInUS.ToString();
                lbl_HighestPurchaseCost.Text = "Building Id: " + buildingWithLargestPurchase.building_id + "   Building Name: "+buildingWithLargestPurchase.building_name;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }  

        }

        /// <summary>
        /// This method is used to find the building which has the most total purchase cost
        /// </summary>
        /// <returns>The building object which the most total purchase cost.</returns>
        private Building findHighestPurchaseCostBuilding(AnalyticsData[] analyticsDataList, Building[] buildings)
        {
            double largestPurhcaseCost = 0;
            Building buildingWithLargestPurchase = null;
            foreach (Building building in buildings)
            {
                double totalPurchaseCost = calculateTotalPurchaseCostForABuilding(building.building_id, analyticsDataList);
                if(totalPurchaseCost > largestPurhcaseCost ){
                    largestPurhcaseCost = totalPurchaseCost;
                    buildingWithLargestPurchase = building;
                }
            }
            return buildingWithLargestPurchase;
        }

        /// <summary>
        /// This method is used to find the total purchase cost for a building id
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="analyticsDataList"></param>
        /// <returns>The total purchase cost for the building id</returns>
        private double calculateTotalPurchaseCostForABuilding(int buildingId, AnalyticsData[] analyticsDataList)
        {
            double totalPurchaseCost = 0;
            foreach (AnalyticsData analyticsData in analyticsDataList)
            {
                UsageStatistics usage_statistics = analyticsData.usage_statistics;
                List<SessionInfo> sessionInfos = usage_statistics.session_infos;
                foreach (SessionInfo sessionInfo in sessionInfos)
                {
                    if (sessionInfo.building_id == buildingId)
                    {
                        List<Purchase> purchases = sessionInfo.purchases;
                        foreach (Purchase purchase in purchases)
                        {
                            totalPurchaseCost = totalPurchaseCost + purchase.cost;
                        }
                    }
                }
            }
            totalPurchaseCost = Math.Round(totalPurchaseCost, 2);
            return totalPurchaseCost;
        }


        /// <summary>
        /// This method calculates the total purchase cost in a state
        /// </summary>
        /// <param name="country"></param>
        /// <param name="analyticsDataList"></param>
        /// <param name="buildings"></param>
        /// <returns>The total purchase cost in a state</returns>
        private double calculateTotalPurchaseCostInAState(String state, AnalyticsData[] analyticsDataList, Building[] buildings)
        {
            double totalPurchaseCost = 0;
            foreach (Building building in buildings)
            {
                if (building.state == state)
                {
                    int buildingId = building.building_id;
                    foreach (AnalyticsData analyticsData in analyticsDataList)
                    {
                        UsageStatistics usage_statistics = analyticsData.usage_statistics;
                        List<SessionInfo> sessionInfos = usage_statistics.session_infos;
                        foreach (SessionInfo sessionInfo in sessionInfos)
                        {
                            if (sessionInfo.building_id == buildingId)
                            {
                                List<Purchase> purchases = sessionInfo.purchases;
                                foreach (Purchase purchase in purchases)
                                {
                                    totalPurchaseCost = totalPurchaseCost + purchase.cost;
                                }
                            }
                        }
                    }
                }
            }
            totalPurchaseCost = Math.Round(totalPurchaseCost, 2);
            return totalPurchaseCost;
        }


        /// <summary>
        /// This method calculates the total purchase cost in a country
        /// </summary>
        /// <param name="country"></param>
        /// <param name="analyticsDataList"></param>
        /// <param name="buildings"></param>
        /// <returns>The total purchase cost in the country</returns>
        private double calculateTotalPurchaseCostInACountry(String country, AnalyticsData[] analyticsDataList, Building[] buildings)
        {
            double totalPurchaseCost = 0;
            foreach (Building building in buildings)
            {
                if (building.country == country)
                {
                    int buildingId = building.building_id;
                    foreach (AnalyticsData analyticsData in analyticsDataList)
                    {
                        UsageStatistics usage_statistics = analyticsData.usage_statistics;
                        List<SessionInfo> sessionInfos = usage_statistics.session_infos;
                        foreach (SessionInfo sessionInfo in sessionInfos)
                        {
                            if (sessionInfo.building_id == buildingId)
                            {
                                List<Purchase> purchases = sessionInfo.purchases;
                                foreach (Purchase purchase in purchases)
                                {
                                    totalPurchaseCost = totalPurchaseCost + purchase.cost;
                                }
                            }
                        }
                    }
                }
            }
            totalPurchaseCost = Math.Round(totalPurchaseCost, 2);
            return totalPurchaseCost;
        }

        /// <summary>
        /// This method calculates the total purchase cost of all items in a category.
        /// </summary>
        /// <param name="itemCategoryId"></param>
        /// <param name="analyticsDataList"></param>
        /// <returns>Total purchase cost of all items in the category</returns>
        private double calculatePurchaseCostForAnItemCategory(int itemCategoryId, AnalyticsData[] analyticsDataList)
        {
            double totalPurchaseCost = 0;
            foreach (AnalyticsData analyticsData in analyticsDataList)
            {
                UsageStatistics usage_statistics = analyticsData.usage_statistics;
                List<SessionInfo> sessionInfos = usage_statistics.session_infos;
                foreach (SessionInfo sessionInfo in sessionInfos)
                {
                    List<Purchase> purchases = sessionInfo.purchases;
                    foreach (Purchase purchase in purchases)
                    {
                        if (purchase.item_category_id == itemCategoryId)
                        {
                            totalPurchaseCost = totalPurchaseCost + purchase.cost;
                        }
                    }
                }
            }
            totalPurchaseCost = Math.Round(totalPurchaseCost, 2);
            return totalPurchaseCost;
        }

        /// <summary>
        /// Calculate the number of times an item is purchased
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="analyticsDataList"></param>
        /// <returns>Number of times the item is purchased</returns>
        private int calculateTimesOfAnItemIsPurchased(int itemId, AnalyticsData[] analyticsDataList)
        {
            int  noOftimes = 0;
            foreach (AnalyticsData analyticsData in analyticsDataList)
            {
                UsageStatistics usage_statistics = analyticsData.usage_statistics;
                List<SessionInfo> sessionInfos = usage_statistics.session_infos;
                foreach (SessionInfo sessionInfo in sessionInfos)
                {
                    List<Purchase> purchases = sessionInfo.purchases;
                    foreach (Purchase purchase in purchases)
                    {
                        if (purchase.item_id == itemId)
                        {
                            noOftimes++;
                        }
                    }
                }
            }
            return noOftimes;
        }

        /// <summary>
        /// This method is used to calculate the  total cost for passed manufacturer.
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <param name="analyticsDataList"></param>
        /// <returns>Total cost for passed manufacturer</returns>
        private double calculateTotalCostForAManufacturer(String manufacturer, AnalyticsData[] analyticsDataList)
        {
            // Total purchase cost for a manufacture devices
            double totalSamsungCost = 0;
            foreach (AnalyticsData analyticsData in analyticsDataList)
            {
                if (analyticsData.manufacturer == manufacturer)
                {
                    UsageStatistics usage_statistics = analyticsData.usage_statistics;
                    List<SessionInfo> sessionInfos = usage_statistics.session_infos;
                    foreach (SessionInfo sessionInfo in sessionInfos)
                    {
                        List<Purchase> purchases = sessionInfo.purchases;
                        foreach (Purchase purchase in purchases)
                        {
                            totalSamsungCost = totalSamsungCost + purchase.cost;
                        }
                    }
                }
            }
            totalSamsungCost = Math.Round(totalSamsungCost, 2);
            return totalSamsungCost;

        }

    }
}