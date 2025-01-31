﻿using System;
using UnityEngine.Purchasing;
using UnityEngine.Serialization;

namespace CodeBase.Services.IAP
{
    [Serializable]
    public class ProductConfig
    {
        public string Id;
        public ProductType ProductType;

        public int MaxPurchaseCount;
        public ItemType ItemType;
        public int Quantity;
        public string Price;
        public string Icon;
    }
}