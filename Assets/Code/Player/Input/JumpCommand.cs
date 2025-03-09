using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : ICommand
{
    private PlayerMovement _playerMovement;
    
    public JumpCommand(PlayerMovement playerMovement)
    {
        this._playerMovement = playerMovement;
    }
    public void Canceled()
    {
        _playerMovement.JumpCancelled();
    }

    public void Execute()
    {
        _playerMovement.JumpPressed();
    }
}
