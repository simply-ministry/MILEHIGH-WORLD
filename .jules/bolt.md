## 2024-05-26 - [SceneDirector Caching Optimization]
**Learning:** [SceneDirector.cs was performing expensive O(N) `GameObject.Find()` calls and linear prefab searches inside character instantiation loops. Unity's native object destruction creates "fake nulls" (C# reference is not null, but native object is destroyed), which complicates dictionary-based caching.]
**Action:** [Implemented a multi-level caching system in `SceneDirector.cs`. 1. `_objectCache` for GameObjects with negative caching and `ReferenceEquals` checks for fake nulls. 2. `_prefabCache` to avoid repeated linear searches on the prefab list. 3. `_controllerCache` using `GetInstanceID()` to avoid redundant `GetComponent` calls. All caches are cleared during scenario setup to prevent memory leaks and stale references.]
## 2024-05-24 - Unity GameObject.Find Performance Bottleneck
**Learning:** In Unity 3D scripts like `SceneDirector.cs` within this project, methods frequently call `GameObject.Find()` to search for characters or interactive objects by name during scene setups. This forces Unity to traverse the entire scene hierarchy repeatedly (O(N) operation), which can cause noticeable load times or frame drops if called continuously or on large scenes.
**Action:** Replace direct `GameObject.Find()` calls with a `Dictionary<string, GameObject>` cache when objects are fetched by name multiple times or instantiated dynamically. Check `cachedObj != null` to handle Unity's custom object destruction logic safely before returning a cached reference.
## 2024-05-24 - Unity GameObject.Find Bottleneck
**Learning:** `GameObject.Find` is heavily used in setup scripts like `SceneDirector.cs` which can cause significant frame drops during scenario initialization or interaction processing. Unity's overridden `!= null` check works properly for detecting destroyed objects in a Dictionary cache.
**Action:** Always consider replacing `GameObject.Find` inside loops or repetitive functions with a cached Dictionary approach in Unity C# scripts.
## 2024-05-23 - GameObject.Find caching in iterative setups
**Learning:** Using `GameObject.Find` in setup loops (like iterating over character profiles and interactive objects) can cause noticeable hitches during scene initialization, particularly in complex scenes. It is extremely expensive as it iterates through the whole scene hierarchy.
**Action:** Always use dictionary-caching or direct references for multiple lookups by name instead of repeated `GameObject.Find` calls to speed up scene initialization by making repeated lookups O(1) instead of O(N).
## 2024-03-16 - Unity Coroutine Yield Instruction Caching
**Learning:** Found that using inline `new WaitForSeconds(time)` repeatedly inside Unity coroutines (especially in heavily choreographed cinematic sequences) generates unnecessary Garbage Collection allocations every time the coroutine hits that yield instruction, which can lead to GC spikes and frame stuttering.
**Action:** Always cache frequently used yield instructions like `WaitForSeconds` in a shared static dictionary or similar caching mechanism, so that existing yield objects are reused and no extra garbage is created per sequence execution.
## 2024-05-24 - Avoid O(n) GameObject.Find in Initialization/Setup Loops
**Learning:** Unity's `GameObject.Find()` operation is O(n) and deeply iterates the scene. Repeated calls for the same objects, especially when setting up many characters or interactive objects from JSON configuration loops, leads to significant and unnecessary latency. This pattern is common in dynamic data-driven scene instantiators like `SceneDirector`.
**Action:** When dynamically instantiating or initializing a scene from a list of identifiers, cache `GameObject.Find()` results and instantiated references into a `Dictionary<string, GameObject>`. This converts repetitive O(n) string-based scene lookups into an O(1) dictionary retrieval.
## 2024-05-24 - Unity GameObject.Find Performance Bottleneck in Setup Loops
**Learning:** `GameObject.Find()` in Unity executes an O(N) full scene graph traversal across all active objects. In data-driven setup loops (e.g., parsing JSON to build a scene or instantiating/finding characters and interactive objects in `SceneDirector.cs`), using it repeatedly causes significant CPU spikes on initialization.
**Action:** Always cache `GameObject.Find()` results in a generic `Dictionary<string, GameObject>` specifically for repeating setups, and check the cache before issuing the underlying `GameObject.Find()` call.

## 2024-03-20 - GameObject.Find caching during scene initialization
**Learning:** The SceneDirector repeatedly used the highly expensive `GameObject.Find` to resolve objects dynamically based on campaign JSON data during scene setup. For scenarios with multiple interactive objects or characters, this O(N) hierarchy traversal scales poorly.
**Action:** Implemented a simple dictionary cache (`_objectCache`) to memoize object lookups within `SceneDirector`, falling back to `GameObject.Find` only when necessary and caching newly instantiated objects. Always cache `GameObject.Find` results if they might be queried multiple times in a setup loop.
## 2026-03-20 - [GameObject.Find() Cache Optimization]
**Learning:** [SceneDirector.cs was doing expensive `GameObject.Find()` calls in a loop when spawning characters or applying interactions, which scans the entire scene tree. Unity overrides the == operator for GameObjects, making cached null checks a safe and effective way to see if an object was destroyed.]
**Action:** [Use a `Dictionary<string, GameObject>` to cache GameObjects found or created, substituting repeated `GameObject.Find()` calls with dictionary lookups for faster scene setups.]
## 2024-05-24 - O(N*M) lookup bottleneck in SetupScene
**Learning:** SceneDirector.cs used multiple GameObject.Find() calls inside iterative loops across characters and interactions. Since GameObject.Find() traverses the entire scene hierarchy, nesting this in a loop causes significant setup spikes.
**Action:** Use a Dictionary to cache GameObject lookups. When instantiating new objects dynamically, immediately add them to the dictionary cache to avoid secondary lookups.
## 2025-03-15 - [Coroutines in Unity Cinematics]
**Learning:** Unity coroutine `new WaitForSeconds()` creates a new object instance every time it's called, causing GC pressure during long sequences.
**Action:** Cache the yield instructions or use constant values in long coroutines/cinematics to avoid allocating memory for short delays repeatedly.
## 2024-05-24 - Avoid GameObject.Find in Scene Initialization Loops
**Learning:** Using `GameObject.Find` inside loops or scene initialization sequences (like in `SceneDirector.cs`) causes expensive O(N) scene hierarchy traversals, leading to severe performance bottlenecks during level load or scenario updates.
**Action:** Always use dictionary caches (e.g., `Dictionary<string, GameObject>`) to store and retrieve object references instead of relying on `GameObject.Find` inside loops.
## 2024-05-24 - Expensive `GameObject.Find` in Setup Loops
**Learning:** `GameObject.Find` is an O(N) operation that iterates over the entire scene hierarchy. When executed inside setup loops (e.g. `SceneDirector.cs` reading from JSON configurations), it creates a severe performance bottleneck during scene initialization and updates. Furthermore, caching GameObjects via `Dictionary<string, GameObject>` requires checking `obj != null` on retrieval, as Unity's overloaded equality operator natively checks if the object was destroyed in the C++ layer, even if the managed C# reference still exists.
**Action:** Replace `GameObject.Find` inside loops with O(1) Dictionary lookups (`Dictionary<string, GameObject>`). Always check `obj != null` when retrieving cached objects to avoid NullReferenceExceptions on natively destroyed instances.

## 2024-05-15 - Shader 'pow()' optimization
**Learning:** Using `pow(x, c)` for small constant integer exponents in pixel-evaluated fragment shaders can be highly unoptimized depending on the graphics API.
**Action:** Always rewrite integer-based exponentiation as explicit variable multiplications. For example, use `x * x * x` instead of `pow(x, 3.0)` to save ALU cycles and improve GPU fragment processing times.
## 2024-05-24 - Expensive Scene Traversal in Initialization Loops
**Learning:** `GameObject.Find` causes O(N) scene hierarchy traversals, which becomes a severe bottleneck when called repeatedly inside initialization loops (e.g., `SceneDirector.SetupScene` iterating over character profiles and interactive objects).
**Action:** Always use a dictionary cache (`Dictionary<string, GameObject>`) to store and retrieve object references during scene initialization sequences, ensuring to check `obj != null` on retrieval to handle natively destroyed objects correctly.
## 2024-05-24 - Expensive GameObject.Find in Loops
**Learning:** `GameObject.Find` is an O(N) operation traversing the scene hierarchy and is especially expensive when called repeatedly inside loops or initialization sequences like `SceneDirector.SetupScene`.
**Action:** Always use dictionary caches (e.g., `Dictionary<string, GameObject>`) to store and retrieve object references to avoid repeated scene traversals.
## 2024-05-25 - Unity WaitForSeconds GC Allocation in Loops
**Learning:** Using `new WaitForSeconds()` inside a tight loop (like a typewriter text effect in a Coroutine) creates a new object allocation on the heap for every single character typed. This causes unnecessary garbage collection (GC) pressure and can lead to frame stuttering during text-heavy cinematic sequences.
**Action:** Always cache `WaitForSeconds` objects outside the loop or use a shared caching mechanism when the duration is constant, returning the cached instance inside the loop.

## 2026-03-25 - [Redundant Member Clutter Performance Impact]
**Learning:** The 'SceneDirector.cs' file was severely cluttered with over a dozen redundant dictionary declarations and duplicate helper methods for GameObject caching. This not only increases memory overhead but also creates a "state fragmentation" risk where different parts of the initialization loop use different caches, leading to redundant O(N) traversals despite the caching intent.
**Action:** Always audit caching implementations for redundancy. Consolidate into a single, unified caching pattern to ensure O(1) lookups are consistent across the entire system.
## 2024-05-25 - Robust Negative Caching for Unity Lookups
**Learning:** In Unity, simply caching the result of 'GameObject.Find' is insufficient if the object is missing. Without storing the 'null' result (Negative Caching), the system continues to perform O(N) scene traversals for every query of the missing object. Additionally, 'ReferenceEquals(obj, null)' is the only reliable way to check if a cached reference is a 'Real Null' (never existed) vs a 'Fake Null' (Unity object was destroyed).
**Action:** Implement negative caching by storing 'null' results in lookups. Use 'ReferenceEquals' to distinguish cache hits for missing objects from invalidated references of destroyed objects.

## 2024-05-25 - Fragment Shader Dead Work Elimination
**Learning:** In HLSL fragment shaders, assigning to 'o.Albedo' at the end of a function overwrites all previous additive operations (like Subsurface Scattering) applied to that member. This creates "dead work" where expensive GPU calculations are performed but then discarded.
**Action:** Always assign the base albedo at the start of the 'surf' function. This ensures subsequent additive or multiplicative effects are correctly layered on the final output and prevents cycles from being wasted on discarded results.
## 2024-05-27 - Caching O(N) list searches with lazy evaluation and negative caching
**Learning:** In Unity setup scripts (e.g., `SceneDirector.cs`), using an O(N) list search with string comparisons (like `List.Find(p => p.name.Contains(searchString))`) inside initialization loops creates a significant performance bottleneck. Naively precaching the list using `prefab.name` won't work correctly if partial matching (e.g. `Contains`) is required by the logic.
**Action:** Replace the loop-based list search with a dictionary cache (`Dictionary<string, GameObject>`). Use lazy evaluation: perform the expensive O(N) list search ONLY on a cache miss, and store the result mapped to the original `searchString` key. Importantly, also cache `null` results (negative caching) to prevent repeated expensive searches for assets that do not exist.
## 2024-05-24 - Unity Coroutine Yield Instruction Caching with Floats
**Learning:** Found that using a `Dictionary<float, WaitForSeconds>` to cache Unity yield instructions is an anti-pattern. Floating-point precision inaccuracies can cause cache misses, leading to redundant Garbage Collection (GC) allocations.
**Action:** Always use an integer key (e.g., milliseconds calculated via `Mathf.RoundToInt(time * 1000f)`) when caching time-based objects like `WaitForSeconds` in Unity to ensure accurate cache hits and eliminate GC overhead.
## 2024-05-24 - Unity WaitForSeconds float key dictionary cache anti-pattern
**Learning:** Using `Dictionary<float, WaitForSeconds>` to cache yield instructions causes cache misses due to floating-point precision inaccuracies, leading to the creation of unintended garbage allocations (GC pressure) during high frequency calls like cinematic loops or typewriters.
**Action:** Use an integer key (e.g. milliseconds `Dictionary<int, WaitForSeconds>`) to correctly cache float-based values safely, avoiding precision-related cache misses while still eliminating GC allocation within loops.
## 2024-05-25 - Float-keyed Dictionary Cache Misses for WaitForSeconds
**Learning:** In `Cinematic_IntoTheVoid.cs`, using a `Dictionary<float, WaitForSeconds>` for caching yield instructions resulted in cache misses due to floating-point precision inaccuracies. While intended to prevent GC allocations, the float keys caused redundant instantiation of `WaitForSeconds` objects.
**Action:** Always use an integer-keyed dictionary (e.g., converting seconds to milliseconds via `Mathf.RoundToInt(time * 1000f)`) when caching temporal objects like `WaitForSeconds` to ensure reliable cache hits and truly eliminate GC allocations.

## 2026-04-02 - Float Keys in Caches Cause Misses
**Learning:** Using floats for Dictionary keys (e.g., `Dictionary<float, WaitForSeconds>`) causes cache misses due to floating point precision issues, leading to the GC allocations the cache is meant to avoid.
**Action:** When caching objects using float parameters, multiply by a suitable constant and round to an integer (like milliseconds for time) to use as the Dictionary key.
## 2024-05-26 - WaitForSeconds Dictionary Cache Float Precision Issue
**Learning:** Using a float as a Dictionary key for caching `WaitForSeconds` objects (e.g., `Dictionary<float, WaitForSeconds>`) can lead to cache misses due to floating-point precision inaccuracies, unintentionally creating new GC allocations.
**Action:** Always use an integer-keyed dictionary (e.g., representing milliseconds by multiplying the float time by 1000 and rounding) to safely and accurately cache float-based values without precision issues.

## 2026-03-26 - Cache List Searches inside Instantiation Loops
**Learning:** In Unity, using O(N) list searches with string comparisons (e.g., `List.Find(p => p.name.Contains(...))`) inside instantiation or scene setup loops causes significant performance bottlenecks due to unnecessary iterations.
**Action:** Always pre-cache the list items into a `Dictionary<string, GameObject>` to achieve O(1) lookups and prevent performance bottlenecks during repetitive lookups in loops.
## 2024-05-26 - Unity WaitForSeconds Float Cache Misses
**Learning:** Using a `Dictionary<float, WaitForSeconds>` to cache coroutine yield instructions is an anti-pattern due to floating-point precision inaccuracies. Even identical float literal arguments can yield tiny precision differences, resulting in cache misses and continuous heap allocations (GC spikes) that the cache was explicitly designed to prevent.
**Action:** Always key floating-point delay caches using integers (e.g., convert `float` seconds to `int` milliseconds using `Mathf.RoundToInt(time * 1000f)`). This ensures robust cache hits and zero GC allocations for repeated coroutine delays.

## 2024-06-12 - List.Find with string allocation in instantiation loop
**Learning:** Using `List.Find` combined with a string operation (like `p.name.Contains`) inside a scene initialization or instantiation loop scales at O(N) and creates repeated overhead per instantiated entity.
**Action:** Cache prefab lookups into a `Dictionary<string, GameObject>` keyed by the search name to convert sequential iterations into an O(1) hash map lookup, preventing performance hitches during dynamic scene setups.
## 2026-04-02 - Unity Negative Caching and ReferenceEquals
**Learning:** Unity's 'GameObject.Find' is expensive, especially when it fails. Implementing negative caching by storing 'null' in the dictionary prevents repeated O(N) scene traversals for non-existent objects. Additionally, using 'System.Object.ReferenceEquals(obj, null)' alongside the '== null' check is critical for robustly identifying truly null entries versus destroyed Unity objects (fake nulls).
**Action:** Use negative caching in GameObject caches and combine '== null' with 'ReferenceEquals' to safely manage the lifecycle of cached Unity objects.

## 2026-03-27 - [Triple-Layer Caching & Unity Null Safety]
**Learning:** Effective scene initialization in Unity requires a triple-layer caching strategy: 1) Negative caching for `GameObject.Find` to avoid repeated O(N) traversals for missing objects, 2) Prefab memoization to replace O(M) list searches, and 3) `InstanceID`-based component caching to optimize `GetComponent` calls. Crucially, robust negative caching MUST distinguish between "real nulls" (negative cache hits) and "fake nulls" (destroyed objects) using `System.Object.ReferenceEquals(obj, null)`.
**Action:** Implement unified `_objectCache`, `_prefabCache`, and `_controllerCache` patterns. Clear scene-specific caches (`_objectCache`, `_controllerCache`) during scenario transitions while persisting asset-based caches (`_prefabCache`). Always use `ReferenceEquals` for deep null-safety in Unity dictionaries.
## 2026-03-30 - Unity WaitForSeconds Anti-pattern
**Learning:** Caching `WaitForSeconds` with a Dictionary using float keys is an anti-pattern due to floating-point precision issues.
**Action:** Pre-calculate `WaitForSeconds` outside of loops to eliminate GC allocations without float dictionary keys.

