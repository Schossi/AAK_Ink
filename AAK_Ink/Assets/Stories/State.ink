this example demonstrates the use of InkAction in a scenario similar to souls games
whenever used the action shows another chunk of dialog until it eventually runs out and keeps repeating the same one #EndAction
The different chunks of dialog are seperated by the EndAction tag #EndAction
How far the dialog has progressed is persisted by the persister set in the StatePersister field #EndAction
->END

===ENDED===
this line will be shown if the action is started after the story has ended because the knot has been set as the ResetKnot on the InkAction
it could contain something like 'I have nothing more to say to you!' or 'Goodbye!'
if the ResetKnot is empty the action can no longer be started if the story has ended
->DONE