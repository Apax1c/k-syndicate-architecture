using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.IAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopItem : MonoBehaviour
    {
        public Button BuyItemButton;
        public TextMeshProUGUI PriceText;
        public TextMeshProUGUI QuantityText;
        public TextMeshProUGUI AvailableItemsLeftText;
        public Image Icon;
        
        private ProductDescription _productionDescription;
        private IIAPService _iapService;
        private IAssetProvider _assets;

        public void Construct(IIAPService iapService, IAssetProvider assets, ProductDescription productionDescription)
        {
            _iapService = iapService;
            _assets = assets;
            
            _productionDescription = productionDescription;
        }

        public void Initialize()
        {
            BuyItemButton.onClick.AddListener(OnBuyItemClick);

            PriceText.text = _productionDescription.Config.Price;
            QuantityText.text = _productionDescription.Config.Quantity.ToString();
            AvailableItemsLeftText.text = _productionDescription.AvailablePurchasesLeft.ToString();
            Icon.sprite = _assets.LoadSprite<Sprite>(_productionDescription.Config.Icon);
        }

        private void OnBuyItemClick() => 
            _iapService.StartPurchase(_productionDescription.Id);
    }
}