## 2024-05-24 - Avoid GameObject.Find in Scene Initialization Loops
**Learning:** Using `GameObject.Find` inside loops or scene initialization sequences (like in `SceneDirector.cs`) causes expensive O(N) scene hierarchy traversals, leading to severe performance bottlenecks during level load or scenario updates.
**Action:** Always use dictionary caches (e.g., `Dictionary<string, GameObject>`) to store and retrieve object references instead of relying on `GameObject.Find` inside loops.
