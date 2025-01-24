using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : Singleton<CommandManager>
{
    private readonly Dictionary<string ,Type> _command = new();
    public void Register(string name ,Type commmand)
    {
        if(_command.ContainsKey(name))
        {
            return;
        }
        _command[name] = commmand;
    }
    public void Unregister(string name )
    {
        _command.Remove(name);
    }
    public void Excute(string name , object data = null)
    {
        Command command  =(Command)Activator.CreateInstance(_command[name]);
        command.Data = data;
        command.Execute();
    }
}
