using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookCommand : ICommand
{
    private CameraControlPlayer _cameraControl;
    private Vector2 _mouseDelta;

    public MouseLookCommand(CameraControlPlayer cameraControlPlayer, Vector2 mouseDelta) 
    {
        this._cameraControl = cameraControlPlayer;
        this._mouseDelta = mouseDelta;
    }

    public void Canceled()
    {
        _cameraControl.SetIsTakingInput(false);
    }

    public void Execute()
    {
        _cameraControl.SetMouseLook(_mouseDelta);
    }
}
