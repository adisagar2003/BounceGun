using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand: ICommand
{
    private PlayerMovement _playerMovement;
    private Vector2 _keyboardInput;
    
    public  MoveCommand(PlayerMovement playerMovement, Vector2 KeyboardInput)
    {
        this._playerMovement = playerMovement;
        this._keyboardInput = KeyboardInput;
    }

    public void Execute()
    {
        _playerMovement.GetInput(_keyboardInput);
        _playerMovement.SetIsMovePressed(true);
    }

    public void Canceled()
    {
        _playerMovement.GetInput(_keyboardInput);
        _playerMovement.SetIsMovePressed(false);
    }
}
