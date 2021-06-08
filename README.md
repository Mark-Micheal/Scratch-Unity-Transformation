# Scratch-Unity-Transformation
A Unity Project that takes a pseudocode generated from Scratch project and execute it in Unity.
## MiniProject 1 (MP1) link
https://docs.google.com/document/d/1aafmrA98h12HkP8Nk-SCun2ipajcGY0GRxTgwjJef5s/edit#heading=h.2j1ngfn5ir9z

## Process
* Scratch program was implemented and downloaded.
* The json file extracted from the scratch project was passed to the python program done on MP1.
* The pseudocode output is generated according to the instructions in MP1.
* C# was used to parse the pseudocode and execute the program using Unity 3D engine .

## How to run
1. Download the zip from the release and extract
2. Write the desired scratch psuedocode (according to MP1 readme) in the `code.txt` file.
3. Run the .exe file.
4. The Flag button is present on the top-left corner of the screen.
5. The input psuedocode is displayed n the top-right corner.
6. The current coordinates of scratch (the cat) is displayed at the bottom.
7. The program can be started by either clicking on the flag button (`When Flag` command) or by pressing a keyboard button (`When KP {button}` command).

# Note
* To exit the program, force exit using ALT+F4.
* There is a limit to what scratch can move around the screen, specifically between [-7,7] in the x-dimension and [-4,4] in the y-dimension, so please take that into consideration when running a custom program.
