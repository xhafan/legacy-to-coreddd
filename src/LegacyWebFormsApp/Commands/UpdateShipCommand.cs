﻿using CoreDdd.Commands;

namespace LegacyWebFormsApp.Commands
{
    public class UpdateShipCommand : ICommand
    {
        public int ShipId { get; set; }
        public string ShipName { get; set; }
        public decimal Tonnage { get; set; }
    }
}