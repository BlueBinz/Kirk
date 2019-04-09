using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            player.ObjectsTouchingFeet.Add(collision);
        }
        catch (Exception e) { }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        player.ObjectsTouchingFeet.Remove(collision);
    }
}
