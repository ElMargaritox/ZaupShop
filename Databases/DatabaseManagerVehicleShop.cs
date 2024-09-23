using System;
using System.Collections.Generic;
using System.Globalization;
using EnvyGarage.DataStorage;
using System.Security.Principal;

using Rocket.Core.Logging;
using Steamworks;

using System.Linq;
using ZaupShop.Models;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace ZaupShop.Databases
{
    public class DatabaseManagerVehicleShop
    {

        private List<VehicleShop> Data;
        private DataStorage<List<VehicleShop>> VehicleShopStorage { get; set; }

        internal DatabaseManagerVehicleShop()
        {
            System.IO.Directory.CreateDirectory($"{Environment.CurrentDirectory}/Plugins/ZaupShop/Database");
            this.VehicleShopStorage = new DataStorage<List<VehicleShop>>(ZaupShop.Instance.Directory + "/Database", "ItemShopStorage.json");
            Logger.Log("Loading json... OK  [VEHICLE SHOP STORAGE]");
            CheckSchema();
        }

        public  decimal GetVehicleCost(int id)
        {
            try
            {
                return this.Data.FirstOrDefault(x => x.Id == id).Cost;
            }
            catch
            {
                return 0;
            }
        }

        public bool DeleteVehicle(int id)
        {

            try
            {
                var exist = this.Data.FirstOrDefault(x => x.Id == id);

                if (exist != null)
                {
                    this.Data.Remove(exist);
                    VehicleShopStorage.Save(this.Data);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool AddVehicle(int id, string name, decimal cost, bool change)
        {
            try
            {
                var exist = this.Data.FirstOrDefault(x => x.Id == id);

                if (exist == null)
                {
                    VehicleShop data = new VehicleShop()
                    {
                        Cost = cost,
                        Id = id,
                        VehicleName = name,
                    };
                    this.Data.Add(data);
                    VehicleShopStorage.Save(this.Data);
                    return true;
                }
                else
                {
                    exist.VehicleName = name;
                    exist.Cost = cost;
                    VehicleShopStorage.Save(this.Data);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }




        internal void CheckSchema()
        {
            Data = VehicleShopStorage.Read();
            if (Data == null)
            {
                Data = new List<VehicleShop>();
                VehicleShopStorage.Save(Data);
            }
        }


    }
}