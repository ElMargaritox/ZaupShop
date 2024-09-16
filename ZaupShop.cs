﻿using System.Collections.Generic;
using System.Linq;
using fr34kyn01535.Uconomy;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;


using Logger = Rocket.Core.Logging.Logger;

namespace ZaupShop
{
    public class ZaupShop : RocketPlugin<ZaupShopConfiguration>
    {
        public DatabaseMgr ShopDB;
        public static ZaupShop Instance;
        public string ItemShopTableName;
        public string VehicleShopTableName;
        public string GroupListTableName;
        public GroupManager GroupManager;
        
        #region Events

        public delegate void PlayerShopBuy(UnturnedPlayer player, decimal totalCost, byte itemAmount, ushort itemID,
            string type = "item");

        public delegate void PlayerShopSell(UnturnedPlayer player, decimal totalIncome, byte itemAmount, ushort itemID);

        public event PlayerShopBuy OnShopBuy;
        public event PlayerShopSell OnShopSell;
        
        #endregion

        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                {
                    "buy_command_usage",
                    "Usage: /buy [v.]<name or id> [amount] [quality of 25, 50, 75, or 100] (last 2 optional and only for items, default 1 amount, 100 quality)."
                },
                {
                    "cost_command_usage",
                    "Usage: /cost [v.]<name or id>."
                },
                {
                    "sell_command_usage",
                    "Usage: /sell <name or id> [amount] (optional)."
                },
                {
                    "shop_command_usage",
                    "Usage: /shop <add/rem/chng/buy/group> [v.]<itemid> <cost> | <cost> is not required for rem, buy is only for items."
                },
                {
                    "shop_group_usage",
                    "Usage: /shop group <create/delgroup/add/rem>"
                },
                {
                    "shop_group_create_usage",
                    "Usage: /shop group create <groupName> <wlist/blist>"
                },
                {
                    "shop_group_del_usage",
                    "Usage: /shop group delgroup <groupName>"
                },
                {
                    "shop_group_change_usage",
                    "Usage: /shop group <add/rem> <groupName> [v.]<id>"
                },
                {
                    "error_giving_item",
                    "There was an error giving you {0}.  You have not been charged."
                },
                {
                    "error_getting_cost",
                    "There was an error getting the cost of {0}!"
                },
                {
                    "item_cost_msg",
                    "The item {0} costs {1} {2} to buy and gives {3} {4} when you sell it."
                },
                {
                    "vehicle_cost_msg",
                    "The vehicle {0} costs {1} {2} to buy."
                },
                {
                    "item_buy_msg",
                    "You have bought {5} {0} for {1} {2}.  You now have {3} {4}."
                },
                {
                    "vehicle_buy_msg",
                    "You have bought 1 {0} for {1} {2}.  You now have {3} {4}."
                },
                {
                    "not_enough_currency_msg",
                    "You do not have enough {0} to buy {1} {2}."
                },
                {
                    "buy_items_off",
                    "I'm sorry, but the ability to buy items is turned off."
                },
                {
                    "buy_vehicles_off",
                    "I'm sorry, but the ability to buy vehicles is turned off."
                },
                {
                    "item_not_available",
                    "I'm sorry, but {0} is not available in the shop."
                },
                {
                    "vehicle_not_available",
                    "I'm sorry, but {0} is not available in the shop."
                },
                {
                    "could_not_find",
                    "I'm sorry, I couldn't find an id for {0}."
                },
                {
                    "sell_items_off",
                    "I'm sorry, but the ability to sell items is turned off."
                },
                {
                    "not_have_item_sell",
                    "I'm sorry, but you don't have any {0} to sell."
                },
                {
                    "not_enough_items_sell",
                    "I'm sorry, but you don't have {0} {1} to sell."
                },
                {
                    "not_enough_ammo_sell",
                    "I'm sorry, but you don't have enough ammo in {0} to sell."
                },
                {
                    "sold_items",
                    "You have sold {0} {1} to the shop and receive {2} {3} in return.  Your balance is now {4} {5}."
                },
                {
                    "no_sell_price_set",
                    "The shop is not buying {0} right now"
                },
                {
                    "no_itemid_given",
                    "An itemid is required."
                },
                {
                    "no_cost_given",
                    "A cost is required."
                },
                {
                    "invalid_amt",
                    "You have entered in an invalid amount."
                },
                {
                    "v_not_provided",
                    "You must specify v for vehicle or just an item id.  Ex. /shop rem/101"
                },
                {
                    "invalid_id_given",
                    "You need to provide a valid item or vehicle id."
                },
                {
                    "no_permission_shop_chng",
                    "You don't have permission to use the shop chng msg."
                },
                {
                    "no_permission_shop_add",
                    "You don't have permission to use the shop add msg."
                },
                {
                    "no_permission_shop_rem",
                    "You don't have permission to use the shop rem msg."
                },
                {
                    "no_permission_shop_buy",
                    "You don't have permission to use the shop buy msg."
                },
                {
                    "changed",
                    "changed"
                },
                {
                    "added",
                    "added"
                },
                {
                    "shop_group_created",
                    "You created a shop group: {0} of type: {1}."
                },
                {
                    "shop_group_create_failed",
                    "Failed to create shop group: {0}."
                },
                {
                    "shop_group_deleted",
                    "You deleted the shop group: {0}."
                },
                {
                    "shop_group_delete_failed",
                    "Failed to delete shop group: {0}."
                },
                {
                    "shop_group_added_id",
                    "You added ID: {0} to shop group: {1}."
                },
                {
                    "shop_group_add_failed",
                    "Failed to add ID: {0} to shop group: {1}."
                },
                {
                    "shop_group_removed_id",
                    "You removed ID: {0} from shop group: {1}."
                },
                {
                    "shop_group_remove_failed",
                    "Failed to remove ID: {0} from shop group: {1}."
                },
                {
                    "changed_or_added_to_shop",
                    "You have {0} the {1} with cost {2} to the shop."
                },
                {
                    "error_adding_or_changing",
                    "There was an error adding/changing {0}!"
                },
                {
                    "removed_from_shop",
                    "You have removed the {0} from the shop."
                },
                {
                    "not_in_shop_to_remove",
                    "{0} wasn't in the shop, so couldn't be removed."
                },
                {
                    "not_in_shop_to_set_buyback",
                    "{0} isn't in the shop so can't set a buyback price."
                },
                {
                    "set_buyback_price",
                    "You set the buyback price for {0} to {1} in the shop."
                },
                {
                    "invalid_shop_command",
                    "You entered an invalid shop command."
                },
                {
                    "blacklisted",
                    "You are forbidden from buying {0}."
                },
                {
                    "not_whitelisted",
                    "You are forbidden from buying {0}."
                }
            };

