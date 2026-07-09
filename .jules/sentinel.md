## 2024-05-24 - Path Traversal in Editor Extension Scripts
**Vulnerability:** Found a Path Traversal vulnerability in an Editor window script (`CharacterFactory.cs`). The script trusted the `name` field from a parsed JSON file (`campaign_master.json`) to construct the file path for creating new `.asset` files (`string assetPath = $"{folderPath}/{charProfile.name...}.asset";`).
**Learning:** Even though the JSON is a "local" file used by a developer in the Editor, trusting external data to construct file paths without sanitization is a security risk. If a malicious JSON file is imported, it could contain names like `../../Scripts/Core/ImportantScript` which would overwrite arbitrary files in the project.
**Prevention:** Always sanitize any data sourced from external files (like JSON, XML) before using it in path construction. Use `Path.GetInvalidFileNameChars()` to filter out OS-level invalid characters and `Path.GetFileName()` to strip directory traversal sequences like `../` to ensure the resulting string is strictly a file name.
## 2024-05-24 - Path Traversal via Unsanitized JSON Data in Unity Editor
**Vulnerability:** Path traversal vulnerability in `CharacterFactory.cs` where imported JSON `charProfile.name` was used directly in an `AssetDatabase.CreateAsset` path without sanitization.
**Learning:** Unity Editor scripts that dynamically generate files or paths from imported data (like JSON) are susceptible to path traversal. Even seemingly safe strings like "character name" can be manipulated maliciously (e.g. `../MaliciousAsset`) to overwrite critical project assets or write files outside the intended folder.
**Prevention:** Always sanitize dynamically imported strings that are used in file paths. Use `Path.GetInvalidFileNameChars()` to strip/replace illegal characters, and ensure no directory separators are left before appending to a hardcoded base folder path.
## 2024-11-06 - Prevent Information Exposure via Error Logs in Unity Scripts
**Vulnerability:** Found `CampaignManager.cs` logging exact absolute file paths (using `Application.streamingAssetsPath` and `Application.dataPath`) and lacking exception handling around a JSON read/parse block, potentially leaking system info or stack traces.
**Learning:** Unity's built-in platform paths can expose underlying OS directory structures, usernames, or execution environments when logged directly, especially if logs are captured.
**Prevention:** Mask runtime exception stack traces (e.g. log only `ex.Message`) and replace absolute path logs with safe generic identifiers (like the filename) to avoid information exposure.
## 2024-05-18 - Information Disclosure via Absolute File Paths in Logs\n**Vulnerability:** Found `Debug.Log` statements outputting absolute paths like `Path.Combine(Application.dataPath, ...)` into Unity logs. In production, this leaks server or client filesystem structure, potentially aiding attackers in path traversal or deeper filesystem exploitation.\n**Learning:** `Application.dataPath` and `Application.streamingAssetsPath` resolve to complete absolute paths that should remain internal.\n**Prevention:** Only use filenames or safe identifiers when logging file operations, rather than the full `filePath`. Also, explicitly catch file parsing errors to avoid leaking internal stack traces.
## 2024-05-24 - [Information Leakage via Stack Trace]
**Vulnerability:** Public GameObjects and UI components (like `DialogueBox`, `SpeakerNameText`, and `DialogueText`) were being accessed directly in `Cinematic_IntoTheVoid.cs` without prior null checks.
**Learning:** If these components are unassigned in the Unity Editor, calling methods or properties on them triggers a `NullReferenceException`, which exposes the internal application call stack/stack trace. This violates "Fail securely - errors should not expose sensitive data."
**Prevention:** Always implement defensive programming by verifying that required object references are not null before accessing them. If they are missing, log a secure, generic error message (e.g., "Missing UI components required for cinematic") and safely abort the operation (e.g., using `return;`).
## 2024-05-24 - Unity File Path Information Disclosure and Unhandled Exception Logging
**Vulnerability:** Information Disclosure and Error Stack Trace Leak
**Learning:** `Application.dataPath` and `Application.streamingAssetsPath` resolve to physical file paths on the host system. Logging these directly in production environments or allowing unhandled exceptions from methods like `File.ReadAllText` and `JsonUtility.FromJson` to print to the console can expose internal file structures and stack traces to attackers.
**Prevention:** Always use relative file paths or file names (e.g. `fileName`) in logs. Wrap file I/O operations and JSON deserialization in `try-catch` blocks to fail securely and prevent unhandled exceptions from leaking internal details. Catch generic exceptions and provide a sanitized, secure error message.
## 2025-01-24 - Input Validation and Secure Exception Handling in Game Data Management
**Vulnerability:** The application lacked a systematic way to validate the integrity and security of deserialized JSON campaign data, potentially leading to logic errors or vulnerabilities if malicious data was loaded. Furthermore, previous "security fixes" in `CampaignManager.cs` and `CharacterFactory.cs` had introduced syntax errors and redundant/ineffective logging.
**Learning:** Security fixes applied without full contextual understanding or verification can lead to compilation errors and degraded maintainability. A centralized `IsValid()` pattern in the data model provides a clean, reusable way to enforce business constraints and security requirements immediately after deserialization.
**Prevention:** Implement an `IsValid()` method for all serializable data models that are populated from external sources. Use these methods at the point of ingestion (e.g., in `CampaignManager`). Ensure error handling in these ingestion points uses generic error messages and avoids logging absolute paths or stack traces.
## 2024-05-24 - Data Integrity and Security Validation for Deserialized Assets
**Vulnerability:** Untrusted external data (JSON) was being used directly by the application without validation, potentially leading to out-of-bounds values or corrupted application state.
**Learning:** Even if data is "local", it should be treated as untrusted input once it crosses the boundary from a file into the application.
**Prevention:** Implement an `IsValid()` pattern in data models to perform security and integrity checks immediately after deserialization. This ensures the application fails fast and securely when encountering malicious or corrupted data.

