# SavaDev.DemoKit.ConsoleEngine

**SavaDev.DemoKit.ConsoleEngine** is a lightweight console demo engine for running interactive, scenario-based demonstrations.

The engine provides a simple menu-driven console UI that allows users to select and execute demo scenarios without requiring dependency injection or a hosting framework.

---

## Purpose

This library is intended for:

* interactive console demos
* educational examples
* experimentation and prototyping
* showcasing libraries or concepts via runnable scenarios

The engine focuses on **clarity, predictability, and minimal setup** rather than flexibility or extensibility through complex infrastructure.

---

## Key characteristics

* No dependency injection
* No hosting abstractions
* No external frameworks
* Explicit scenario wiring
* Simple console-based interaction
* Designed to be embedded into a console application

---

## Basic usage

A typical console application using the engine looks like this:

```csharp
using SavaDev.DemoKit.ConsoleEngine;

await new ConsoleDemoEngine().RunAsync(
    new IConsoleDemoScenario[]
    {
        new HelloScenario(),
        new CountdownScenario()
    });
```

The host application is responsible for:

* defining `Main`
* creating scenario instances
* passing command-line arguments if needed

The engine handles user interaction and scenario execution.

---

## Scenarios

Scenarios are simple classes that implement `IConsoleDemoScenario`.

Each scenario:

* has a display name
* exposes a single asynchronous execution method
* receives a `CancellationToken`

Example:

```csharp
public sealed class HelloScenario : IConsoleDemoScenario
{
    public string Name => "Hello";

    public async Task RunAsync(CancellationToken ct)
    {
        Console.WriteLine("Hello, world!");
        await Task.CompletedTask;
    }
}
```

Scenarios are executed sequentially and can be cancelled by the engine.

---

## Cancellation behavior

* The engine supports Ctrl+C handling
* Cancellation is propagated to the currently running scenario
* Behavior is configurable via `ConsoleDemoOptions`
* When idle, Ctrl+C may optionally terminate the process

Cancellation is treated as an expected execution path, not an error.

---

## Configuration

The engine can be customized using `ConsoleDemoOptions`, including:

* demo title
* menu prompts
* key bindings
* pause behavior
* Ctrl+C handling

All configuration is explicit and optional.

---

## Design goals

* Keep demos easy to read and reason about
* Avoid hidden behavior or implicit wiring
* Minimize required setup code
* Make demo intent obvious in code

This engine is intentionally not designed for production CLI applications.

---

## Intended audience

This library is suitable for:

* library authors creating demo applications
* educators and presenters
* developers exploring concepts via runnable examples

It is not intended to replace full-featured CLI frameworks.

---

## Summary

`SavaDev.DemoKit.ConsoleEngine` provides a small, explicit, and dependency-free way to run interactive console demos composed of simple scenarios.

If the engine feels intentionally minimal — that is by design.