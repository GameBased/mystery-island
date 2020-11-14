# Setting up your development environment

* Install .NET Core 3.1 from the [.NET website](https://dotnet.microsoft.com/download/dotnet-core). Installation instructions for Windows, Mac OS, and Linux [here](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/install).
* If you are a Visual Studio user, install the .NET Core cross-platform development workload Studio workload from the Visual Studio Installer.
* If you are a Visual Studio Code user, install the [C# language extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp).
* Optional: Install the MonoGame Content Builder(MCGB) tool:
    ```
    dotnet tool install --global dotnet-mgcb-editor
    ```
    And optionally register it to open `*.mgcb` files:
    ```
    mgcb-editor --register
    ```
* Optional: Install tiled from [mapeditor.org](https://www.mapeditor.org/).
