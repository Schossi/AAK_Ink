using AdventureCore;

namespace Assets.Scripts
{
    public class InkActionProxy : CharacterActionBase
    {
        public InkAction InkAction;
        public CharacterBase[] Characters;
        public string Path;

        public override void StartAction(CharacterActorBase actor, bool jumpStart = false, bool force = false)
        {
            InkAction.StartActionProxied(actor, Characters, Path);
        }
    }
}