        protected override void Load()
        {
            Instance = this;

            ItemShopTableName = Instance.Configuration.Instance.ItemShopTableName;
            VehicleShopTableName = Instance.Configuration.Instance.VehicleShopTableName;
            GroupListTableName = Instance.Configuration.Instance.GroupListTableName;

            ShopDB = new DatabaseMgr();
            GroupManager = new GroupManager();
        }

        protected override void Unload()
        {
            ShopDB = null;
            Instance = null;
        }

        public void TellPlayer(SteamPlayer player, string translationKey, params object[] translationParameters) =>
            ChatManager.serverSendMessage(Instance.Translate(translationKey, translationParameters), Palette.SERVER,
                toPlayer: player);

        public void TellConsole(string translationKey, params object[] translationParameters) => Logger.Log(Instance.Translate(translationKey, translationParameters));

        public void RaiseBuyVehicle(UnturnedPlayer player, decimal price, ushort vehicleID)
        {
            Instance.OnShopBuy?.Invoke(player, price, 1, vehicleID, "vehicle");
            
            player.Player.gameObject.SendMessage("ZaupShopOnBuy", new object[] {player, price, 1, vehicleID, "vehicle"},
                SendMessageOptions.DontRequireReceiver);
        }

        public void RaiseBuyItem(UnturnedPlayer player, decimal price, byte amount, ushort itemID)
        {
            Instance.OnShopBuy?.Invoke(player, price, amount, itemID);

            player.Player.gameObject.SendMessage("ZaupShopOnBuy",
                new object[] {player, price, amount, itemID, "item"}, SendMessageOptions.DontRequireReceiver);
        }

        public void RaiseSellItem(UnturnedPlayer uPlayer, decimal income, byte items, ushort itemID)
        {
            Instance.OnShopSell?.Invoke(uPlayer, income, items, itemID);
            uPlayer.Player.gameObject.SendMessage("ZaupShopOnSell", new object[] {uPlayer, income, items, itemID});
        }
    }
}