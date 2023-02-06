using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplat : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetup>().footPrinter.isPrinting == true)
        {
            FootPrintManager.manager.StopSpawnFootPrint(other.GetComponent<PlayerSetup>().photonView.ViewID);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetup>().footPrinter.isPrinting == true)
        {
            FootPrintManager.manager.StopSpawnFootPrint(other.GetComponent<PlayerSetup>().photonView.ViewID);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetup>().footPrinter.isPrinting == false)
        {
            FootPrintManager.manager.SpawnFootPrint(other.GetComponent<PlayerSetup>().photonView.ViewID);
        }
    }
}
