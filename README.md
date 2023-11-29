For mastermind, I was way more focued on the structure of the code. Which I feel I managed and it payed of. It was way easier to navigate the code, even if I hadn't touched it in some time.

I felt most of this task was quite simple and relatively easy to do. However, for some reason, I could never get the colors to work. I came close once when I figured out that the ANSI codes I wanted to use ranged from 0-9. With the input being 0-9, I figured I could use that to determine the color output. Until I realized it would not solve the issue. The issue being that in the evalutaion, 1 == green e.g.. 

Another thing I struggled with was the output window in general. Having it display the amount of tries until the amount was reached, while also being the same output was something I did not manage to do. I seems like such a simple task, but I could never figure it out. My thought process was to have one array that shows the attemps, and then replacing array[0] by the attempt. Then increase it until the whole array is filled with attemps.


You must use the code from [https://github.com/CodeCraftCurriculum-I/module_6_mastermind](https://github.com/CodeCraftCurriculum-I/module_6_mastermind)

- Create a different animation for the splash screen, this includes creating a new "graphics" etc.
# DONE - Fix it so that the menu is always below the "logo".  
# DONE - The rules of mastermind state that the players should:
 ## - (A) Agree on the number of attempts (but max 10)
 ## - (B) Agree if it is allowed with duplicates.
- Add the code for the game to have these options (A and B). (Duplicates are the use of the same colour more than once.)    
  ## - (A) Added code for
  (B) - Duplicates are added but not color output
# DONE - Currently, the player can input a sequence shorter or longer than needed. Only permit inputs that are the correct length.
- The current output is not pretty and uses numbers instead of colours. Change the code, so the board looks like the following image (as close as possible). The evaluation is on the left, and the guesses are on the right. Note that we are showing the whole board, including the attempts that have not been used yet (all white colours). The code already contains code that shows how to use colours.
    ![description of what the game should look like.](./brett_1.png)
## DONE - Add a Legend after the game board. A legend explains characters, symbols, or markings that may be unfamiliar to the reader.
- Find some way to have the player select colours, not just type numbers (at the moment, the players do not know what the numbers symbolize in colors).
## DONE - Add a summary after the game, including information about whether the player won or lost, how well they did, etc. Then return to the menu.

### Challenge Requirements (Higher Grades)

## - Let the player decide the number of elements in the hidden sequence (Currently it is 4)
## -  Let the player decide the number of colors that can be used for the sequence.
- Implement for decided amount of colors in the sequecne
- ... Come up with something interesting.

In your `readme.md` file write some reflections about your code.

**Your submission must be in the form of a Zip file with a sensible internal structure.**

## About assessment

Assessment for this project will be based not just on feature completion, but also on your attention to detail, the cleanliness and readability of your code, and the thoughtfulness of your README file reflections. This is your opportunity to demonstrate not just what you've learned, but how you can apply it creatively and effectively in a real programming project.

---
