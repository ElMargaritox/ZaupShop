using System;


using Rocket.Core.Logging;
using ZaupShop.Databases;


namespace ZaupShop
{
    public class DatabaseMgr
    {

        public DatabaseManagerItemShop DatabaseItemshop;
        public DatabaseManagerVehicleShop DatabaseManagerVehicleShop;

        internal DatabaseMgr()
        {
         
            CheckSchema();
        }

        private void CheckSchema()
        {
            string itemShopTableName = ZaupShop.Instance.ItemShopTableName;
            string vehicleShopTableName = ZaupShop.Instance.VehicleShopTableName;
            string groupsTableName = ZaupShop.Instance.GroupListTableName;


            DatabaseItemshop = new DatabaseManagerItemShop();
            DatabaseManagerVehicleShop = new DatabaseManagerVehicleShop();

        }



        public bool AddItem(int id, string name, decimal cost, bool change)
        {

            return DatabaseItemshop.AddItem(id, name, cost, change);

        }

        public bool AddVehicle(int id, string name, decimal cost, bool change)
        {
            return DatabaseManagerVehicleShop.AddVehicle(id, name, cost, change);
        }

        public decimal GetItemCost(int id)
        {
            return DatabaseItemshop.GetItemCost(id);
        }

        public decimal GetVehicleCost(int id)
        {
           return DatabaseManagerVehicleShop.GetVehicleCost(id);
        }

        public bool DeleteItem(int id)
        {
            return DatabaseItemshop.DeleteItem(id);
        }

        public bool DeleteVehicle(int id)
        {
            return DatabaseManagerVehicleShop.DeleteVehicle(id);
        }

        public bool SetBuyPrice(int id, decimal cost)
        {
            return DatabaseItemshop.SetBuyPrice(id, cost);
        }

        public decimal GetItemBuyPrice(int id)
        {
            return DatabaseItemshop.GetItemBuyPrice(id);
        }


    }
}