using TMPro;
using UnityEngine;

public class uiAmmo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI uiAmmoMagCurrent, uiAmmoStock;
    void Start()
    {
        
    }

    void Update()
    {
        uiAmmoMagCurrent.text = Player.instance.weaponEquipped.weaponAmmoMagCurrent.ToString();
        uiAmmoStock.text = Player.instance.weaponEquipped.weaponAmmoStock.ToString();

        float magPercent = (float)Player.instance.weaponEquipped.weaponAmmoMagCurrent / (float)Player.instance.weaponEquipped.weaponAmmoMagMax;
        uiAmmoMagCurrent.color = new Color(1, magPercent, magPercent, 1);

        if (Player.instance.weaponEquipped.weaponAmmoStock < Player.instance.weaponEquipped.weaponAmmoMagMax) 
        { 
            uiAmmoStock.color = new Color(1, 0, 0, 1);
        }
        else 
        { 
            uiAmmoStock.color = new Color(1, 1, 1, 1);
        }
    }
}