## 2024-05-25 - Unity WaitForSeconds Float Dictionary Cache Anti-Pattern
**Learning:** Using `Dictionary<float, WaitForSeconds>` to cache yield instructions is an anti-pattern due to floating-point precision inaccuracies, leading to cache misses and subtle memory leaks/GC pressure.
**Action:** Instead of dictionary caching for `WaitForSeconds`, instantiate and cache specific `WaitForSeconds` objects as local variables outside of loops or as fields if the durations are static, avoiding dictionary lookups with float keys.
## 2026-03-26 - [Unity Component Caching and Lifecycle Management]
**Learning:** While caching `GetComponent` calls into a `Dictionary<int, T>` using `GetInstanceID()` as a key is a powerful O(1) optimization, it introduces a memory leak risk if not cleared during scene or scenario transitions. Unity's "fake null" objects (destroyed on the native side but alive in managed memory) persist in dictionaries, leading to redundant processing and memory bloat.
**Action:** Always clear component and object caches during scenario setups or scene transitions. In `SceneDirector.cs`, calling `_controllerCache.Clear()` ensures that only active objects are tracked, preventing leaks of destroyed character controllers.
## 2024-05-25 - Dictionary<float, WaitForSeconds> Anti-Pattern
**Learning:** Using `Dictionary<float, WaitForSeconds>` to cache yield instructions is an anti-pattern due to floating-point precision inaccuracies, which can cause cache misses and still result in per-iteration GC allocations.
**Action:** When variable wait times are needed in a tight loop (e.g., rhythmic punctuation delays in a typewriter effect), pre-calculate and cache the specific `WaitForSeconds` instances in local variables outside the loop instead of using a float-keyed dictionary.
## 2024-05-24 - Dead Code in Shaders
**Learning:** In 'HyperPBRCharacter_4D.shader', a Subsurface Scattering (SSS) block was performing expensive texture lookups and mathematical operations only for the result to be immediately overwritten by a subsequent albedo assignment. This "dead code" wastes GPU cycles and memory bandwidth without contributing to the final frame.
**Action:** Always audit surface shaders for assignment overwrites. Removing high-cost logic that is logically unreachable or overwritten is a high-impact optimization for rendering performance.
## 2024-05-25 - Unity WaitForSeconds GC Allocation in Loops vs Static Cache
**Learning:** Using `new WaitForSeconds()` inside a tight loop (like a typewriter text effect in a Coroutine) creates a new object allocation on the heap for every single character typed, causing unnecessary GC pressure. However, using a `Dictionary<float, WaitForSeconds>` for caching is an anti-pattern due to floating-point precision inaccuracies, and it risks a static memory leak if typing speeds vary continuously.
**Action:** Remove static `Dictionary<float, WaitForSeconds>` caches. Always safely cache `WaitForSeconds` objects in a local variable *outside* the loop (e.g., `var wait = new WaitForSeconds(time);`) instead of relying on unverified custom static dictionary caching methods.
## 2026-03-28 - Unity Component and Prefab Caching in SceneDirector
**Learning:** Even with GameObject caching, O(N) operations like `GetComponent` and O(P) operations like `List.Find` (especially with string operations like `Contains`) inside character setup loops create measurable initialization spikes.
**Action:** Use `Dictionary<int, T>` (using `GetInstanceID`) for component caching and `Dictionary<string, GameObject>` for prefab lookups to ensure all repetitive setup operations are O(1).

