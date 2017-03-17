using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHandler
{
    public static bool JumpPressed(int playerIndex)
    {
        string buttonName = "";
        switch(playerIndex)
        {
            case 1:
                buttonName = "JumpP1";
                break;
            case 2:
                buttonName = "JumpP2";
                break;
        }
        return Input.GetButtonDown(buttonName);
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
        switch (playerIndex)
        {
            case 1:
                horizontalAxisName = "Horizontal2P1";
                verticalAxisName = "Vertical2P1";
                break;
            case 2:
                horizontalAxisName = "Horizontal2P2";
                verticalAxisName = "Vertical2P2";
                break;
        }
        return new Vector2(Input.GetAxis(horizontalAxisName), Input.GetAxis(verticalAxisName));
    }
}
