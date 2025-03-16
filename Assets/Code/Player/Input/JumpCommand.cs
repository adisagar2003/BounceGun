using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : ICommand
{
    private PlayerMovement _playerMovement;
    
    public JumpCommand(PlayerMovement playerMovement)
    {
        if (_playerMovement == null) return;
        this._playerMovement = playerMovement;
    }
    public void Canceled()
    {
        if (_playerMovement == null) return;
        _playerMovement.JumpCancelled();
    }

    public void Execute()
    {
        if (_playerMovement == null) return;
        _playerMovement.JumpPressed();
    }
}
