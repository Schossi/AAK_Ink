using AdventureCore;
using Ink.Runtime;
using UnityEngine;

public class InkAction : CharacterActionBase
{
    public TextAsset InkAsset;

    public override void OnStart(CharacterActorBase actor, bool jumpStart = false)
    {
        base.OnStart(actor, jumpStart);

        var story = new Story(InkAsset.text);

        DialogUI.Instance.Show(story, EndAction);
    }

    public override void OnInput(bool parameter)
    {
        base.OnInput(parameter);

        DialogUI.Instance.Continue();
    }
}
