EXTERNAL SetBool(name,value)
EXTERNAL SetInt(name,value)
EXTERNAL SetFloat(name,value)

EXTERNAL OnMessages(messages)

EXTERNAL SetCharacterBool(character,name,value)
EXTERNAL SetCharacterInt(character,name,value)
EXTERNAL SetCharacterFloat(character,name,value)

EXTERNAL OnCharacterMessages(character,messages)

InkAction also provides various external functions, this example will show some common ones directly related to the character
~SetBool("Shake",true)
The external function SetBool(name,value) calls the same method on the action which sets a bool parameter in the characters animator and makes is shake
~SetBool("Shake",false)
SetInt and SetFloat are also available
~SetCharacterBool(1,"Shake",true)
There are also variants like SetCharacterBool(int character,string name,bool value) that let you specify which character to use
~SetCharacterBool(1,"Shake",false)
OnMessages/OnCharacterMessages can be used to put messages into the characters messaging system(split by space)
~OnMessages("Ping")
Just like messages sent by, for example, animations these can be used for pretty much anything like for example to play sounds
