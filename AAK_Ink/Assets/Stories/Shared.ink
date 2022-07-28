VAR choice = ""

->Info

=== Info ===
This scene demonstrates using a single story for multiple actions.
InkActionProxy is used to redirect to one global InkAction.
The Path field specifies where the proxy starts inside the shared story.
->DONE

=== Franz ===
#character:0
{ choice == "": Go talk to Fritz! | Fritz told me you like {choice}. }
->DONE

=== Fritz ===
#character:0
{ choice != "": You've made your choice! -> DONE}

Which colour do you like?
    + [Red]
    ~choice="Red"
    Good choice, I like Red too!
    ->DONE
    + [Blue]
    ~choice="Blue"
    I see, so you like Blue...
    ->DONE