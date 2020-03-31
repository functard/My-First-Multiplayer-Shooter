using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerName : MonoBehaviour
{
    [SerializeField]
    private InputField nameField;

    [SerializeField]
    private Button setNameButton;

    public void OnNameFieldChange()
    {
        // if user name has more then 3 charecters less and less then 8
        if(nameField.text.Length > 2 && nameField.text.Length < 8)
        {
            //can press button
            setNameButton.interactable = true;
        }
        else
            setNameButton.interactable = false;



    }
    public void OnClick_SetName()
    {
        //set name
        PhotonNetwork.NickName = nameField.text;
    }
}
