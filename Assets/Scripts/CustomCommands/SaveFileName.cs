using Naninovel;
using System.Threading;

using UnityEngine;

[CommandAlias("savefilename")]
public class SaveFileName : Command
{
    public StringParameter Name;
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        Engine.GetService<IStateManager>().SaveGameAsync(Name).Forget();
        return UniTask.CompletedTask;
    }

}

