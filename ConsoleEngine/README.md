# SavaDev.DemoKit.ConsoleEngine

A lightweight interactive console demo engine for running and showcasing scenarios.

`SavaDev.DemoKit.ConsoleEngine` provides a small, focused infrastructure for building interactive console demos:
a menu, scenario execution, Ctrl+C handling, and a predictable execution flow -- without turning into a framework.

It is designed primarily for **demo applications**, **playgrounds**, and **documentation-driven showcases** for libraries in the SavaDev ecosystem, but can be reused independently.

## Status

Current version: **0.1.0**

This is the initial public release of `SavaDev.DemoKit.ConsoleEngine`.
The API is stable within the 0.x series, but may evolve based on feedback.

---

## Key features

* Interactive console menu with numbered scenarios
* Runs a single scenario, all scenarios, or graceful exit
* Built-in Ctrl+C handling to cancel running scenarios or exit when idle
* Sequential scenario execution
* Clear separation between demo engine and demo scenarios
* Minimal API surface
* Fully async and cancellation-aware
* Covered by automated tests

---

## Core concepts

### ConsoleDemoEngine

The central orchestrator that:

* renders the menu
* handles user input
* manages scenario execution
* coordinates cancellation behavior

```csharp
var engine = new ConsoleDemoEngine();
await engine.RunAsync(scenarios);
```

### IConsoleDemoScenario

Each demo scenario is a small, self-contained unit:

```csharp
public interface IConsoleDemoScenario
{
    string Name { get; }

    Task RunAsync(CancellationToken ct);
}
```

Scenarios:

* declare only their name and execution logic
* receive a `CancellationToken`
* do not know about menus, input, or other scenarios

---

## Basic usage

```csharp
var scenarios = new IConsoleDemoScenario[]
{
    new FirstScenario(),
    new SecondScenario(),
    new CancellationScenario()
};

var engine = new ConsoleDemoEngine();

await engine.RunAsync(scenarios);
```

The engine will:

1. Render a menu
2. Ask the user for input
3. Execute the selected scenario(s)
4. Return to the menu until exit is requested

---

## Keyboard behavior

* **Number (1, 2, 3, ...)** — run a specific scenario
* **Run all key** (default: `A`, case-insensitive) — run all scenarios sequentially
* **Quit key** (default: `Q`, case-insensitive) — exit the demo
* **Ctrl+C**: cancels the currently running scenario or exits the application when idle (configurable)

---

## Configuration

The engine behavior can be customized via `ConsoleDemoOptions`:

```csharp
var options = new ConsoleDemoOptions
{
    Title = "SavaDev Demo",
    PauseAfterScenarios = true,
    HandleCancelKeyPress = true,
    ExitOnCancelWhenIdle = true
};

var engine = new ConsoleDemoEngine(options);
```

Available options include:

* menu labels and prompts
* header and separator formatting
* pause behavior
* Ctrl+C handling strategy

All options have sensible defaults.

---

## Testing

The project includes automated tests that verify the core behavior of the demo engine, including:

* menu input handling (single selection, run all, quit)
* scenario execution flow and ordering
* console output for headers, footers, and user-facing messages
* cancellation behavior via Ctrl+C
* handling of invalid input and unexpected scenario errors

The tests focus on **behavioral correctness of the interactive loop**
rather than on exact console rendering details.

The engine is designed to be testable without intrusive console I/O hacks,
using controlled boundaries and predictable execution flow.

---

## Design goals

* **Simple, not clever**
* **Explicit control flow**
* **No hidden magic**
* **Demo-friendly defaults**
* **No dependency on specific libraries**

This is intentionally **not** a general-purpose CLI framework.
It is a small, predictable tool for demos.

---

## Package structure

Typical repository layout:

```
src/
  SavaDev.DemoKit.ConsoleEngine/
tests/
  SavaDev.DemoKit.ConsoleEngine.Tests/
```

Demo applications that use this engine usually live in **separate projects**.

---

## Scope and stability

This library is intentionally minimal.

* The public API is small
* Breaking changes may occur while the version is `0.x`
* Once stabilized, the API surface is expected to remain stable

Future extensions (if any) will remain additive and conservative.

---

## AI-assisted & cat-assisted development

This library was developed with the assistance of AI tools and under close supervision of a Siamese cat who actively participated in:

* keyboard walking
* random code review
* morale boosting

All architectural decisions were made by a human.

---

## License

This project is licensed under the **Business Source License (BSL)** with a planned automatic transition to **MIT**.

See the full license text in the `LICENSE` file.
