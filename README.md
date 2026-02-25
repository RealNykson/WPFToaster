# WPF Toaster

A lightweight toast notification system for WPF, inspired by Sonner. Supports stacking, animations, promise-based toasts, and configurable positioning.

## Setup

1. Add the `Essentials`, `ViewModels`, `Views`, `Converters`, `Behaviors`, and `Utilities` folders to your project.
2. Merge `Resources/Theme.xaml` and `Resources/Styles.xaml` into your `App.xaml` resource dictionaries.
3. Wrap your root content in `ToastHost`:

```xml
<views:ToastHost Position="BottomRight">
    <!-- Your application content -->
</views:ToastHost>
```

## Usage

```csharp
// Simple toast
Toaster.Toast("File saved", ToastStatus.Success);

// Toast with title and description
Toaster.Toast("Error", "Connection timed out", ToastStatus.Error);

// Promise toast (shows spinner, resolves to success or error)
await Toaster.Promise(
    async () => await SomeLongRunningTask(),
    loading: "Processing...",
    success: "Done!",
    error: "Something went wrong"
);

// Generic promise toast with result
var result = await Toaster.Promise(
    async () => await FetchDataAsync(),
    loading: "Fetching...",
    success: data => $"Loaded {data.Name}",
    error: "Failed to fetch"
);
```

## Positions

`TopLeft` · `TopCenter` · `TopRight` · `BottomLeft` · `BottomCenter` · `BottomRight`

## Theming

Edit `Resources/Theme.xaml` to adjust colors. Toast accent brushes reference Color keys that can be swapped for light/dark mode support.

## License

MIT
