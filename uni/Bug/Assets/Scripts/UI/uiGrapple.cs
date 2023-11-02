using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class uiGrapple : MonoBehaviour
{
    [SerializeField] GameObject
        uiHookAmmoInfinite,
        uiHookAmmoDivider;
    [SerializeField] TextMeshProUGUI
        uiHookAmmo;
    void Awake()
    {

    }
    void Start()
    {
        
    }
    void Update()
    {

    }
    public void Refresh(Grapple.ammoStateEnum ammoState, float ammoStateCurrent = 0, float ammoStateMax = 0)
    {
        uiHookAmmoInfinite.SetActive(false);
        uiHookAmmo.text = "";
        uiHookAmmoDivider.SetActive(false);
        switch (ammoState)
        {
            case Grapple.ammoStateEnum.infinite:
                {
                    uiHookAmmoInfinite.SetActive(true);
                    break;
                }
            case Grapple.ammoStateEnum.distanceLimited:
                {
                    uiHookAmmo.text = ammoStateCurrent + "m\n" + ammoStateMax + "m";
                    uiHookAmmoDivider.SetActive(true);
                    break;
                }
            case Grapple.ammoStateEnum.usesLimited:
                {
                    uiHookAmmo.text = ammoStateCurrent + "/" + ammoStateMax;
                    break;
                }
        }
    }
}