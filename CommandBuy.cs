﻿using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;

namespace ZaupShop
{
    public class CommandBuy : IRocketCommand
    {
        public bool AllowFromConsole
        {
            get
            {
                return false;
            }
        }
        public string Name
        {
            get
            {
                return "buy";
            }
        }
        public string Help
        {
            get
            {
                return "Allows you to buy items from the shop.";
            }
        }
        public string Syntax
        {
            get
            {
                return "[v.]<name or id> [amount] [25 | 50 | 75 | 100]";
            }
        }
        public List<string> Aliases
        {
            get { return new List<string>(); }
        }
        public List<string> Permissions
        {
            get { return new List<string>(); }
        }
        public void Execute(IRocketPlayer playerid, string[] msg)
        {
            ZaupShop.Instance.Buy((UnturnedPlayer)playerid, msg);
        }
    }
}