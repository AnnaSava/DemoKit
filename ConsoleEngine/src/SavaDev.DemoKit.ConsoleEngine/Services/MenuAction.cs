namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Describes a custom menu action bound to a single-letter key.
/// </summary>
/// <param name="Key">The input key that triggers the action.</param>
/// <param name="Label">The menu label displayed to the user.</param>
/// <param name="Action">The action to execute when selected.</param>
internal readonly record struct MenuAction(string Key, string Label, Action Action);
