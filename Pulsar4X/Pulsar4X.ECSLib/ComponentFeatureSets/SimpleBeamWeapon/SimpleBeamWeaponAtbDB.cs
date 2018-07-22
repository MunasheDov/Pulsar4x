﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulsar4X.ECSLib
{
    public class SimpleBeamWeaponAtbDB : BaseDataBlob, IComponentDesignAttribute
    {
        [JsonProperty]
        public double MaxRange { get; internal set; }
        [JsonProperty]
        public double DamageAmount { get; internal set; }
        [JsonProperty]
        public double ReloadRate { get; internal set; }

        public SimpleBeamWeaponAtbDB() { }

        public SimpleBeamWeaponAtbDB(double maxRange, double damageAmount, double reloadRate)
        {
            MaxRange = maxRange;
            DamageAmount = damageAmount;
            ReloadRate = reloadRate;
        }

        public SimpleBeamWeaponAtbDB(SimpleBeamWeaponAtbDB db)
        {
            MaxRange = db.MaxRange;
            DamageAmount = db.DamageAmount;
            ReloadRate = db.ReloadRate;
        }

        public override object Clone()
        {
            return new SimpleBeamWeaponAtbDB(this);
        }

        public void OnComponentInstalation(Entity parentEntity, Entity componentInstance)
        {
            if (!parentEntity.HasDataBlob<FireControlAbilityDB>())
            {
                var fcdb = new FireControlAbilityDB();
                parentEntity.SetDataBlob(fcdb);
            }
            var ability = parentEntity.GetDataBlob<FireControlAbilityDB>();
            if (!componentInstance.HasDataBlob<WeaponInstanceStateDB>())
                componentInstance.SetDataBlob(new WeaponInstanceStateDB());
            ability.AddWeaponToParentEntity(componentInstance);

        }
    }
}
