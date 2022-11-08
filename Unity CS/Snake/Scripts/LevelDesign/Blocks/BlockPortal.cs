using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPortal : Block
{
    [SerializeField] private List<PortalSettings> _portalSettings = new List<PortalSettings>();
    [SerializeField] private List<Portal> _portals = new List<Portal>();

    // Start is called before the first frame update
    void Start()
    {
        if (_portalSettings.Count != _portals.Count)
            Debug.LogError("PortalSettings and Portals do not have the same length, please adjust them");

        ApplySettingsToPortals();
    }

    private void ApplySettingsToPortals()
    {
        int lIndex = 0;
        PortalSettings lPortalSettings;
        foreach (Portal portal in _portals)
        {
            lPortalSettings = _portalSettings[lIndex];

            if (!lPortalSettings.randomizeIsBonus && lPortalSettings.randomizeIsAddition && lPortalSettings.randomizeImpactValue)
                portal.SetupPortal(lPortalSettings.isBonus);
            else if (!lPortalSettings.isBonus && !lPortalSettings.randomizeIsAddition && lPortalSettings.randomizeImpactValue)
                portal.SetupPortal(lPortalSettings.isBonus, lPortalSettings.isAddition);
            else if (!lPortalSettings.isBonus && !lPortalSettings.randomizeIsAddition && !lPortalSettings.randomizeImpactValue)
                portal.SetupPortal(lPortalSettings.isBonus, lPortalSettings.isAddition, lPortalSettings.impactValue);
            else portal.SetupPortal();

            lIndex++;
        }
    }
}
