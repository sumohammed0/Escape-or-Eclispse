# Escape or Eclipse
*An immersive multiplayer escape room experience with time-based puzzles and dynamic lighting.*

## 👥 Team Members
- Safa Mohammed
- Bryan Choo
- Alireza Khatami

## 🎮 Key Features
| Avatars & Animation | 
| **Photon Networking** (Multiplayer) | 
| Escape Room Scene System | 
| Inventory System (Teleport Gun/Flashlight) | 
| Time-Based Lighting System | 

## 🕹️ Player Journey
1. **Opening Scene**  
   - View game controls in settings
2. **Lobby Scene**  
   - Create/join rooms via passcode
3. **Gameplay**  
   - Solve 4 interconnected puzzles:
     - Moon Phase Lock Puzzle
     - Light-Based Hourglass Puzzle
     - Celestial Stone Placement
   - Inventory management 
   - Time pressure: Lights dim progressively

## 🛠️ Technical Implementation
### Scene Architecture
1. `OpeningScene.unity`  
2. `LoadingScene.unity`  
3. `LobbyScene.unity`  
4. `Puzzle2Scene.unity`  

### Android Controls
| Input | Action |
|-------|--------|
| Joystick | Movement/Menu Navigation |
| X | Grab Objects/Select |
| Y | Teleport |
| B | Access Inventory |
| A | Drop Objects |
| Device Motion | View Rotation (Cardboard) |

### Multiplayer Setup
1. Both devices connected to WiFi
2. One player creates room with passcode
3. Second player joins same passcode

## 📱 Target Platform
**Android** (Optimized for Google Cardboard)

## 🔗 Links
- [YouTube Demo](INSERT_YOUTUBE_LINK_HERE)

## 🧩 Puzzle Flowchart
```mermaid
graph TD
    A[Start] --> B[Moon Phase Lock]
    B --> C[Glowing Orb Puzzle]
    C --> D[Teleport Upstairs]
    D --> E[Light/Hourglass Puzzle]
    E --> F[Sun/Moon Stone Placement]
    F --> G[Door Unlocks]
