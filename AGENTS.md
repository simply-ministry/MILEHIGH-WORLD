# Agent Briefing: MILEHIGH-WORLD

This document provides instructions for agents (like Jules and Gemini CLI) working on the MILEHIGH-WORLD project.

## 🛠️ Build and Validation

Before submitting any code changes, agents must run the following validation script to ensure the project structure and data integrity are maintained:

```bash
python3 validate_implementation.py
```

## 🛡️ Security Compliance

All contributions must adhere to the standards defined in `GEMINI.md`, specifically:
- Implementation of the **Sentinel Protocol** for data ingestion.
- Adherence to the **Clean Logging Policy**.
- **Path Sanitization** for all dynamic file operations.

## 📦 Project Structure

- `Assets/Scripts/Core`: Core game management and logic.
- `Assets/Scripts/Data`: Data models and JSON campaign data.
- `Assets/Scripts/Editor`: Editor-only tools and importers.
- `Assets/Scripts/Characters`: AI and character behavior scripts.

## 🚀 Deployment Rules

- Do not modify files in the `Assets/StreamingAssets` directory directly unless instructed.
- Ensure all new ScriptableObjects are generated through the `CharacterFactory` or equivalent editor tools to maintain path sanitization.
