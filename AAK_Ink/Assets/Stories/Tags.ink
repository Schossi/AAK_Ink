This example demonstrates some common ways for characters to interact with ink scripts using tags
For example aligning characters to the one that is currently speaking #character:0
The <color=red>character:n</color> tag is meant to set which character is currently active in the narrative
Index 0 is always the character executing the action(player) followed by the character set in the InkAction.Characters field(NPCs)#character:1
Alignment is just the current default implementation for setCurrentCharacter in InkAction
This method is virtual and can easily by overriden to change the behaviour(for example character portraits)#character:2
The <color=red>trigger:xxxx</color> tag sets a trigger on the animator of the current character or the executing one if there is none#character:0 #trigger:Surprise