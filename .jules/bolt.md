
## 2024-03-20 - GameObject.Find caching during scene initialization
**Learning:** The SceneDirector repeatedly used the highly expensive `GameObject.Find` to resolve objects dynamically based on campaign JSON data during scene setup. For scenarios with multiple interactive objects or characters, this O(N) hierarchy traversal scales poorly.
**Action:** Implemented a simple dictionary cache (`_objectCache`) to memoize object lookups within `SceneDirector`, falling back to `GameObject.Find` only when necessary and caching newly instantiated objects. Always cache `GameObject.Find` results if they might be queried multiple times in a setup loop.
