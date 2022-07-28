using AdventureCore;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// minimal demo character
/// </summary>
public class PlayerCharacter : CharacterBaseTyped<MinimalCharacterActor, CharacterControllerMovement, ListedInventory>
{
    [Header("Interaction")]
    public CharacterActionArea InteractionArea;
    public GameObject InteractionUI;
    public TMPro.TMP_Text InteractionText;

    protected override void Start()
    {
        InteractionArea.Changed.AddListener(new UnityAction(interactionChanged));
        interactionChanged();
    }
    
    public void OnConfirm(InputValue _)
    {
        if (!Actor.IsActing && InteractionArea.CanStart)
            InteractionArea.StartAction();
        else
            Actor.OnInput(true);
    }
    public void OnCancel(InputValue _)
    {
        Actor.OnInput(false);
    }

    private void interactionChanged()
    {
        InteractionUI.SetActive(InteractionArea.CanStart);
        InteractionText.text = InteractionArea.Text;
    }
}
