using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class PurchaseData
    {
        public List<BoughtIap> BoughtIAPs = new List<BoughtIap>();

        public Action Changed;
        
        public void AddPurchase(string id)
        {
            BoughtIap boughtIap = Product(id);

            if (boughtIap != null) 
                boughtIap.Count++;
            else
                BoughtIAPs.Add(new BoughtIap {IAPid = id, Count = 1});
            
            Changed?.Invoke();
        }

        private BoughtIap Product(string id) => 
            BoughtIAPs.Find(x => x.IAPid == id);
    }
}