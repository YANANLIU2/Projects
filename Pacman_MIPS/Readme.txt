The project is created to replicate pacman game in assembly language using mips mars simulator. * Implemented a basic game loop that can run continuously and listen for exit commands ('q').
* Added rendering for the maze by drawing individual tiles. 
* Added incremental rendering for the player.
* Implemented player keyboard input handling
* Ensured proper collision detection
* Added teleportation tunnel so the player can travel from one end to another end


Setup steps
1. Open in Mars MIPS
2. Connect with "Bitmap Display" and set config to:
 - Unit width in pixels: 1					     
 - Unit height in pixels: 1
 - Display width in pixels: 512
 - Display height in pixels: 512
 - Base Address for Display: 0x10040000 ($gp)
3. Connect with "Keyboard and Display MMIO Simulator"
4. Run