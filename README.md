# Smut Engine
Alternative name: SonaSim

Simple little project I made to practice c#. Before this I have never touched it.
This is a visual novel engine that uses specially formatted files to create interactive experinces you can play in your command line

## How to use
For Readers: Start the program, provide the file and go!

For Authors: Read on.

# File structure

### Keyword Cheatsheet
| Keyword | Action or Use |
|---|---|
| !!&&CLEAR&&!! | When placed at beginning of line, clears console output |
| [{characterCode}] | When placed at beginning of line, denotes that character's dialogue |
| [n] | When placed at beginning of line, denotes a narrator line |
| [n:{characterCode}] | When placed at beginning of line, denotes a narrator line WITH a character's pronouns to use within that line |
| [{number}] | When placed at the end of a line or end of a choice, denotes the next line to read after that line completes.<br>Please note that these numbers are the INDEXES of the lines and starts from zero.<br>The number you should put here is `desiredLine - 1` |
| ;= | Denotes beginning of a choice option |
| ;; | Denotes end of choice option |

### End of file line
The last line of your file should be structured like this (all on one line!):

`***{k:KIRA:MAGENTA:she:her:hers}{g:GRIMLEY:DARKRED:he:him:his}{z:ZACH:BLUE:he:him:his}
?NAME:y/n,CPRONOUN1:p1,CPRONOUN2:p2,CPRONOUN3:p3,PPRONOUN1:#$1,PPRONOUN2:#$2,PPRONOUN3:#$3,TITLE:README.md,AUTHOR:Zemchar?***`

#### Explanation

* Each character should have their information contained within a brace like this:
    - `{[character denoter that is placed at beginning of line]:[Name of character]:[ConsoleColor value for character's dialouge]:[pronoun1]:[pronoun2]:[pronoun3]}`
* Information about other parts of the file should be placed within two `?`'s 
    - `NAME` value is the string in the file that will be substituted for the player's name
    - `CPRONOUN[1-3]` string that the program will replace for the character's programs in the story
    - `PPRONOUN[1-3]` string that will be replaced with the character's pronouns in the story.
        - Make sure this value has no overlapping patterns between the CPRONOUN values and itself
    - `TITLE` the title of the story
    - `AUTHOR` the author of the story
* All of the above information should be placed within two `***` marks on the very last line of the file.
* Choices can be represented like `;=choice1[nextLineIfPicked];;;=choice2 [nextLineIfPicked];;`
    - The square brackets within each choice represent the next line to read if that choice is picked
    - The response to choose a choice is handled internally. No need to put any identifiers

# Examples

### This file:
[testdoc.txt](https://github.com/Zemchar/SmutEngine/files/8828036/testdoc.txt)

### Yeilds:
![2022-06-02 18-26-58](https://user-images.githubusercontent.com/48448818/171755215-ada866c8-1af3-4d04-8ff7-48a7966f6a15.gif)

