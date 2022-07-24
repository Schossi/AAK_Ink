using AdventureCore;
using Ink.Runtime;
using UnityEngine;

public class InkAction : CharacterActionBase
{
    public TextAsset InkAsset;
    public PersisterBase StatePersister;
    public string ResetKnot;

    private Story _story;

    public override bool CanStart(CharacterActorBase actor)
    {
        if (_story != null && !_story.canContinue)
            return false;

        return base.CanStart(actor);
    }

    public override void OnStart(CharacterActorBase actor, bool jumpStart = false)
    {
        base.OnStart(actor, jumpStart);

        if (_story == null)
        {
            _story = new Story(InkAsset.text);

            if (StatePersister && StatePersister.Check())
                _story.state.LoadJson(StatePersister.Get<string>());
        }

        DialogUI.Instance.Show(_story, EndAction);
    }

    public override void OnEnd(CharacterActionBase next)
    {
        base.OnEnd(next);

        DialogUI.Instance.Hide();

        if (!_story.canContinue && !string.IsNullOrWhiteSpace(ResetKnot))
            _story.ChoosePathString(ResetKnot);

        if (StatePersister)
            StatePersister.Set(_story.state.ToJson());
    }

    public override void OnInput(bool parameter)
    {
        base.OnInput(parameter);

        DialogUI.Instance.Continue();
    }
}
