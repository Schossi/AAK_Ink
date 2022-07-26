EXTERNAL GetItemQuantity(key)

EXTERNAL CanAddItems(key,quantity)
EXTERNAL CanRemoveItems(key,quantity)

EXTERNAL AddItems(key,quantity)
EXTERNAL RemoveItems(key,quantity)

Removing items works just like adding them, the functions are called <color=blue>CanRemoveItems(key,quantity)</color> and <color=blue>RemoveItems(key,quantity)</color>
It is also possible to check how many items a character currently has using <color=blue>GetItemQuantity(key)</color>
All the item functions also come with a variant that lets you specify the character by passing its index as the first parameter

{ CanRemoveItems("i",1): -> remove | -> abort }

=== remove ===
~RemoveItems("i",1)
<>removed 1 item to inventory
->finish

=== abort ===
can't remove any more!
->finish

=== finish ===
the character currently has {GetItemQuantity("i")} items
->END