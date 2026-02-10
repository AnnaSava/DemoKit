# SavaDev.DemoKit.ConsoleWorker

**SavaDev.DemoKit.ConsoleWorker** is a small **library** for creating a configured console worker "turnkey" with multiple predefined behavior modes.

The library is intended to be embedded into a host console application and started with a single method call. It does **not** define its own `Main` method and does **not** represent a standalone executable by itself.

---

## Purpose

This library exists to provide a ready-to-use console worker that can be
embedded into a thin host application with minimal setup, while still behaving
like a predictable external process.

It is primarily used to:

* demonstrate process lifecycle handling
* simulate long-running workloads
* test cancellation and Ctrl+C behavior
* observe crashes and abnormal termination
* generate continuous console output

The library contains **no business logic** and **no framework dependencies** beyond the .NET runtime.

---

## Basic usage

The base usage pattern is to create a thin console app, reference the library,
and start the worker from `Main`.

Example `YouWorkerApp` entry point:

```csharp
var worker = new ConsoleDemoWorker(options, args);
await worker.RunAsync();
```

A typical invocation from the command line:

```bash
YouWorkerApp --mode run --interval 1000
```

Or with colored output enabled:

```bash
YouWorkerApp --mode spam --interval 50 --color cyan
```

The host application is responsible for:

* defining `Main`
* passing command-line arguments
* deciding how the worker is packaged and executed

The worker logic itself is fully contained in this library, so the host can stay thin.

**Important:** a `ConsoleDemoWorker` instance can be run only once. Reusing the
same instance and calling `RunAsync` again will throw.

---

## Execution model

When invoked, the worker engine:

1. Parses command-line arguments
2. Prints basic startup information (mode, PID)
3. Executes the selected behavior mode
4. Returns a process exit code to the host

Cancellation via **Ctrl+C** is supported automatically. The host is responsible
only for passing arguments and consuming the exit code.

---

## Available modes

### `run`

Runs indefinitely and prints periodic heartbeat messages.

**Behavior:**

* Infinite loop
* Periodic output
* Graceful shutdown on cancellation

**Use cases:**

* Demonstrating long-running processes
* Testing cancellation handling
* Observing process termination

---

### `exit`

Waits for a configurable delay and then exits normally.

**Behavior:**

* Delayed completion
* Exit code `0`

**Use cases:**

* Testing graceful process completion
* Simulating short-lived background work

---

### `crash`

Immediately throws an exception to simulate an abnormal termination.

**Behavior:**

* Unhandled exception
* Non-graceful shutdown

**Use cases:**

* Testing crash detection
* Verifying error handling in process launchers

---

### `spam`

Continuously prints messages at a configured interval.

**Behavior:**

* High-volume console output
* Runs until cancelled

**Use cases:**

* Stress-testing output observers
* Demonstrating buffering and throughput handling

---

### `echo`

Reads user input and prints it back until cancelled.

**Behavior:**

* Interactive input loop
* Echoes each entered line
* Runs until cancelled

**Use cases:**

* Demonstrating interactive console input
* Testing cancellation while awaiting input

---

### `art`

Renders an ASCII art banner until cancelled.

**Behavior:**

* Continuous banner rendering
* Runs until cancelled

**Use cases:**

* Visual demonstration of continuous output
* Testing cancellation with animated console output

## Cancellation behavior

* Pressing **Ctrl+C** requests cancellation
* Continuous modes (`run`, `spam`) stop gracefully
* Cancellation is treated as an expected outcome
* Cancellation results in exit code `0`

---

## Exit codes

| Scenario          | Exit code |
| ----------------- | --------- |
| Normal completion | `0`       |
| Cancellation      | `0`       |
| Crash             | non-zero  |

---

## Intended usage

This library is intended to be used as:

* a ready-to-run demo worker embedded into thin console apps
* a controlled external process launched by process orchestration tools

It is **not** intended for production workloads.

---

## Design principles

* Library-first design
* Explicit behavior over implicit logic
* One responsibility per execution mode
* Predictable and observable output
* Minimal dependencies
* Easy to embed and reuse

---

## Summary

`SavaDev.DemoKit.ConsoleWorker` is a turnkey console worker library that can be embedded into a host console application with minimal effort.

Its sole purpose is to provide well-defined, intentionally simple process behavior for demonstrations, testing, and experimentation.

If the worker feels intentionally boring and repetitive -- that is by design.
