using AdventureCore;
using Ink.Runtime;
using System.Linq;
using UnityEngine;

public class InkAction : CharacterActionBase
{
    public enum InkEndingMode { None = 0, Reset = 5, ResetKnot = 10 }

    public TextAsset InkAsset;
    public CharacterBase[] Characters;
    public PersisterBase StatePersister;
    public PersisterBase FunctionPersister;
    [Header("Ending")]
    public InkEndingMode EndingMode = InkEndingMode.Reset;
    public string EndingModeParameter;
    [Header("Sets")]
    public ItemSet Items;
    public EffectSet Effects;

    public CharacterBase CurrentCharacter { get; private set; }

    private Story _story;
    private Vector3[] _memorizedAlignments;

    private void Start()
    {
        _story = createStory();

        if (StatePersister && StatePersister.Check())
            _story.state.LoadJson(StatePersister.Get<string>());
    }

    public void StartActionProxied(CharacterActorBase actor, CharacterBase[] characters, string path)
    {
        Characters = characters;
        if (_story == null)
            _story = createStory();
        _story.ChoosePathString(path);
        StartAction(actor);
    }

    public override bool CanStart(CharacterActorBase actor)
    {
        if (_story != null && !_story.canContinue)
            return false;

        return base.CanStart(actor);
    }

    public override void OnStart(CharacterActorBase actor, bool jumpStart = false)
    {
        base.OnStart(actor, jumpStart);

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

    protected virtual Story createStory()
    {
        var story = new Story(InkAsset.text);

        //GENERAL

        story.BindExternalFunction("SetBool", (string name, bool value) => setBool(Animator.StringToHash(name), value));
        story.BindExternalFunction("SetInt", (string name, int value) => setInt(Animator.StringToHash(name), value));
        story.BindExternalFunction("SetFloat", (string name, float value) => setFloat(Animator.StringToHash(name), value));

        story.BindExternalFunction("OnMessages", (string messages) => Actor.Character.OnMessages(messages));

        //GENERAL BY CHAR

        story.BindExternalFunction("SetCharacterBool", (int character, string name, bool value) => getCharacter(character).SetBool(Animator.StringToHash(name), value));
        story.BindExternalFunction("SetCharacterInt", (int character, string name, int value) => getCharacter(character).SetInt(Animator.StringToHash(name), value));
        story.BindExternalFunction("SetCharacterFloat", (int character, string name, float value) => getCharacter(character).SetFloat(Animator.StringToHash(name), value));

        story.BindExternalFunction("OnCharacterMessages", (int character, string messages) => getCharacter(character).OnMessages(messages));

        //PERSISTENCE

        story.BindExternalFunction("GetPersistedBool", (string key) => FunctionPersister.Get<bool>(key));
        story.BindExternalFunction("SetPersistedBool", (string key, bool value) => FunctionPersister.Set(value, key));

        story.BindExternalFunction("GetPersistedInt", (string key) => FunctionPersister.Get<int>(key));
        story.BindExternalFunction("SetPersistedInt", (string key, int value) => FunctionPersister.Set(value, key));

        story.BindExternalFunction("GetPersistedFloat", (string key) => FunctionPersister.Get<float>(key));
        story.BindExternalFunction("SetPersistedFloat", (string key, float value) => FunctionPersister.Set(value, key));

        story.BindExternalFunction("GetPersistedString", (string key) => FunctionPersister.Get<string>(key));
        story.BindExternalFunction("SetPersistedString", (string key, string value) => FunctionPersister.Set(value, key));

        //ITEMS

        story.BindExternalFunction("GetItemQuantity", (string key) => Actor.Character.InventoryBase.GetQuantity(Items.GetItem(key)));

        story.BindExternalFunction("CanAddItems", (string key, int quantity) => Actor.Character.InventoryBase.CanAddItems(new ItemQuantity(Items.GetItem(key), quantity)));
        story.BindExternalFunction("CanRemoveItems", (string key, int quantity) => Actor.Character.InventoryBase.CanRemoveItems(new ItemQuantity(Items.GetItem(key), quantity)));

        story.BindExternalFunction("AddItems", (string key, int quantity) => Actor.Character.InventoryBase.AddItems(new ItemQuantity(Items.GetItem(key), quantity))?.Quantity ?? 0);
        story.BindExternalFunction("RemoveItems", (string key, int quantity) => Actor.Character.InventoryBase.RemoveItems(new ItemQuantity(Items.GetItem(key), quantity))?.Quantity ?? 0);

        //ITEMS BY CHAR

        story.BindExternalFunction("GetCharacterItemQuantity", (int character, string key) => getCharacter(character).InventoryBase.GetQuantity(Items.GetItem(key)));

        story.BindExternalFunction("CanCharacterAddItems", (int character, string key, int quantity) => getCharacter(character).InventoryBase.CanAddItems(new ItemQuantity(Items.GetItem(key), quantity)));
        story.BindExternalFunction("CanCharacterRemoveItems", (int character, string key, int quantity) => getCharacter(character).InventoryBase.CanRemoveItems(new ItemQuantity(Items.GetItem(key), quantity)));

        story.BindExternalFunction("AddCharacterItems", (int character, string key, int quantity) => getCharacter(character).InventoryBase.AddItems(new ItemQuantity(Items.GetItem(key), quantity))?.Quantity ?? 0);
        story.BindExternalFunction("RemoveCharacterItems", (int character, string key, int quantity) => getCharacter(character).InventoryBase.RemoveItems(new ItemQuantity(Items.GetItem(key), quantity))?.Quantity ?? 0);

        //EFFECTS

        story.BindExternalFunction("HasEffect", (string key) => Actor.Character.EffectPool.HasEffect(Effects.GetEffect(key)));

        story.BindExternalFunction("AddEffect", (string key) => { Actor.Character.EffectPool.Add(Effects.GetEffect(key)); });
        story.BindExternalFunction("RemoveEffect", (string key) => { Actor.Character.EffectPool.Remove(Effects.GetEffect(key)); });

        //EFFECTS BY CHAR

        story.BindExternalFunction("HasCharacterEffect", (int character, string key) => getCharacter(character).EffectPool.HasEffect(Effects.GetEffect(key)));

        story.BindExternalFunction("AddCharacterEffect", (int character, string key) => { getCharacter(character).EffectPool.Add(Effects.GetEffect(key)); });
        story.BindExternalFunction("RemoveCharacterEffect", (int character, string key) => { getCharacter(character).EffectPool.Remove(Effects.GetEffect(key)); });

        return story;
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
