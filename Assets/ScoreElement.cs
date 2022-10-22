using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{

    public TMP_Text usernameText;
    public TMP_Text startText;
    public TMP_Text endText;
    public TMP_Text phoneText;

    public void NewScoreElement (string _username, string _start, string _phone, string _end)
    {
        usernameText.text = _username;
        startText.text = _start.ToString();
        endText.text = _end.ToString();
        phoneText.text = _phone.ToString();
    }

}