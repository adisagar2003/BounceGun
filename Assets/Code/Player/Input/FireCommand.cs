using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCommand : ICommand
{
    private IThrowable _throwable;
    private PlayerGun _playerGun;

    public FireCommand(IThrowable throwable, PlayerGun playerGun)
    {
        _throwable = throwable;
        _playerGun = playerGun;
    }

    public void Canceled()
    {
    }

    public void Execute()
    {
        _playerGun.Shoot();
    }
}
