using AdventureCore;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// character action that redirects to an InkAction<br/>
    /// it sets the characters of that action and moves the story to a certain point<br/>
    /// this enables using one Ink Story for multiple actions with a shared state
    /// </summary>
    public class InkActionProxy : CharacterActionBase
    {
        [Tooltip("the ink action that holds the story, when the proxy is started it starts this action instead and moves it to a specified knot")]
        public InkAction InkAction;
        [Tooltip("the characters involved in this action, overrides the InkAction Characters when the proxy is started")]
        public CharacterBase[] Characters;
        [Tooltip("the knot the main ink story is moved to when the proxy is started")]
        public string Path;

        public override bool StartAction(CharacterActorBase actor, bool jumpStart = false, bool force = false)
        {
            InkAction.StartActionProxied(actor, Characters, Path);
            return true;
        }
    }
}
