using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHandler
{
    public static bool JumpPressed(int playerIndex)
    {
        string buttonName = "";
        string axisName = "";
        switch(playerIndex)
        {
            case 1:
                buttonName = "JumpP1";
                axisName = "Vertical1P1";
                break;
            case 2:
                buttonName = "JumpP2";
                axisName = "Vertical1P2";
                break;
        }
        return Input.GetButtonDown(buttonName) || Input.GetAxis(axisName) > 0.5f;
    }

    public static float HorizontalInput(int playerIndex)
    {
        string axisName = "";
        switch (playerIndex)
        {
            case 1:
                axisName = "Horizontal1P1";
                break;
            case 2:
                axisName = "Horizontal1P2";
                break;
        }
        return Input.GetAxis(axisName);
    }

    public static Vector2 AimingDirection(int playerIndex)
    {
        string horizontalAxisName = "";
        string verticalAxisName = "";
        string verticalKeyName = "";
        switch (playerIndex)
        {
            case 1:
                horizontalAxisName = "Horizontal2P1";
                verticalAxisName = "Vertical2P1";
                verticalKeyName = "VerticalKeysP1";
                break;
            case 2:
                horizontalAxisName = "Horizontal2P2";
                verticalAxisName = "Vertical2P2";
                verticalKeyName = "VerticalKeysP2";
                break;
        }
        Vector2 joystickAim = new Vector2(Input.GetAxis(horizontalAxisName), Input.GetAxis(verticalAxisName));

        if (joystickAim.magnitude > 0f)
            return joystickAim;
        else
            return new Vector2(Input.GetAxis(verticalKeyName), Mathf.Abs(Input.GetAxis(verticalKeyName) * 0.5f)).normalized;
    }
}
