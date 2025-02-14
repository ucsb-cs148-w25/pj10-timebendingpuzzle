Mukun Zhou
I used ChatGPT (GPT4o) for camera tracking logic in our Unity 2D game
### Outcomes Produced
- Generated C# script for camera following behavior
- Implemented screen-edge buffer zone detection
- Added smooth damping movement
Instead of fixing the center of the camera on the player, this script allows the camera to follow the player in a smart way.
The camera moves only if the player trespasses 1/4 screen length from the edge of the window. This makes the camera movement
smoother, improving gaming experience. 


**Tom Shangguan**
AI used: Chatgpt (Canvas), DALL-E 3
What I used for: For our game, I used ChatGPT’s Canvas feature to enhance the UI design and DALL-E 3 to generate concept art. My goal is 
to improve the current design of the main page UI. I started by uploading the UI design in MVP to ChatGPT Canvas, where I received AI-driven 
suggestions on layout improvements, button placements, and visual hierarchy. The AI provided insights on making the interface more intuitive 
and readable, which helped refine the player experience.I also used DALL-E 3 to generate a background picture. 


**Mike Petrus**
AI used: ChatGPT
Outcome: I used ChatGPT to help generate code for the visual oscillation on the player character while using the rewind feature. ChatGPT has been very useful for Unity development because, while many of the functions are fairly straightfoward, I have not used Unity prior to this project. This has helped me get more famililar with the specific classes used by Unity and how they are linked to various scripts and the graphical editor. When it came time to write the flashing effect I had the idea to adjust the sprite color in a loop. ChatGPT helped me generate a clever line of code using a sin function so that the color value oscillates between a specified range. It a gave me better insight into the functions that can be used to manipulate sprites and color. Testing was a fairly simple process of first making sure that the sprite color was changing as expected, and then applying the oscillation effect to the loop that controls player rewinding. Going forward I can carry this experience into some of our other sprites and effects. For example, having the player flash red when they take damage or having a different visual effect when objects move through time, rather than the player.