## 2024-04-22 - IDOR in Scene Object Lookups
**Vulnerability:** Insecure Direct Object Reference (IDOR) via unsanitized `GameObject.Find` queries in `SceneDirector.cs`.
**Learning:** External JSON data could provide malicious object IDs (like "CampaignManager") to interact with core architectural singletons, because `GameObject.Find` has uncontrolled access to any scene hierarchy object. Broad string heuristics were avoided to prevent functional regressions.
**Prevention:** Always sanitize and validate object IDs originating from external sources using exact matches before using them in scene traversal queries.

## 2024-04-22 - IDOR in Scene Object Interactions
**Vulnerability:** Insecure Direct Object Reference (IDOR) via unsanitized `interaction.objectId` queries in `SceneDirector.cs`.
**Learning:** External JSON data could provide malicious object IDs (like "CampaignManager") to interact with core architectural singletons, because `ApplyInteraction` uses the uncontrolled string ID to fetch and manipulate the game object. The validation must happen at the boundary where the untrusted input is used, not inside low-level generic retrieval utilities like `GetCachedObject` to avoid breaking internal game logic.
**Prevention:** Always sanitize and validate object IDs originating from external sources using exact matches before using them to interact with scene objects.
## 2025-05-14 - Code Rot as a Security Risk in Core Data Management
**Vulnerability:** Extreme code rot (redundant member declarations, syntax errors, and duplicate logic) in `CampaignManager.cs` and `CharacterData.cs` created a scenario where security validation could be accidentally bypassed. For example, `CampaignManager.cs` had overlapping `if` blocks that assigned deserialized data to global state even if validation failed on a different instance of the same data.
**Learning:** Code rot isn't just a maintainability issue; it's a security vulnerability. Contradictory or redundant logic paths make it difficult to verify that security controls (like `IsValid()` checks) are consistently and correctly applied.
**Prevention:** Consolidate data structures and logic into a single, clean "source of truth." Ensure that validation logic is performed on the exact object being assigned to the global state immediately after deserialization.

