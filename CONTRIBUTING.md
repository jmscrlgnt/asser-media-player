# Contributing

Contributions are welcome for learning and portfolio development.

1. Create a feature branch from `main`.
2. Keep UI logic in Forms, library/storage/playback logic in Services, persistent data in Models, and reusable file helpers in Utilities.
3. Avoid committing personal media, generated `bin/` or `obj/` folders, `.vs/`, or local state files.
4. Run `python tests/verify_structure.py` before opening a pull request.
5. Build the x64 solution in Visual Studio and manually test playback before merging playback-related changes.

When adding a feature, update the README or relevant document if the behavior is user-visible.
