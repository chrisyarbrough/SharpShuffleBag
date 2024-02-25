# Source Development

The top-level SharpShuffleBag.sln is the main solution which includes projects for the core source, samples and tests.

When building this solution (or the main SharpShuffleBag csproj), the source files are automatically copied to the Unity
project to distribute them as a Unity package. This was chosen to keep the build and publish flow simple, but
remember to only make changes to the main source files and then let the build process copy them to Unity.

Run these shell commands from the repository root:

```bash
dotnet restore
```

```bash
dotnet test
```

```bash
dotnet build
```

Or build the solution with the `Default` configuration in e.g. JetBrains Rider.
For simplicity, there are no debug/release variants.

When editing build properties, take note of the `SharedBuildProperties.props` file in the root directory.

## Code Style

For JetBrains Rider, see the `SharpShuffleBag.sln.DotSettings` file, for other IDE's see the `.editorconfig` file.
When in doubt, try to align with the existing code.

Avoid reformatting existing code when contributing new features.