## 2025-05-14 - Resource Exhaustion (DoS) via Unsanitized Trait Strings
**Vulnerability:** While `HorizonGameData.cs` limited the number of traits per character, it did not limit the length of individual trait strings. A malicious JSON could contain a few traits each with millions of characters, leading to memory exhaustion.
**Learning:** Defense-in-depth requires validating both the quantity and the size of individual elements in collections sourced from external data.
**Prevention:** Implement length constraints for every string field in a serializable data model, especially within lists.

## 2024-05-24 - IDOR in Data-Driven Scene Construction
**Vulnerability:** `GameObject.Find` was used directly on unsanitized external strings from JSON, granting uncontrolled access to any scene hierarchy object.
**Learning:** External data targeting interactive objects can specify core singletons (like `CampaignManager` or `SceneDirector`), allowing attackers to manipulate or destroy critical game components. Broad heuristics like `.Contains("Manager")` break legitimate objects.
**Prevention:** Use exact string matching or specific tagging to block unauthorized access to core architectural singletons when performing dynamic object lookups.

## 2026-04-14 - Denial of Service (DoS) mitigation for GameObject.Find
**Vulnerability:** Untrusted input from JSON was used in `GameObject.Find` in `SceneDirector.cs`. Maliciously long strings or path-traversal-like patterns (e.g., using `/`) could lead to expensive scene traversals or unexpected object access.
**Learning:** `GameObject.Find` is an O(N) operation that can be abused. While caching helps, the initial lookup using unsanitized strings is a risk.
**Prevention:** Implement strict string length limits (e.g., 128 chars) and character whitelists (alphanumeric, underscores, spaces, etc.) for any string passed to `GameObject.Find` or used as a key in object caches derived from external data.

## 2025-05-14 - [Information Disclosure in Unity Logs]
**Vulnerability:** Absolute file paths being logged during data loading.
**Learning:** Hardcoded paths or `Application.dataPath` in logs can reveal developer filesystem structure.
**Prevention:** Only log filenames or relative identifiers in production-facing logs.

## 2026-05-12 - [Resource Exhaustion via Untrusted JSON Data]
**Vulnerability:** Deserialized campaign data (`HorizonGameData.cs`) lacked uniform length and count constraints, making the application vulnerable to resource exhaustion (DoS) attacks if a malicious JSON with extremely large strings or lists was loaded. Additionally, severe code rot (redundant class definitions) masked these vulnerabilities and impeded security audits.
**Learning:** Code rot and structural duplication not only degrade maintainability but also provide vectors for bypassing security controls or complicating audits. Centralized, hardened `IsValid()` methods are essential for defense-in-depth when dealing with untrusted external data.
**Prevention:** Consolidate data structures to ensure a single source of truth and implement strict validation (length, range, count) for all fields in serializable models.
## 2026-05-15 - [UI Injection via Code Rot in OtisTerminal]
**Vulnerability:** Redundant command processing logic in `OtisTerminal.cs` echoed user input to the terminal before performing length or regex validation. Since the terminal supports Rich Text tags, an attacker could inject malicious tags (e.g., extremely large font sizes or overlapping colors) to disrupt the UI or bypass intended interface constraints.
**Learning:** Code rot (duplicate logic paths and redundant echoes) often hides missing security controls. In this case, one echo path was correctly placed but another was redundant and unvalidated.
**Prevention:** Consolidate interactive input processing into a single, linear pipeline: Validate -> Sanitize -> Echo -> Execute. Never echo untrusted input before it has passed all security checks.

## 2026-05-18 - [Security Validation Bypass via Code Rot]
**Vulnerability:** A missing closing brace in a redundant scenario check within `HorizonGameData.cs` caused the C# compiler to skip subsequent character and scenario validation loops, effectively bypassing deep-validation for untrusted JSON data.
**Learning:** Code rot, specifically orphaned or malformed conditional blocks, can lead to silent security failures where critical validation logic is bypassed without triggering immediate runtime errors.
**Prevention:** Regularly audit validation logic for redundancy and ensure that all validation paths are fully covered by unit tests and static analysis. Favor clean, linear validation pipelines over complex, nested, or redundant checks.

