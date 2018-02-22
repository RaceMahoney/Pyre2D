# Pyre2D
A two-dimensinal platfromer in Unity 2017 to be used as a testing environment for my senior research project. The project aims to determine if using capture replay and image difference can accurately detect software bugs for games in regression testing. 

![alt tag](/Docs/images/Capture1.png)

## Implementation 
For my experiment, I created this game taking heavy inspiration from my favorite indie game and Kickstarter project, "Shovel Knight". For the purpose of testing I made the first level an almost exact replica of the first stage in "Shovel Knight". The unnamed protagonist is able to jump, dash and attack with a sword of fire. While playing, all inputs and important values will be recorded to a file to be used later for replay. Along with the input recording, periodically throughout the game, screenshots will be automatically taken and send to Google's Firebase cloud database. 

With all this data collected, the recorded input can be read back into the game and an exact replicate of the play session can be made and screenshots will be taken once again to be analyzed for differences in order to detect software bugs that manifest during the play session. This will allow for automated regression testing of a game before a patch or update is released for an already released game.  


