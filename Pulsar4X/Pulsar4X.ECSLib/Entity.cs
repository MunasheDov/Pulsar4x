﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Pulsar4X.ECSLib
{

    public delegate void EntityManagerChangeEvent(object sender, EntityManagerChangeEventArgs args);

    public class EntityManagerChangeEventArgs
    {
        public readonly EntityManager OldManager;
        public readonly EntityManager NewManager;

        public EntityManagerChangeEventArgs(EntityManager oldManager, EntityManager newManager)
        {
            OldManager = oldManager;
            NewManager = newManager;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Entity
    {
        [JsonProperty("EntityGuid")]
        public Guid Guid { get; private set; }
        public int ID { get; private set; }
        public EntityManager Manager { get; private set; }
        public ComparableBitArray DataBlobMask { get { return Manager.GetMask(this); } }

        public event EventHandler Deleting;
        public event EventHandler Deleted;

        public event EntityManagerChangeEvent ChangingManagers;
        public event EntityManagerChangeEvent ChangedManagers;

        internal Entity(Guid guid, EntityManager currentManager)
        {
            Guid = guid;
            Manager = currentManager;
        }

        public bool IsValid { get { return Manager.IsValidEntity(this); } }

        public List<BaseDataBlob> GetAllDataBlobs()
        {
            return Manager.GetAllDataBlobsOfEntity(this);
        }

        public T GetDataBlob<T>() where T : BaseDataBlob
        {
            return Manager.GetDataBlob<T>(ID);
        }

        public T GetDataBlob<T>(int typeIndex) where T : BaseDataBlob
        {
            return Manager.GetDataBlob<T>(ID, typeIndex);
        }

        public void SetDataBlob<T>(T dataBlob) where T : BaseDataBlob
        {
            Manager.SetDataBlob(ID, dataBlob);
        }

        public void SetDataBlob(BaseDataBlob dataBlob, int typeIndex)
        {
            Manager.SetDataBlob(ID, dataBlob, typeIndex);
        }

        public void RemoveDataBlob<T>() where T : BaseDataBlob
        {
            Manager.RemoveDataBlob<T>(ID);
        }

        public void RemoveDataBlob(int typeIndex)
        {
            Manager.RemoveDataBlob(ID, typeIndex);
        }

        public void DeleteEntity()
        {
            if (Deleting != null)
            {
                Deleting(this, EventArgs.Empty);
            }

            Manager.RemoveEntity(this);

            if (Deleted != null)
            {
                Deleted(this, EventArgs.Empty);
            }
        }

        public void TransferEntity(EntityManager newManager)
        {
            if (newManager == null)
            {
                throw new ArgumentNullException("newManager");
            }

            if (ChangingManagers != null)
            {
                ChangingManagers(this, new EntityManagerChangeEventArgs(Manager, newManager));
            }

            Manager.TransferEntity(this, newManager);
            Manager = newManager;

            if (ChangedManagers != null)
            {
                ChangedManagers(this, new EntityManagerChangeEventArgs(Manager, newManager));
            }
        }

        internal void SetID(int newID)
        {
            ID = newID;
        }

        public static Entity GetInvalidEntity()
        {
            return new Entity(Guid.Empty, Game.Instance.GlobalManager) {ID = -42};
        }
    }
}