## 2026-03-28 - Performance vs. Correctness in Shader Logic
**Learning:** Fixing a "dead code" bug (e.g., an albedo overwrite that discarded an expensive SSS calculation) can unintentionally decrease performance by enabling high-cost GPU operations that were previously optimized out by the compiler.
**Action:** When optimizing, verify if a "fix" actually enables expensive paths. If performance is the priority, consider removing the unused logic entirely instead of enabling it.
## 2024-05-25 - Unity WaitForSeconds float Dictionary Anti-Pattern
**Learning:** While caching `WaitForSeconds` is necessary to avoid per-iteration Garbage Collection inside Unity coroutines, using a `Dictionary<float, WaitForSeconds>` is an anti-pattern. Floating-point precision inaccuracies make `float` unreliable as a dictionary key, potentially causing cache misses or unexpected behavior.
**Action:** Instead of using a float-keyed dictionary, pre-calculate specific `WaitForSeconds` instances (e.g., normal delay, short pause, long pause) into local variables outside the coroutine loop, and yield those local variables directly inside the loop.
## 2026-03-26 - [Advanced Unity Caching & Memory Efficiency]
**Learning:** When caching Unity components, using `InstanceID` (int) as a dictionary key is significantly more efficient than strings or `ToString()` calls, as it avoids heap allocations during every lookup. Furthermore, robust negative caching for Unity objects (which override `== null`) requires using `ReferenceEquals(obj, null)` to distinguish between a legitimate `null` cache entry and a reference to a natively destroyed object (fake null).
**Action:** Use `int` (GetInstanceID) for component cache keys and `ReferenceEquals` to manage the lifecycle of cached Unity objects correctly.
## 2026-03-26 - Unity Negative Caching Pitfalls
**Learning:** In Unity managers like `SceneDirector.cs` that both find and instantiate objects, "negative caching" (storing `null` in the dictionary when `GameObject.Find` fails) is a dangerous anti-pattern. If an object is instantiated later in the same frame or scenario, subsequent lookups will incorrectly return the cached `null` instead of the newly created object. Furthermore, Unity's `obj != null` check is essential even for cached references to detect if the native C++ object was destroyed.
**Action:** When caching `GameObject.Find` results, always use the `if (_cache.TryGetValue(key, out obj) && obj != null)` pattern. Do not cache `null` results if there is any chance the object will be created later. Ensure the cache is updated immediately after any `Instantiate` calls.

## 2026-05-06 - Float Keys in WaitForSeconds Cache
**Learning:** Using floats as keys in a Dictionary for caching WaitForSeconds causes cache misses due to floating-point imprecision, leading to unnecessary GC allocations and defeating the purpose of the cache.
**Action:** Use an integer key representing milliseconds (via `Mathf.RoundToInt(time * 1000f)`) to ensure cache hits.
## 2024-05-26 - WaitForSeconds Float Dictionary Cache Misses
**Learning:** Caching `WaitForSeconds` using a `float` as the dictionary key can lead to cache misses due to floating-point imprecision, causing unintended `WaitForSeconds` instantiations and unnecessary GC allocations.
**Action:** Always use an integer key (e.g., representing milliseconds via `Mathf.RoundToInt(time * 1000f)`) instead of a `float` when caching `WaitForSeconds` instances in a dictionary.
