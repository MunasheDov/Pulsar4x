﻿using System;
using System.Collections.Generic;

namespace Pulsar4X.ECSLib
{
    [StaticData(true, IDPropertyName = "ID")]
    public class ProcessedMaterialSD : ICargoable
    {
        public string Name { get; set; }
        public string Description;
        public Guid ID { get; set; }

        public Dictionary<Guid, int> MineralsRequired;
        public Dictionary<Guid, int> MaterialsRequired;
        public ushort RefineryPointCost;
        public ushort WealthCost;
        public ushort OutputAmount;
        public Guid CargoTypeID { get; set; }
        public int Mass { get; set; }
    }
}