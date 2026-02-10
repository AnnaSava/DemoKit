# SavaDev.DemoKit.ConsoleWorker.Demo

This project is a thin console application that showcases the
`SavaDev.DemoKit.ConsoleWorker` library through interactive demo scenarios.
It uses `SavaDev.DemoKit.ConsoleEngine` to present a menu and execute
scenarios in-process.

---

## What it demonstrates

The demo app runs the worker library in different modes to show:

* Normal long-running output (`run`)
* Graceful exit (`exit`)
* Intentional crash (`crash`)
* High-frequency output (`spam`)
* Interactive echo (`echo`)
* ASCII art banner (`art`)
* Custom output formatting and color options

---

## How to run

From the repository root:

```bash
dotnet run --project ConsoleWorker/demo/SavaDev.DemoKit.ConsoleWorker.Demo
```

Then select a scenario from the menu.

---

## Notes

* This demo runs the worker **in-process** (no external process launch).
* The worker handles Ctrl+C internally for long-running modes.
* The app itself is intentionally thin: it wires scenarios and starts
  the demo engine.
