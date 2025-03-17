using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCommand : ICommand
{
    private IThrowable _throwable;

    public FireCommand(IThrowable throwable)
    {
        _throwable = throwable;
    }
    public void Canceled()
    {
        // futuer implementation
    }

    public void Execute()
    {
        _throwable?.Throw();       
    }
}
