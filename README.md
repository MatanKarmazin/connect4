# Connect 4 – C# Console Application

## Overview
This project is a **Connect 4 game implemented in C#**, developed as part of an academic course assignment.  
The application is built as a **console-based game** using **C# 2.0** and **.NET Framework 4.0**.

A key design goal of this project is **clear separation between game logic and user interface**, allowing the core logic to be reused and upgraded in the future — specifically for a **Windows-based GUI environment**.

---

## Technology Stack
- **Language:** C# 2.0  
- **Framework:** .NET Framework 4.0  
- **Application Type:** Console Application  
- **Architecture:** Logic–UI separation (Layered design)

---

## Project Structure
- **Game Logic Layer**  
  Implements the rules, board state, win conditions, and turn management.  
  This layer is UI-agnostic and designed to be reusable.

- **UI Layer (Console)**  
  Responsible only for input/output operations:
  - Reading player input
  - Displaying the game board
  - Showing messages and game state

This separation enables future migration to a graphical interface without rewriting the game logic.

---

## Design Rationale (Course Context)
The project was intentionally written with **separation of concerns** in mind:

> The implementation separates the game logic from the user interface.  
> This design allows the logic layer to be reused and extended later,  
> with the long-term goal of upgrading the game to a Windows-based environment.

This architectural decision was made to align with software engineering best practices taught in the course.

---

## How to Run
1. Open the solution file (`.sln`) in **Visual Studio**  
2. Ensure **.NET Framework 4.0** is selected
3. Build the solution
4. Run the project from Visual Studio or execute the compiled binary

---

## Future Improvements
- Windows GUI implementation (WinForms / WPF)
- Improved input validation
- Enhanced game flow and UX
- Refactoring to modern C# versions
- Unit tests for game logic

---

## Notes
This project reflects the constraints and requirements of the academic environment in which it was developed.  
The codebase was intentionally designed to support future evolution beyond a console application.

---

## Author
**Matan Karmazin & Lior Zvieli**
