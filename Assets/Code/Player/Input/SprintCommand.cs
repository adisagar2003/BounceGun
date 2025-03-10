using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintCommand : ICommand
{
    private PlayerMovement _playerMovement;
    
    public SprintCommand(PlayerMovement playerMovement)
    {
        this._playerMovement = playerMovement;
    }
    public void Canceled()
    {
        _playerMovement.SetIsSprintPressed(false);
    }

    public void Execute()
    {
        _playerMovement.SetIsSprintPressed(true);
    }

}
