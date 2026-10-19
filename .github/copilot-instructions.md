# GitHub Copilot Instructions for PawnsChooseResearch (Continued)

## Mod Overview and Purpose

**PawnsChooseResearch (Continued)** is a RimWorld mod that streamlines the research process by allowing pawns to autonomously select and commit to research projects based on their skills, passions, and traits. This enhances gameplay by reducing the need for constant player intervention in research management, allowing colonies to thrive with minimal micromanagement. 

## Key Features and Systems

- **Auto-Assigned Research:**
  - Pawns automatically select research projects fitting their skills and interests.
  - Projects are categorized, and pawns prioritize core technologies.
  
- **Dynamic Research Preferences:**
  - Pawns avoid projects they are incapable of contributing to due to skill limitations or conflicting traits.
  - Research choices are influenced by pawns' traits—e.g., teetotalers avoid drug-related research.
  
- **Collaboration and Progression:**
  - Multiple pawns can work on different or the same projects, contributing to progression.
  - Uses the vanilla system to track collective research progress.
  
- **Customization Options:**
  - Options for players to regain research control temporarily or manage research preferences, such as disabling advanced projects for lower-skilled pawns.

- **Research Timeline Tracking:**
  - Allows players to track what research has been completed and by whom.

- **Compatibilities and Extensions:**
  - Integrates with many popular mods and extends functionality without disrupting the vanilla research system.

## Coding Patterns and Conventions

- **Consistent Naming:**
  - Variables and methods are named using `CamelCase` for easy readability.
  - Class names are descriptive of their function (e.g., `JobDriver_GetReport`, `SetResearch`).

- **Modular Design:**
  - Code is divided into logical units for different mod functionalities such as setting research (`SetResearch.cs`) and initializing the mod (`Startup.cs`).

- **Clear Documentation:**
  - Inline comments are used throughout the C# files to explain logic, especially in complex sections.
  - XML files are organized by feature-specific patches, facilitating easy adjustments and updates.

## XML Integration

- The mod utilizes 79 XML files to manage and integrate research categories, trait effects, and compatibility patches for other mods.
- XML patches are categorized by feature (e.g., `AA_Research.xml`, `BackupPower.xml`) allowing precise modification and enhancement of the vanilla game elements.

## Harmony Patching

- Utilizes the Harmony library by `brrainz` for runtime modification.
- Key methods in the game are patched using robust practices, ensuring minimal conflicts and high compatibility.
- Patching utilizes postfix methods to append functionality without altering the base game code directly.

## Suggestions for Copilot

- **Autocomplete and Suggestions:**
  - Copilot can assist in automatically generating harmonized patches to expand mod capabilities.
  - Provide suggestions on efficient patterns for XML patch creation to support new mods.

- **Debugging and Error Resolution:**
  - Use Copilot to help generate and integrate efficient logging mechanisms for error tracking.

- **Efficiency Improvements:**
  - Suggest potential optimizations or refactoring opportunities, such as consolidating redundant XML patches or streamlining C# logic flows.

- **Feature Expansion:**
  - Assist in creating templates for new mod integrations by mapping existing XML patches and identifying consistent patterns that can be reused or extended.
  
By following these guidelines and leveraging the powerful capabilities of GitHub Copilot, mod developers can enhance the PawnsChooseResearch mod efficiently while maintaining high standards of code quality and interoperability.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

