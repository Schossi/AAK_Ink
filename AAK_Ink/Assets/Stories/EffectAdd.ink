EXTERNAL HasEffect(key)
EXTERNAL AddEffect(key)
EXTERNAL RemoveEffect(key)

It is possible to add effects to a characters using AddEffect(key)
Whether the character already has the effect can be checked using HasEffect(key)
{ HasEffect("e"): -> abort | -> add }

=== add ===
~ AddEffect("e")
<>effect has been added
->END

=== abort ===
effect is already active
->END