using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintManagerTutorial : MonoBehaviour
{
    public static FootPrintManagerTutorial manager;
    public List<FootPrintTutorial> notUse;
    public List<FootPrintTutorial> inUse;
    public float timePrinter;
    private void Awake()
    {
        manager = this;
    }
    public void SpawnFootPrint(PlayerSetupTutorial playerSetup)
    {
        SpawnFootPrintCmd(playerSetup);
    }

    void SpawnFootPrintCmd(PlayerSetupTutorial playerSetup)
    {
        playerSetup.footPrinter.StartCoroutine(playerSetup.footPrinter.FootPrinter());
    }
    public void StopSpawnFootPrint(PlayerSetupTutorial playerSetup)
    {
        StopSpawnFootPrintCmd(playerSetup);
    }
    
    void StopSpawnFootPrintCmd(PlayerSetupTutorial playerSetup)
    {
        playerSetup.footPrinter.isPrinting = false;
    }
}
