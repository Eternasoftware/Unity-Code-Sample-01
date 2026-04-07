# 🎮 Somnambulo — Unity Code Sample

A modular Unity project built with **VContainer** for dependency injection, showcasing a clean architecture for game development. This repository contains production-ready patterns for dependency management, build automation, scene lifecycle handling, and event-driven trigger systems.

---

## 📂 Project Structure

### 🔧 Build Info

> *Automated build metadata injection for Unity projects.*

This module ensures every build carries its own DNA. The `BuildInfoPreprocessor` hooks into Unity's build pipeline (`IPreprocessBuildWithReport`) and automatically stamps the `BuildInfoConfig` ScriptableObject with:

- **Build Date** — UTC timestamp of the build
- **Bundle Version Code** — Android version code
- **Commit Hash** — Git short hash for traceability

Perfect for debugging, QA reporting, and release tracking.

**Key Files:**
- `BuildInfoConfig.cs` — ScriptableObject holding build metadata
- `BuildInfoPreprocessor.cs` — Editor-time build hook that auto-updates the config

---

### 🚀 Entry Point

> *The heart of the application — DI scopes, scene bootstrapping, and service registration.*

This is where the game comes alive. Using **VContainer**'s `LifetimeScope` system, the Entry Point module wires up all core services, databases, and infrastructure components in a clean, hierarchical fashion.

**Highlights:**

| File | Purpose |
|------|---------|
| `GameLifetimeScope.cs` | Root DI scope. Registers all databases (triggers, items, weapons, projectiles, doors, sockets), build info, sound manager, and the `SceneLoaderService`. Marked as `DontDestroyOnLoad` for persistence across scenes. |
| `LevelScopeExtensions.cs` | Extension method `RegisterGeneralLevelDependencies()` — a reusable recipe for registering level-scoped services, factories, binders, and entry points. |
| `PlaygroundLevelLifetimeScope.cs` | Per-level DI scope. Registers `PlaygroundLevelViewModel`, `PlayerViewModel`, and player context, then delegates to the general level dependencies. |
| `SceneMasterBinder.cs` | The conductor of the orchestra. Implements `IStartable` to orchestrate the binding sequence for doors, weapons, items, triggers, fabric cuts, puzzles, sockets, and inventory at scene start. |

---

### ⚡ Triggers Handling

> *Event-driven trigger system with ScriptableObject IDs and ViewModel-based event consumption.*

A decoupled, observable trigger system that turns in-game events into clean, testable signals. Built on the **EventBus** pattern with ScriptableObject-based identifiers for designer-friendly configuration.

**Key Components:**

- **`ScriptableId.cs`** — Abstract base class for ScriptableObject IDs. Provides a `StringId` derived from the asset name, enabling human-readable identifiers without magic strings.

- **`TriggerId.cs`** — Concrete ScriptableObject for trigger identifiers. Create trigger IDs via `Right Click → Create → Somnambulo/TriggerId`. Perfect for designers to wire up triggers in the editor.

- **`PlaygroundLevelViewModel.cs`** — A level-scoped ViewModel that subscribes to the `TriggersEventBus`. Reacts to trigger activations (`OnTriggerActivated`) and can execute level-specific logic (e.g., logging, state changes, scene transitions). Demonstrates the **MVVM** pattern adapted for Unity.

---

## 🛠️ Tech Stack

| Technology | Purpose |
|------------|---------|
| **VContainer** | High-performance dependency injection |
| **Odin Inspector** | Enhanced ScriptableObject UI (info boxes, validation) |
| **Sonity** | Audio management |
| **NodeCanvas** | Visual scripting (via `NodeCanvasGlobalBridge`) |
| **SRDebugger** | In-game debugging overlay |

---

## 📐 Architecture Highlights

- **Dependency Injection** — All dependencies are resolved through VContainer's `LifetimeScope` hierarchy (Game → Level)
- **Event-Driven Design** — `TriggersEventBus` decouples trigger sources from consumers
- **ScriptableObject IDs** — Designer-friendly identifiers with zero magic strings
- **Build Automation** — Preprocessor hooks inject metadata without manual intervention
- **MVVM-inspired ViewModels** — Clean separation of view logic from Unity components

---

> *This project is a sample demonstrating clean architecture patterns in Unity. Adapt and extend as needed for your own projects.*
