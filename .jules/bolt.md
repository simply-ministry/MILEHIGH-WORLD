## 2024-05-23 - GameObject.Find caching in iterative setups
**Learning:** Using `GameObject.Find` in setup loops (like iterating over character profiles and interactive objects) can cause noticeable hitches during scene initialization, particularly in complex scenes. It is extremely expensive as it iterates through the whole scene hierarchy.
**Action:** Always use dictionary-caching or direct references for multiple lookups by name instead of repeated `GameObject.Find` calls to speed up scene initialization by making repeated lookups O(1) instead of O(N).