## 2024-05-18 - IDOR and NRE in Scene Interaction System
**Vulnerability:** Insecure Direct Object Reference (IDOR) via unsanitized `interaction.objectId` in `SceneDirector.cs`, and a `NullReferenceException` risk due to accessing properties before null-checking the `interaction` object.
**Learning:** Broad interaction systems that allow external data to target objects by name or ID are vulnerable to IDOR attacks if critical system managers (e.g., `CombatManager`, `GlobalResonanceManager`) are not strictly blocked. Additionally, incorrect ordering of validation logic can lead to runtime crashes that expose internal state.
**Prevention:** Implement a hardened blocklist for all core architectural singletons and system managers at the boundary of the interaction system. Always perform defensive null-checks and property validation before attempting to use untrusted data.
## 2026-06-20 - [IDOR and Information Disclosure via Code Rot in SceneDirector]
**Vulnerability:** Insecure Direct Object Reference (IDOR) and potential stack trace leakage in `SceneDirector.cs`.
**Learning:** Code rot (duplicate variable declarations and redundant logic) masked missing security controls. `ApplyInteraction` was using unsanitized string IDs to manipulate scene objects, including core architectural managers. Additionally, missing null checks on external `ObjectInteraction` data could trigger `NullReferenceException`, exposing internal stack traces.
**Prevention:** Implement strict defensive programming by null-checking untrusted input before access. Use an explicit blocklist (or ideally an allowlist) to prevent IDOR attacks on sensitive architectural singletons. Always qualify static Unity method calls (like `UnityEngine.Object.Instantiate`) when working in environments with potential namespace ambiguity or simplified mocks.
## 2026-05-24 - [IDOR and NullReference Leak in SceneDirector]
**Vulnerability:** `ApplyInteraction` in `SceneDirector.cs` lacked defensive null checks for the `ObjectInteraction` parameter, which could lead to `NullReferenceException` and information leakage via stack traces. Furthermore, the IDOR blocklist was incomplete, omitting several critical system managers.
**Learning:** Security blocklists must be regularly audited to ensure all core architectural singletons are covered. Additionally, defensive programming is the first line of defense against information disclosure via runtime errors.
**Prevention:** Ensure all external interaction points have comprehensive null checks at the start of the method. Maintain a centralized or well-audited blocklist for sensitive scene objects.
## 2026-05-20 - [IDOR Protection Bypass via Logic Flaws and Code Rot]
**Vulnerability:** In `SceneDirector.cs`, the `ApplyInteraction` method accessed `interaction.objectId` before performing a null check on `interaction`, and it contained redundant variable declarations that caused compilation failures. Furthermore, the IDOR blocklist was incomplete, leaving several system managers (CombatManager, GlobalResonanceManager, BicameralBattleEngine) vulnerable to unauthorized manipulation.
**Learning:** Security controls implemented within "rotted" or redundant code are prone to logic errors and bypasses. A security check that crashes the application (via `NullReferenceException`) or fails to compile is as ineffective as no check at all.
**Prevention:** Consolidate security-critical logic into a single, clean, and properly sequenced pipeline: Validate Input -> Check Authorization (Blocklist/Allowlist) -> Execute. Ensure that the blocklist is comprehensive and covers all sensitive architectural singletons.
## 2026-05-20 - Exhaustive IDOR Blocklisting for Core Managers
**Vulnerability:** Incomplete IDOR blocklist in `SceneDirector.cs`.
**Learning:** Initial security implementations for data-driven object interaction blocked some system managers (`CampaignManager`, `SceneDirector`) but missed others (`CombatManager`, `GlobalResonanceManager`, `BicameralBattleEngine`). This left critical game state vulnerable to unauthorized manipulation via external campaign data.
**Prevention:** When implementing a blocklist for sensitive architectural components, ensure it is exhaustive and regularly audited as new managers are added to the codebase.
## 2025-05-18 - [IDOR Protection and Code Refactoring in SceneDirector]
**Vulnerability:** The `ApplyInteraction` method in `SceneDirector.cs` had a incomplete IDOR blocklist and suffered from code rot (duplicate variable declarations and misplaced null checks), which could lead to `NullReferenceException` or unauthorized access to core system managers.
**Learning:** Security validation must be the first line of defense in a method. Incomplete blocklists and redundant logic paths (code rot) can obscure security gaps and cause runtime failures.
**Prevention:** Always place null checks and security validation at the very beginning of methods processing external data. Ensure the blocklist of sensitive system objects is comprehensive and includes all core managers and engines (e.g., `CombatManager`, `GlobalResonanceManager`, `BicameralBattleEngine`).
## 2026-05-26 - [IDOR and Code Rot in Scene Interaction]
**Vulnerability:** Insecure Direct Object Reference (IDOR) via an incomplete security blocklist in `SceneDirector.cs`. Malicious external data could target critical system managers like `CombatManager` or `GlobalResonanceManager` because they were omitted from the initial security check. Additionally, severe code rot (duplicate variable declarations and redundant logic) obscured these gaps.
**Learning:** Security blocklists must be comprehensive and regularly updated to include new architectural components. Code rot and redundant logic paths increase the risk of security validation being bypassed or misconfigured.
**Prevention:** Maintain a centralized, hardened list of protected system objects. Consolidate interaction logic into a clean "Validate-then-Execute" pipeline to ensure consistent security enforcement.

