EXTERNAL GetItemQuantity(key)

EXTERNAL CanAddItems(key,quantity)
EXTERNAL CanRemoveItems(key,quantity)

EXTERNAL AddItems(key,quantity)
EXTERNAL RemoveItems(key,quantity)

It is possible to add items to a characters inventory using AddItems(key,quantity)
It might be wise to check CanAddItems(key,quantity) first if the inventory has limited space for example
{ CanAddItems("i",1): -> add | -> abort }

=== add ===
~ AddItems("i",1)
<>added 1 item to inventory
->finish

=== abort ===
can't add any more!
->finish

=== finish ===
the character currently has {GetItemQuantity("i")} items
->END