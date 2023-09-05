using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManagerLib : MonoBehaviour
{
    public static EventsManagerLib Instance = null;

    private void Awake()
    {
        // Debug.Log("GAME eventMANAGER Awake");
        //  FindObjectOfType<cursorkiller>().ShouldIkillCursor();
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    #region Display_Text_Broadcast
    public delegate void EVENT_TextDisplay(string argstr0, string argstr1);
    public static event EVENT_TextDisplay On_TextDisplay;
    public static void CALL_DisplayStrings(string argstr0, string argstr1)
    {
        if (On_TextDisplay != null) On_TextDisplay(argstr0, argstr1);
    }
    #endregion
}