## 2026-06-21 - [Consolidation of Security Controls and Code Rot Mitigation]
**Vulnerability:** Insecure Direct Object Reference (IDOR) via an incomplete blocklist in `SceneDirector.cs` and UI Injection/DoS via code rot in `OtisTerminal.cs`.
**Learning:** Code rot (specifically redundant field declarations and conflicting logic blocks) masks missing security controls and introduces bugs like double instantiation. In `OtisTerminal.cs`, redundant echo paths bypassed input validation, potentially allowing Rich Text UI injection.
**Prevention:** Consolidate interactive input and external data processing into a single, linear "Validate -> Sanitize -> Execute" pipeline. Ensure security validation (length, regex, range) is the first line of defense before any echoing or state changes occur.

## 2024-05-29 - [Resource Exhaustion (DoS) via Unbounded History]
**Vulnerability:** The command history in `OtisTerminal.cs` grew indefinitely, potentially leading to memory exhaustion if a large number of commands were processed.
**Learning:** Even simple lists of strings can become a DoS vector if they are populated from user input without bounds.
**Prevention:** Always implement explicit size limits for collections that store historical user input.
## 2024-05-27 - [IDOR Bypass and DoS via Code Rot and Missing Validation]
**Vulnerability:** Insecure Direct Object Reference (IDOR) bypass in `SceneDirector.cs` due to a syntax error (dangling `||`) that truncated the blocklist check, and a physics-based Denial of Service (DoS) vulnerability in `HorizonGameData.cs` where `NaN` or `Infinity` values were not validated.
**Learning:** Code rot, specifically malformed conditional logic, can silently disable security controls. Furthermore, numeric validation is critical in engine-integrated systems to prevent terminal instability from malicious data.
**Prevention:** Always verify that security blocklists are syntactically complete and maintain a "fail-fast" validation layer for all numeric inputs sourced from external data.
## 2024-05-27 - [UI Injection via Code Rot in OtisTerminal]
**Vulnerability:** Redundant member declarations and overlapping command processing logic in `OtisTerminal.cs` echoed user input to the terminal without escaping Rich Text tags (`<`, `>`). This allowed users to inject malicious tags (e.g., `<size=1000>`) to disrupt the UI.
**Learning:** Code rot, specifically duplicate class members and fragmented logic paths, can hide missing security controls and make it difficult to ensure consistent input sanitization.
**Prevention:** Consolidate interactive input processing into a single, clean pipeline and ensure that all untrusted input is sanitized (e.g., escaping rich text tags) before being echoed to the UI.

