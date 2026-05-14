## 2025-05-14 - Code Rot as a Security Risk in Core Data Management
**Vulnerability:** Extreme code rot (redundant member declarations, syntax errors, and duplicate logic) in `CampaignManager.cs` and `CharacterData.cs` created a scenario where security validation could be accidentally bypassed. For example, `CampaignManager.cs` had overlapping `if` blocks that assigned deserialized data to global state even if validation failed on a different instance of the same data.
**Learning:** Code rot isn't just a maintainability issue; it's a security vulnerability. Contradictory or redundant logic paths make it difficult to verify that security controls (like `IsValid()` checks) are consistently and correctly applied.
**Prevention:** Consolidate data structures and logic into a single, clean "source of truth." Ensure that validation logic is performed on the exact object being assigned to the global state immediately after deserialization.

## 2025-05-14 - Resource Exhaustion (DoS) via Unsanitized Trait Strings
**Vulnerability:** While `HorizonGameData.cs` limited the number of traits per character, it did not limit the length of individual trait strings. A malicious JSON could contain a few traits each with millions of characters, leading to memory exhaustion.
**Learning:** Defense-in-depth requires validating both the quantity and the size of individual elements in collections sourced from external data.
**Prevention:** Implement length constraints for every string field in a serializable data model, especially within lists.
