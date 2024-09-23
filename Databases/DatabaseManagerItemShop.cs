using System;
using System.Collections.Generic;
using System.Globalization;
using EnvyGarage.DataStorage;
using System.Security.Principal;

using Rocket.Core.Logging;
using Steamworks;

using System.Linq;
using ZaupShop.Models;
using fr34kyn01535.Uconomy;
using SDG.Unturned;

namespace ZaupShop.Databases
{
    public class DatabaseManagerItemShop
    {

        private List<ItemShop> Data;
        private DataStorage<List<ItemShop>> ItemShopStorage { get; set; }

        internal DatabaseManagerItemShop()
        {
            System.IO.Directory.CreateDirectory($"{Environment.CurrentDirectory}/Plugins/ZaupShop/Database");
            this.ItemShopStorage = new DataStorage<List<ItemShop>>(ZaupShop.Instance.Directory + "/Database", "ItemShopStorage.json");
            Logger.Log("Loading json... OK  [ITEM SHOP STORAGE]");
            CheckSchema();
        }


        public decimal GetItemCost(int id)
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

        public decimal GetItemBuyPrice(int id)
        {
            try
            {
                return this.Data.FirstOrDefault(x => x.Id == id).BuyBack;
            }
            catch 
            {
                return 0;
            }

        }

        public bool SetBuyPrice(int id, decimal cost)
        {
            try
            {
                var existe =  this.Data.FirstOrDefault(x => x.Id == id);
                if(existe != null)
                {
                    existe.BuyBack = cost;
                    ItemShopStorage.Save(this.Data);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteItem(int id)
        {
            try
            {
                var exist =   this.Data.FirstOrDefault(x => x.Id == id);
                if(exist != null)
                {
                    this.Data.Remove(exist);
                    ItemShopStorage.Save(this.Data);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool AddItem(int id, string name, decimal cost, bool change)
        {
            try
            {
                var exist = this.Data.FirstOrDefault(x => x.Id == id);

                if (exist == null)
                {
                    ItemShop data = new ItemShop()
                    {
                        BuyBack = 0,
                        Cost = cost,
                        Id = id,
                        ItemName = name,
                    };
                    this.Data.Add(data);
                    ItemShopStorage.Save(this.Data);
                    return true;
                }
                else
                {
                    exist.ItemName = name;
                    exist.Cost = cost;
                    ItemShopStorage.Save(this.Data);
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
            Data = ItemShopStorage.Read();
            if (Data == null)
            {
                Data = new List<ItemShop>();
                ItemShopStorage.Save(Data);
            }
        }


    }
}