## 2024-05-28 - [IDOR Protection Hardening and Code Rot Consolidation]
**Vulnerability:** Incomplete IDOR blocklist in  and a syntax error (dangling `||` operator) caused by redundant, rotted code blocks that potentially bypassed security checks. Critical system managers like `TimelineSimulationEngine` were omitted from the protection list.
**Learning:** Code rot, especially when it results in multiple overlapping validation blocks, often leads to syntax errors that are overlooked during quick audits. These errors can silently disable security controls.
**Prevention:** Consolidate validation logic into a single, well-defined pipeline. Regularly audit blocklists against the actual list of architectural singletons in the project.

## 2024-05-28 - [IDOR Protection Hardening and Code Rot Consolidation]
**Vulnerability:** Incomplete IDOR blocklist in SceneDirector.cs and a syntax error (dangling '||' operator) caused by redundant, rotted code blocks that potentially bypassed security checks. Critical system managers like TimelineSimulationEngine were omitted from the protection list.
**Learning:** Code rot, especially when it results in multiple overlapping validation blocks, often leads to syntax errors that are overlooked during quick audits. These errors can silently disable security controls.
**Prevention:** Consolidate validation logic into a single, well-defined pipeline. Regularly audit blocklists against the actual list of architectural singletons in the project.
## 2025-05-20 - [IDOR Protection Hardening and Code Rot Consolidation]
**Vulnerability:** `SceneDirector.cs` had a fragmented IDOR blocklist and redundant null checks (code rot), which masked a missing protection for `TimelineSimulationEngine`.
**Learning:** Overlapping and redundant security checks often lead to logic errors and maintenance gaps. Consolidating security validation into a single, linear pipeline ensures all checks are executed and simplifies auditing.
**Prevention:** Always prioritize a "Validate-then-Execute" pipeline and maintain a comprehensive, single-source-of-truth blocklist for sensitive architectural components.

## 2024-05-28 - [IDOR Protection Bypass via Code Rot and Syntax Errors]
**Vulnerability:** A critical security bypass was found in `SceneDirector.cs` where multiple overlapping `if` blocks and a dangling `||` operator in the `ApplyInteraction` method effectively neutralized the IDOR protection. This allowed potentially malicious external data to interact with any scene object, including core managers.
**Learning:** Code rot and redundant logic paths are major security risks. In this case, previous attempts to "harden" the code actually broke it by introducing syntax errors and logic contradictions.
**Prevention:** Consolidate security-critical logic into a single, linear "Validate-then-Execute" pipeline. Avoid redundant validation blocks that can lead to maintenance gaps and silent failures.
## 2025-05-21 - [IDOR Bypass via Syntax Error in SceneDirector]
**Vulnerability:** A syntax error in `SceneDirector.cs` (a dangling `||` operator in an `if` condition) truncated the IDOR blocklist check, allowing unauthorized manipulation of critical system managers.
**Learning:** Code rot, especially when it results in multiple overlapping validation blocks, often leads to syntax errors that are overlooked during quick audits. These errors can silently disable security controls.
**Prevention:** Consolidate security-critical logic into a single, clean, and properly sequenced pipeline: Validate Input -> Check Authorization (Blocklist) -> Execute. Avoid redundant, rotted code blocks that obscure the primary security path.

## 2025-05-22 - [Case-Sensitive IDOR Bypass and Terminal Spoofing]
**Vulnerability:** The IDOR blocklist in `SceneDirector.cs` used a default case-sensitive HashSet, allowing potential bypasses via casing variations (e.g., "campaignmanager"). Additionally, `OtisTerminal.cs` used `\s` in its validation regex, which includes newline characters, making the terminal vulnerable to UI spoofing.
**Learning:** Security blocklists using string matching must account for casing variations to prevent trivial bypasses. Furthermore, input validation regexes should be as restrictive as possible; using broad character classes like `\s` can introduce unexpected vulnerabilities like terminal spoofing.
**Prevention:** Always initialize security-critical HashSets or Dictionaries with `StringComparer.OrdinalIgnoreCase`. Use explicit character sets like `[ \t]` instead of `\s` when newline characters should be excluded from validated input.
## 2024-05-29 - [Terminal Spoofing via Newline Injection]
**Vulnerability:** The `SafeCommandRegex` in `OtisTerminal.cs` used `\s` which allowed newline characters. An attacker could inject newlines followed by fake system messages (e.g., `help\n[SYSTEM]: Admin access granted`) to spoof the terminal UI and deceive users.
**Learning:** Generic whitespace matchers (`\s`) are dangerous in terminal/chat interfaces where newlines can be used to break out of the intended message format.
**Prevention:** Use explicit whitespace character classes like `[ \t]` when newlines should be disallowed in a single-line input field.

