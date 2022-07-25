using AdventureCore;
using Ink.Runtime;
using System.Linq;
using UnityEngine;

public class InkAction : CharacterActionBase
{
    public enum InkEndingMode { None, Reset, ResetKnot }

    public TextAsset InkAsset;
    public CharacterBase[] Characters;
    public PersisterBase StatePersister;
    [Header("Ending")]
    public InkEndingMode EndingMode;
    public string EndingModeParameter;

    public CharacterBase CurrentCharacter { get; private set; }

    private Story _story;
    private Vector3[] _memorizedAlignments;

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

            _story.BindExternalFunction("SetBool", (string name, bool value) => setBool(Animator.StringToHash(name), value));
            _story.BindExternalFunction("SetInt", (string name, int value) => setInt(Animator.StringToHash(name), value));
            _story.BindExternalFunction("SetFloat", (string name, float value) => setFloat(Animator.StringToHash(name), value));

            _story.BindExternalFunction("OnMessages", (string messages) => Actor.Character.OnMessages(messages));

            _story.BindExternalFunction("SetCharacterBool", (int character, string name, bool value) => getCharacter(character).SetBool(Animator.StringToHash(name), value));
            _story.BindExternalFunction("SetCharacterInt", (int character, string name, int value) => getCharacter(character).SetInt(Animator.StringToHash(name), value));
            _story.BindExternalFunction("SetCharacterFloat", (int character, string name, float value) => getCharacter(character).SetFloat(Animator.StringToHash(name), value));

            _story.BindExternalFunction("OnCharacterMessages", (int character, string messages) => getCharacter(character).OnMessages(messages));

            if (StatePersister && StatePersister.Check())
                _story.state.LoadJson(StatePersister.Get<string>());
        }

        DialogUI.Instance.Show(_story, storyContinued, EndAction);
    }

    public override void OnEnd(CharacterActionBase next)
    {
        base.OnEnd(next);

        restoreAlignments();

        DialogUI.Instance.Hide();

        if (!_story.canContinue)
        {
            switch (EndingMode)
            {
                case InkEndingMode.Reset:
                    _story.ResetState();
                    break;
                case InkEndingMode.ResetKnot:
                    _story.ChoosePathString(EndingModeParameter);
                    break;
            }
        }

        if (StatePersister)
            StatePersister.Set(_story.state.ToJson());
    }

    public override void OnInput(bool parameter)
    {
        base.OnInput(parameter);

        DialogUI.Instance.Continue();
    }

    protected virtual void setCurrentCharacter(CharacterBase current)
    {
        CurrentCharacter = current;

        memorizeAlignments();

        foreach (var character in Characters)
        {
            if (character == current)
                align(character, Actor.Character);
            else
                align(character, current);
        }

        if (Actor.Character == current)
            align(Actor.Character, Characters.FirstOrDefault());
        else
            align(Actor.Character, current);
    }

    protected virtual void storyContinued()
    {
        foreach (var tag in _story.currentTags)
        {
            if (tag.Contains(":"))
            {
                var split = tag.Split(':');
                var key = split[0];
                var value = split[1];

                switch (key)
                {
                    case "character":
                        setCurrentCharacter(getCharacter(int.Parse(value)));
                        break;
                    case "trigger":
                        (CurrentCharacter ?? Actor.Character).SetTrigger(Animator.StringToHash(value));
                        break;
                }
            }
        }
    }

    protected void memorizeAlignments()
    {
        if (_memorizedAlignments != null)
            return;
        _memorizedAlignments = new Vector3[Characters.Length];
        for (int i = 0; i < Characters.Length; i++)
        {
            _memorizedAlignments[i] = Characters[i].MovementBase.Forward;
        }
    }
    protected void restoreAlignments()
    {
        if (_memorizedAlignments == null)
            return;
        for (int i = 0; i < Characters.Length; i++)
        {
            Characters[i].MovementBase.Align(_memorizedAlignments[i]);
        }
        _memorizedAlignments = null;
    }

    protected void align(CharacterBase character, CharacterBase target)
    {
        if (character == null || target == null)
            return;
        character.MovementBase.Align(target.Position - character.Position);
    }

    protected CharacterBase getCharacter(int i)
    {
        if (i == 0)
            return Actor.Character;
        return Characters.ElementAtOrDefault(i - 1);
    }
}
