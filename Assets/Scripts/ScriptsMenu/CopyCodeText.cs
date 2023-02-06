using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCodeText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textCode;
    public TMPro.TextMeshProUGUI textBtn;
    public void ClickCopy()
    {
        GUIUtility.systemCopyBuffer = textCode.text;
        textBtn.text = "Copied";
    }
}
