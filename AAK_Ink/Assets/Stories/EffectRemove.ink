EXTERNAL HasEffect(key)
EXTERNAL AddEffect(key)
EXTERNAL RemoveEffect(key)

It is possible to remove effects to a characters using RemoveEffect(key)
There are also variants for the effect functions that let you specify the character(for example HasCharacterEffect(character,key))
{ HasEffect("e"): -> remove | -> abort }

=== remove ===
~ RemoveEffect("e")
<>effect has been removed
->END

=== abort ===
effect was not active to begin with 
->END