using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintSpawnerTutotial : MonoBehaviour
{
    public float time;
    public bool isPrinting;
    private void Start()
    {
        isPrinting = false;
    }
    public IEnumerator FootPrinter()
    {
        isPrinting = true;
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        if (isPrinting == true)
        {
            InsertPrinter();
            yield return new WaitForSeconds(time);
        }
        else
        {
            yield break;
        }
        isPrinting = false;
    }
    void InsertPrinter()
    {
        FootPrintTutorial footPrinter = FootPrintManagerTutorial.manager.notUse[0];
        footPrinter.transform.position = transform.position;
        footPrinter.transform.rotation = transform.rotation;
        FootPrintManagerTutorial.manager.notUse.Remove(footPrinter);
        FootPrintManagerTutorial.manager.inUse.Add(footPrinter);
        footPrinter.gameObject.SetActive(true);
    }
}
