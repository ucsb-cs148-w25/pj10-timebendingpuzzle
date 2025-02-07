Mukun Zhou
I used ChatGPT (GPT4o) for camera tracking logic in our Unity 2D game
### Outcomes Produced
- Generated C# script for camera following behavior
- Implemented screen-edge buffer zone detection
- Added smooth damping movement
Instead of fixing the center of the camera on the player, this script allows the camera to follow the player in a smart way.
The camera moves only if the player trespasses 1/4 screen length from the edge of the window. This makes the camera movement
smoother, improving gaming experience. 

  
