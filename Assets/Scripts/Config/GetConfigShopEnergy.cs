using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetConfigShopEnergy : MonoBehaviour
{
    public TMPro.TextMeshProUGUI energyPrice;
    public TMPro.TextMeshProUGUI energyGift;
    public TMPro.TextMeshProUGUI energyAD;
    private void OnEnable()
    {
        energyPrice.text = ConfigEnergyShop.energyPrice.ToString();
        energyGift.text = ConfigEnergyShop.energyGift.ToString();
        energyAD.text = ConfigEnergyShop.energyAD.ToString();
    }
}
