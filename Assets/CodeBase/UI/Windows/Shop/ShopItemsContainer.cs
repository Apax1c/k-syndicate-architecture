using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.IAP;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopItemsContainer : MonoBehaviour
    {
        public GameObject[] ShopUnavailableObjects;
        public Transform Parent;
        private IIAPService _iapService;
        private IPersistentProgressService _progressService;
        private IAssetProvider _assets;
        
        private readonly List<GameObject> _shopItems = new List<GameObject>();

        public void Construct(IIAPService iapService, IPersistentProgressService progressService, IAssetProvider assets)
        {
            _iapService = iapService;
            _progressService = progressService;
            _assets = assets;
        }

        public void Initialize() => 
            RefreshAvailableItems();

        public void Subscribe()
        {
            _iapService.Initialized += RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed += RefreshAvailableItems;
        }

        public void Cleanup()
        {
            _iapService.Initialized -= RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed -= RefreshAvailableItems;
        }

        private void RefreshAvailableItems()
        {
            UpdateShopUnavailableObjects();

            if (_iapService != null && !_iapService.IsInitialized)
                return;

            ClearShopItems();
            
            FillShopItems();
        }

        private void UpdateShopUnavailableObjects()
        {
            foreach (GameObject shopUnavailableObject in ShopUnavailableObjects)
            {
                shopUnavailableObject.SetActive(_iapService != null && !_iapService.IsInitialized);
            }
        }

        private void ClearShopItems()
        {
            foreach (GameObject shopItem in _shopItems) 
                Destroy(shopItem);
        }

        private void FillShopItems()
        {
            foreach (ProductDescription productDescription in _iapService.Products())
            {
                GameObject shopItemObject = _assets.Instantiate(AssetPath.ShopItem, Parent);
                ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();
                
                shopItem.Construct(_iapService, _assets, productDescription);
                shopItem.Initialize();
                
                _shopItems.Add(shopItemObject);
            }
        }
    }
}