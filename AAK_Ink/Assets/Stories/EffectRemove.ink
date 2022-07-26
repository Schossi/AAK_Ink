EXTERNAL HasEffect(key)
EXTERNAL AddEffect(key)
EXTERNAL RemoveEffect(key)

It is possible to remove effects to a characters using <color=blue>RemoveEffect(key)</color>
There are also variants for the effect functions that let you specify the character(for example <color=blue>HasCharacterEffect(character,key)</color>)
{ HasEffect("e"): -> remove | -> abort }

=== remove ===
~ RemoveEffect("e")
<>effect has been removed
->END

=== abort ===
effect was not active to begin with 
->END