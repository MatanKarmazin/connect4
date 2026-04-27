# Connect 4 – C# Windows GUI Application

## Overview
This project is a **Connect 4 game implemented in C#**, originally developed as part of an academic course assignment. 

The application has been successfully upgraded from its initial console-based iteration to a full **Windows GUI application**. A key design goal of the original project was a **clear separation between game logic and user interface**. Because the core logic was written to be UI-agnostic, the transition to a graphical environment was seamless and did not require rewriting the underlying game mechanics.

---

## Technology Stack
- **Language:** C# 2.0  
- **Framework:** .NET Framework 4.0  
- **Application Type:** Windows GUI Application (WinForms)  
- **Architecture:** Logic–UI separation (Layered design)

---

## Project Structure
- **Game Logic Layer** Implements the rules, board state, win conditions, and turn management.  
  This layer remains entirely UI-agnostic and acts as the robust engine behind the game.

- **UI Layer (Graphical)** Responsible for all visual rendering and user interactions:
  - Capturing mouse clicks for coin placement
  - Rendering the visual game board and coin colors
  - Displaying pop-up dialogs for game over/win states and turn indicators

---

## Design Rationale (Course Context)
The project was intentionally written with **separation of concerns** in mind from day one. 

> The initial implementation strictly separated the game logic from the console interface.  
> This architectural foresight allowed the logic layer to be perfectly reused for this new Windows-based graphical environment.

This design aligns with software engineering best practices taught in the course and proves the value of maintaining decoupled application layers.

---

## How to Run
1. Open the solution file (`Connect4.slnx` ) in **Visual Studio**.  
2. Ensure **.NET Framework 4.0** is selected.
3. Set `Connect4` as the startup project.
4. Build the solution.
5. Run the project from Visual Studio or execute the compiled binary.

---

## Future Improvements
- Single-player mode against an AI opponent
- Network multiplayer support (LAN/Online)
- Visual animations for coin dropping and winning combinations
- Responsive UI resizing
- Refactoring to modern C# versions (e.g., .NET 8+)
- Unit tests for game logic

---

## Notes
This project reflects the constraints and requirements of the academic environment in which it was developed. The codebase stands as a testament to how good initial architectural planning supports future software evolution.

---

## Authors
**Matan Karmazin & Lior Zvieli**