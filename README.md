# Iks Oks | Noughts & Crosses

A local multiplayer Tic-Tac-Toe game built in Unity. Designed for two players on a single mobile device, supporting both Portrait and Landscape orientations.

---

## Getting Started

### Requirements
- Unity 6.3 LTS or newer
- Android build module installed

### Running the Project
1. Clone the repository
2. Open the project in Unity
3. Open `PlayScene` from `Assets/Scenes`
4. Press Play in the Editor.

### Build
- Set target platform to **Android**
- Ensure `PlayScene` is index 0 and `GameScene` is index 1 in Build Settings
- Build and run
- (Optional) To run without building download the .apk from Releases.

---

## Gameplay

Classic tic-tac-toe gameplay. The first player to connect three markers in a row, either horizontally, vertically, or diagonally, wins. If the board is filled without a winner, the match ends in a draw.

---

## Architecture

The project was implemented to be easy for asset changes via ScriptableObjects & scalable with few to no code adjustments.

### Scene Structure
| Scene | Main elements |
|---|---|
| `PlayScene` | Main menu - Theme selection pop-up, Game Statistics pop-up, Settings pop-up, Exit pop-up |
| `GameScene` | Gameplay - Game Board, GUI/UI, Game over pop-up, Settings pop-up |

### Manager Architecture
There are two distinct Manager categories to avoid stale singleton references across scene loads:

**Persistent Managers** (`PersistentManagers`) - survive scene transitions.

**Scene-Specific Managers** (`GameManagers`) - created on every GameScene load, destroyed when returning to PlayScene:

This separation helps prevent stale event subscribers or instance duplications when transitioning between scenes.

### Basic Game Loop
Once the match begins when both players alternate turns tapping empty cells to place their marker. After each placement, GameManager checks for a win or draw via IGameRules. Match time and player move count is displayed on the HUD.
A win triggers the strike line animation across the winning cells and opens the Game Over pop-up. A draw opens the pop-up without a strike animation and different text. 
From the GameOver pop-up, players can retry the match or return to the main menu. Match duration and result are recorded to persistent storage at the end of every match.

Rather than direct references there is extensive use of events. An example of this in the core loop is `GameManager` broadcasts:
- `OnTurnChanged` - fires when the active player changes.
- `OnPlayerWin` - fires with the winning `PlayerIndex`.
- `OnDraw` - fires when the board is full with no winner.
- `OnWinLineFound` - fires with winning cell indices and direction for the strike animation.

### Scalable Game Rules - `IGameRules`
Win result detection is abstracted behind an `IGameRules` interface implemented by the `StandardGameRules` ScriptableObject. `GameManager` delegates all rule logic to this object, meaning it has zero knowledge of how wins are detected.

To create a new ruleset e.g. 4×4 board, create a new `ScriptableObjects/GameRules` implementing `IGameRules` and assign it to the `GameManager`.

### Theme System
Visual themes are split into two ScriptableObjects:

- **`ThemeData`** - player-specific: marker sprite and player colour. Each player selects their own `ThemeData` independently before the match starts via the `ThemeSelectionPopup`
- **`BoardTheme`** - designer-set: board background sprite, board colour, cell sprite, cell colour. Configured once by the designer and are not player-selectable.

Adding a new theme requires only creating a new `ThemeData` asset - no code changes.

### Audio
`AudioManager` uses a single `AudioConfig` ScriptableObject to centralise all clip assignments. Background music (BGM) loops seamlessly across scene transitions. SFX and BGM are independently settable in the Settings popups and persist via PlayerPrefs.

### Localisation
Full localisation support via Lean.Localisation. Currently supports **English** and **Serbian** (Latin script only). The `LanguageManager` singleton wraps Lean, abstracting the localisation library from all other systems - swapping libraries requires changes only to `LanguageManager`.
Additional languages are addable via using a text file or csv for the language and only requires editing the `Lean.Localisation` component on `LanguageManager`.

### Pop-up menus
All pop-ups inherit from abstract `PopupBase` which shows/hides all popups. The animation is currently coroutine-based but would be ideally replaced with an existing library like Lean Transition or custom inhouse animations for greater designer control.

### Statistics
Match results are serialised as a `StatsData` object to JSON and stored via PlayerPrefs. Adding a new statistic requires only adding a field to `StatsData`. A more secure storage for player data could be implemented in this wrapper without affecting other code.

### Editor Tools
- **`MenuItems`** - adds a **Tools -> ClearPlayerPrefs** menu item for resetting saved stats during development.
- **`LanguageEditor`** - adds a **Tools -> LocalisationEditor** menu item to open a editor tool to add new translations for the game. If the language doesn't have support a new .txt folder is created otherwise the phrase is appended to the end of the .txt folder in the format: KeyName,"Hello World"

---

## Third Party Tools

| Tool | Usage |
|---|---|
| **Lean GUI** | LeanToggle for Settings toggles |
| **Lean Localisation** | String localisation system |
| **Lean Orientation** | Portrait and Landscape orientation management |
| **TextMeshPro** | All UI text rendering |

---

## Known Limitations & Future Improvements
- **Dependency Injection** - Given the scope of the project no dependency injection was implemented in favour of singletons, however this means there is still some coupling of code. Ideally all singletons would be replaced with a service e.g. `IAudioService` to be called instead of `AudioManager.Instance?.SomeMethod`.
- **World** - The game was implemented entirely on the `Canvas` and is therefore 100% UI-based. This eased development especially regarding orientation management however for a real-world game it would be more ideal to have implemented the game elements in world space and to have some state machine to reset the `Camera` position to accomodate the change in orientation.
- **Pop-up animations** are currently coroutine-based. Replacing with Lean Transition would give designers direct control over timing and curves.
- **Strike line visual impact** - The strike line could be further enhanced with a colour tint and glow effect matching the marker colour of the winning player and maybe even additional particle effects.
- **New Games** - `IGameRules` and the existing turn system are designed to accommodate new game types e.g. an AI player without changes to `GameManager`.
- **Board size** - `StandardGameRules` accepts any board size via its ScriptableObject. A 4×4 or 5×5 variant requires only a new asset and UI layout adjustment.
- **Progression** - currently statistics are just that, statistics. Player win counts could be used to unlock new themes, all themes are currently available but with more assets they could be locked behind win counters.

---

## Attribution

- App icons by [cah nggunung](https://www.flaticon.com/free-icons/three-in-a-row) via Flaticon
- Code conventions followed: https://stackoverflow.com/a/310967

---
