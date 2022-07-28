# __AAK__ integration for the __ink__ narrative scripting language

- [Action Adventure Kit](https://adventure.softleitner.com/)  
- [Ink](https://www.inklestudios.com/ink/)

## Setup

The packages are not included in this repository and have to be downloaded separately so after cloning the project will be missing the Ink and SoftLeitner folders.  
![project structure](https://github.com/Schossi/AAK_Ink/blob/main/Project.PNG)  
You only need the AdventureCore folder from AAK for this integration demo. Download it from the [asset store](https://assetstore.unity.com/packages/templates/systems/action-adventure-kit-217284) or copy it from another project you have previously used it in if you want to avoid the errors when opening a project that is missing a lot of its files. 

The unity integration for ink can also be found on the [asset store](https://assetstore.unity.com/packages/tools/integration/ink-unity-integration-60055) and downloaded directly or as a UPM package as described on [github](https://raw.github.com/inkle/ink-unity-integration)

Once you have downloaded the two dependencies you start to try out the integration. Open and play the AAK_Ink scene which contains examples for a lot of the functionality. The stories can be found in the Stories subfolder which can be edited using the awesome [Inky](https://github.com/inkle/inky/releases/tag/0.13.0). The AAK_Ink_Shared scene contains an example for a use case where we want multiple different actions to use the same story and state.

## Tags

InkAction currently handles two tags in its *storyContinued* method. This method is overridable which any derived action can take advantage of to implement additional tags.

- character:N  
This tag sets which character is currently active in the dialog. N is the character index which is 0 for the executing character(player) and 1-... for the characters set in the Characters field of the action(NPCs). Changing the current character is handled in *setCurrentCharacter*. Currently all it does is align the characters so all of them look at the current one, you can override the method to add additional behavior like for example a portrait.
- trigger:ParameterName  
Sets a trigger on the animator of the currently active character.

## Functions

All the different functions are bound in *createStory* which can be overridden if additional functions need to be added in derived actions.

### General
- __SetBool__(name,value), __SetInt__(name,value), __SetFloat__(name,value)  
__SetCharacterBool__(character,name,value), __SetCharacterInt__(character,name,value), __SetCharacterFloat__(character,name,value)  
Set parameters of different types in character animators, the variations with character in their name let you specify which character by adding a character index as the first parameter.(used in the functions example to make a characters shake)
- __OnMessages__(messages) / __OnCharacterMessages__(character,messages)  
Will publish messages to a character, multiple messages can be split with space.(used in the functions example to publish a "Ping" message which, using a message event on the player, plays a sound)

### Persistence
- __GetPersistedBool__(key), __GetPersistedInt__(key), __GetPersistedFloat__(key), __GetPersistedString__(key)  
Can be used to retrieve a value from the persister set in the FunctionPersister field of the InkAction.
- __SetPersistedBool__(key,value), __SetPersistedInt__(key,value), __SetPersistedFloat__(key,value), __GetPersistedString__(key,value)  
Can be used to set a value in the persister set in the FunctionPersister field of the InkAction. Both Set and Set functions are demonstrated in the Persistence example.

### Items
- __GetItemQuantity__(key) /  __GetCharacterItemQuantity__(int,key)  
These return how many of the item of the specified key the character currently has in its inventory.
- __CanAddItems__(key,quantity), __CanRemoveItems__(key, quantity)  
__CanCharacterAddItems__(character,key,quantity), __CanCharacterRemoveItems__(character,key,quantity)  
These check if items can be either added to or removed from a characters inventory. For example nothing can be removed from an empty inventory and nothing can be added to a full one.
- __AddItems__(key,quantity), __RemoveItems__(key, quantity)  
__AddCharacterItems__(character,key,quantity), __RemoveCharacterItems__(character,key,quantity)  
Will add or remove items from the characters inventors. The key has to match the one of the items in the item set on the InkAction Items field.

### Effects
- __HasEffect__(key) /  __HasCharacterEffect__(int,key)  
These check if an effect is currently active in a characters effect pool.
- __AddEffect__(key), __RemoveEffect__(key)  
__AddCharacterEffect__(character,key), __RemoveCharacterEffect__(character,key)  
Will add or remove an effect from the characters effect pool. The key has to match the one of the effects in the effect set on the InkAction Effects field.
