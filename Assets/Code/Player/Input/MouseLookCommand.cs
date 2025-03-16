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
        if (_cameraControl == null) return;
        _cameraControl.SetIsTakingInput(false);
    }

    public void Execute()
    {
        if (_cameraControl == null) return;
        if (!_cameraControl)
        {
            
            return;
        }
        _cameraControl.SetMouseLook(_mouseDelta);   
    }
}