## 2024-05-29 - [IDOR Protection Bypass via Case Sensitivity]
**Vulnerability:** The `ProtectedSystemObjects` blocklist in `SceneDirector.cs` used default string comparison. Malicious external data could use alternate casing (e.g., `campaignmanager`) to potentially bypass the blocklist while still matching the target object in case-insensitive environments or if the lookup logic was inconsistent.
**Learning:** Security blocklists for strings should always use case-insensitive comparison (e.g., `StringComparer.OrdinalIgnoreCase`) to ensure defense-in-depth against variations in input casing.
**Prevention:** Always initialize security-critical HashSets or Dictionaries with `OrdinalIgnoreCase` when dealing with untrusted string identifiers.
## 2024-05-28 - [IDOR Protection and Build Hygiene]
**Vulnerability:** Incomplete IDOR blocklist in SceneDirector.cs and repository pollution via build artifacts (bin/, obj/, .dll, .pdb).
**Learning:** Security auditing must extend beyond code to include repository hygiene. Committing build artifacts can obscure source changes and introduce supply chain risks.
**Prevention:** Maintain a strict .gitignore and always audit the protected object list when new high-level orchestrators or managers are added.
## 2025-05-28 - [IDOR Case-Bypass and Code Rot in Terminal Logic]
**Vulnerability:** Insecure Direct Object Reference (IDOR) bypass in `SceneDirector.cs` via case-insensitive object name matching, and potential security validation bypass in `OtisTerminal.cs` due to extreme code rot (redundant loops and orphaned string blocks).
**Learning:** Security blocklists using default string comparisons are vulnerable to case-variation bypasses (e.g., 'scenedirector' vs 'SceneDirector'). Furthermore, rotted code with duplicate logic paths makes it difficult to ensure that all execution branches are properly validated and sanitized.
**Prevention:** Use `StringComparer.OrdinalIgnoreCase` for all security-critical string lookups and blocklists. Consolidate interactive input processing into a single, linear pipeline to eliminate redundant and unvalidated execution paths.
## 2025-05-24 - [Case-Insensitive IDOR Bypass in SceneDirector]
**Vulnerability:** The `ProtectedSystemObjects` blocklist in `SceneDirector.cs` was initialized as a case-sensitive `HashSet<string>`. This allowed potential IDOR bypasses where an attacker could provide an ID like `scenedirector` or `CAMPAIGNMANAGER` to circumvent the security check while still successfully resolving the object via Unity's (often case-tolerant) lookup methods or the application's internal caches.
**Learning:** Security blocklists must account for the normalization behavior of the underlying systems they protect. If the target system is case-insensitive, the security check must also be case-insensitive.
**Prevention:** Always use `StringComparer.OrdinalIgnoreCase` when creating HashSets or Dictionaries intended for security validation of string-based IDs.

