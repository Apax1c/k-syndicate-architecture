using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
    public class IAPService : IIAPService
    {
        private readonly IAPProvider _iapProvider;
        private readonly IPersistentProgressService _progressService;

        public bool IsInitialized => _iapProvider.IsInitialized;
        public event Action Initialized;

        public IAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
        {
            _iapProvider = iapProvider;
            _progressService = progressService;
        }

        public void Initialize()
        {
            _iapProvider.Initialize(this);
            _iapProvider.Initialized += () => Initialized?.Invoke();
        }

        public List<ProductDescription> Products() => 
            ProductDescriptions().ToList();

        public void StartPurchase(string productId) => 
            _iapProvider.StartPurchase(productId);

        public PurchaseProcessingResult ProcessPurchase(Product purchasedProduct)
        {
            ProductConfig productConfig = _iapProvider.Configs[purchasedProduct.definition.id];

            switch (productConfig.ItemType)
            {
                case ItemType.Skull:
                    _progressService.Progress.WorldData.LootData.Add(productConfig.Quantity);
                    _progressService.Progress.PurchaseData.AddPurchase(purchasedProduct.definition.id);
                    break;
            }

            return PurchaseProcessingResult.Complete;
        }

        private IEnumerable<ProductDescription> ProductDescriptions()
        {
            PurchaseData purchaseData = _progressService.Progress.PurchaseData;

            foreach (string productsId in _iapProvider.Products.Keys)
            {
                ProductConfig config = _iapProvider.Configs[productsId];
                Product product = _iapProvider.Products[productsId];

                BoughtIap boughtIap = purchaseData.BoughtIAPs.Find(x => x.IAPid == productsId);
                if (ProductBoughtOut(boughtIap, config))
                    continue;

                yield return new ProductDescription
                {
                    Id = productsId,
                    Config = config,
                    Product = product,
                    AvailablePurchasesLeft = boughtIap != null
                        ? config.MaxPurchaseCount - boughtIap.Count
                        : config.MaxPurchaseCount,
                };
            }
        }

        private static bool ProductBoughtOut(BoughtIap boughtIap, ProductConfig config) => 
            boughtIap != null && boughtIap.Count > config.MaxPurchaseCount;
    }
}