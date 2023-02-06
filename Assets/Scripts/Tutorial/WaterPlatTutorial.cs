using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlatTutorial : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetupTutorial>().footPrinter.isPrinting == true)
        {
            FootPrintManagerTutorial.manager.StopSpawnFootPrint(other.GetComponent<PlayerSetupTutorial>());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetupTutorial>().footPrinter.isPrinting == true)
        {
            FootPrintManagerTutorial.manager.StopSpawnFootPrint(other.GetComponent<PlayerSetupTutorial>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetupTutorial>().footPrinter.isPrinting == false)
        {
            FootPrintManagerTutorial.manager.SpawnFootPrint(other.GetComponent<PlayerSetupTutorial>());
        }
    }
}