## 2025-05-24 - [Terminal Spoofing and Injection via Newline Characters]
**Vulnerability:** The `SafeCommandRegex` in `OtisTerminal.cs` used the generic `\s` whitespace shorthand, which includes newline (`\n`) and carriage return (`\r`) characters. This could allow an attacker to inject multi-line inputs that might spoof terminal output or bypass certain single-line processing assumptions.
**Learning:** Overly permissive whitespace validation in interactive consoles can lead to UI spoofing or command injection vulnerabilities.
**Prevention:** Use explicit whitespace character classes (e.g., `[ \t]`) instead of `\s` when validating single-line command inputs to ensure control characters like newlines are strictly forbidden.
## 2025-05-22 - Case-Insensitive IDOR Protection in Scene Interaction
**Vulnerability:** Insecure Direct Object Reference (IDOR) via case-sensitive blocklist checks in SceneDirector.cs.
**Learning:** Blocklists for untrusted input identifiers are fragile if they don't account for casing variations, especially if the underlying lookup (like GameObject.Find) or the input source might normalize or ignore casing.
**Prevention:** Always use 'StringComparer.OrdinalIgnoreCase' when initializing blocklists or performing security-critical string comparisons for external identifiers.

## 2025-06-17 - Double-Validation for IDOR Protection
**Vulnerability:** Insecure Direct Object Reference (IDOR) via `GameObject.Find` in `SceneDirector.cs`. Previous protections only checked the input string against a blocklist. An attacker could potentially use whitespace or other string variations to bypass the initial string check while still resolving to a protected object.
**Learning:** Checking only the input string is insufficient if the underlying lookup system (`GameObject.Find`) might resolve different or rotted string variations to the same sensitive object.
**Prevention:** Implement "Double Validation": Validate the untrusted input string against the blocklist, resolve the object, and then *re-validate* the resolved object's actual name against the blocklist before performing any operations.

## 2025-06-25 - [Security Logic Fragility via Code Rot]
**Vulnerability:** Critical security controls (IDOR blocklists in `SceneDirector.cs` and Rich Text sanitization in `OtisTerminal.cs`) were repeatedly bypassed or broken due to extreme code rot and duplicate logic paths. Redundant class members and overlapping method implementations caused silent failures where one path was secured but another was rotted and vulnerable.
**Learning:** Code rot is a high-severity security risk in interactive systems. Security validation that only covers one of several possible execution paths is a "security theater" that masks real vulnerabilities.
**Prevention:** Enforce a strict "Single Source of Truth" for all security-critical pipelines. Consolidate interactive input processing into linear, deduplicated pipelines (Validate -> Sanitize -> Execute) and use automated build checks to detect syntax errors introduced by rotted logic.
## 2025-05-24 - [IDOR Protection and UI Injection Hardening via Code Rot Resolution]
**Vulnerability:** Extreme code rot in `OtisTerminal.cs` and `SceneDirector.cs` created logic bypasses where untrusted input was echoed to the UI without escaping (Rich Text injection) and critical system managers were exposed to IDOR attacks due to fragmented validation blocks and syntax errors.
**Learning:** Code rot (duplicate methods, rotted logic paths, and syntax errors) is a primary vector for security bypasses. A security control that is triplicated is often only updated in one place, leaving the others vulnerable. In `SceneDirector.cs`, a dangling `||` operator effectively neutralized the IDOR blocklist.
**Prevention:** Consolidate security-critical logic into a single, linear "Validate -> Sanitize -> Execute" pipeline. Implement "Double Validation" for object lookups: check the input ID *and* the resolved object's actual name against the blocklist to prevent path-traversal-style bypasses. Always escape interactive UI input (e.g. `<` to `&lt;`) to prevent injection attacks.

## 2026-06-21 - [IDOR Protection Hardening and Code Rot Consolidation]
**Vulnerability:** Insecure Direct Object Reference (IDOR) via an incomplete and case-sensitive blocklist in `SceneDirector.cs`. Malicious external data could bypass the blocklist using case variations (e.g., 'campaignmanager') or path-like strings (e.g., '/CampaignManager') that `GameObject.Find` still resolves.
**Learning:** String-based security blocklists are fragile if they don't account for casing and the normalization behavior of the underlying lookup system. Furthermore, severe code rot (triplicated methods and redundant fields) in `OtisTerminal.cs` obscured UI injection vulnerabilities and caused build failures.
**Prevention:** Implement "Double Validation": check the input string *and* the resolved object's name against a case-insensitive blocklist. Regularly consolidate and deduplicate interactive processing logic to ensure security controls are consistently applied.
