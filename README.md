# Escape-or-Eclipse

## Team Members
- Safa Mohammed
- Alireza Khatami  
- Bryan Choo  


## Overview
"Escape or Eclipse" is a multiplayer VR escape room game designed for Android (with PC testing support). The player(s) explore, solve puzzles, and interact with their environment to escape before time runs out.

## Key Features
- Avatars and Animation *(planned)*
- Multiplayer Networking
- Immersive Escape Room Scenes
- Inventory System (Teleporting Gun & Flashlight)

## Player Walkthrough

1. **Opening Scene**  
   - Users can view game controls via the settings.

2. **Start Game**  
   - Pressing "Start Game" moves the player to a loading scene.

3. **Lobby Scene**  
   - Users create or join a room using an input field.  
   - Room creation allows another player (on a second Android device) to join.
![ReadMe Document2](https://github.com/user-attachments/assets/78f9f296-b92e-447e-bc45-822a5fdba503)

4. **Puzzle Floor 1**
   - Spawn near a corner desk.
   - Interact with drawers and objects.
   - Discover clues:
     - A locked drawer
     - A painting and a riddle
     - Numbered moon phases
   - Use clues to open the drawer.
   - Retrieve a glowing orb and place it on a stone dial.
   - **Puzzle 1 Solved:** Objects disappear; new inventory item notification.

5. **Puzzle Floor 2**
   - Use the teleport gun (press `M` to open inventory).
   - Solve a riddle on the wall.
   - Turn off the lights and use the flashlight to reveal symbols on an hourglass.
   - Use symbols to unlock the puzzle 2 lock, retrieving the **moonstone** and **sunstone**.

6. **Final Puzzle**
   - Place the moonstone and sunstone into their engravings.
   - The door unlocks — escape successful!

> ⚠️ Time-Limit Mechanic: The longer it takes to escape, the lower the ambient light gets. Failing to escape in time results in a black screen and game over.

## Scenes in Order
1. `OpeningScene.unity`  
2. `LoadingScene.unity`  
3. `LobbyScene.unity`  
4. `Puzzle2Scene.unity`

## Controls

### Android (via Gamepad)
- **X** — Interact / Select  
- **Y** — Teleport  
- **B** — Open Inventory  
- **A** — Drop Objects  

> Joystick used for player movement and UI interaction.  
> Camera view is controlled by rotating the device (or using Google Cardboard headset).

### PC (for testing)
Mapped to keyboard equivalents for ease of debugging and testing.

## Multiplayer Setup
- Use two Android devices on the same Wi-Fi network.
- Build and install the project on both.
- One user creates a room in the lobby using a passcode.
- The second user joins using the same code.
- 
![ReadMe Document](https://github.com/user-attachments/assets/b90e097c-7e0f-462d-a740-8edd26e957e0)

## Links
- 🔗 GitHub: [Escape-or-Eclispse](https://github.com/sumohammed0/Escape-or-Eclispse)  
- 📺 YouTube: 

---
