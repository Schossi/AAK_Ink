EXTERNAL GetPersistedInt(name)
EXTERNAL SetPersistedInt(name,value)

The external functions GetPersistedBool(name) and SetPersistedBool(name,value) can be used to interact with the persistence system.
This is useful to share values between different stories or to change the rest of the game depending on choices made in Ink.
There are also variants of the method for Int, Float and String values.

VAR choice = 0
~ choice = GetPersistedInt("choice")
{ choice == 0: -> getChoice | -> showChoice }

=== getChoice ===
For example this choice will be persisted to a value that is observed by a PersistenceTrigger?
    + [1]
        -> chosen(1)
    + [2]
        -> chosen(2)
    
=== chosen(num) ===
~ choice = num
~ SetPersistedInt("choice",num)
You chose {choice}!
-> END

=== showChoice ===
You chose {choice}!
-> END
