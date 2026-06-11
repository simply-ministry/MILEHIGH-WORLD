## 2025-05-14 - GC optimization in Unity C#
**Learning:** In Unity, `new WaitForSeconds(n)` in a coroutine loop and string concatenation in a UI loop are common sources of frame-time spikes due to GC pressure. Additionally, heap-allocating arrays for algorithms like Levenshtein distance during user input can be eliminated using `stackalloc` and `Span<T>`.
**Action:** Always check for `new WaitForSeconds` in coroutines and string concatenation in high-frequency UI updates. Use `StringBuilder` for string building and `stackalloc Span<T>` for temporary buffer needs in performance-critical paths.
