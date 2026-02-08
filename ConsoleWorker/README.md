# SavaDev.DemoKit.ConsoleWorker

**SavaDev.DemoKit.ConsoleWorker** is a turnkey library for building configured
console workers with predictable behavior. It is designed to be embedded into
thin host applications and started with a single call.

The package itself contains no `Main` method and is not a standalone
executable.

---

## Library

The library provides:

* A ready-to-run worker engine with multiple execution modes
* A simple CLI argument parser
* Built-in Ctrl+C handling
* Configurable output and message templates

It is intended for demos, testing, and process orchestration experiments.

---

## Tests

The test project focuses on:

* Argument parsing and validation
* Mode execution behavior
* Cancellation handling and output

---

## Demo

The demo app is a thin console host that uses `SavaDev.DemoKit.ConsoleEngine`
to show scenarios running the worker in-process.

To run the demo:

```bash
dotnet run --project ConsoleWorker/demo/SavaDev.DemoKit.ConsoleWorker.Demo
```

---

## Notes

* A `ConsoleDemoWorker` instance can be run only once.
* The library is intentionally simple and non-production by design.
