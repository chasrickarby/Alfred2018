using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlfredLauncher : MonoBehaviour
{
    public string AlfredUrl = "http://alfred-hack.eastus.cloudapp.azure.com/Alfred/";
    public StringReference AddressOfLastAccess;
    public StringReference OfficeLocation;

    public void ButtonClicked()
    {
        if (!AddressOfLastAccess.Value.Equals(""))
        {
            Application.OpenURL(AlfredUrl + "?room=" + AddressOfLastAccess.Value);
        }
        else
        {
            Application.OpenURL(AlfredUrl);
        }
    }
}
