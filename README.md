# BAT-Manager 🦇

[![GitHub Release](https://img.shields.io/github/v/release/vincenzomy03/BAT-Manager?color=blue&logo=github)](https://github.com/vincenzomy03/BAT-Manager/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows%2010%20%7C%2011-0078D6?logo=windows)](https://github.com/vincenzomy03/BAT-Manager)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![AI-Assisted](https://img.shields.io/badge/AI--Assisted-Google%20Antigravity-7B1FA2?logo=google&logoColor=white)](https://github.com/vincenzomy03/BAT-Manager)

A lightweight, portable Windows desktop application designed to manage, create, organize, edit, and execute Batch scripts (`.bat` / `.cmd`) through a clean and intuitive interface.

Built with **C# Windows Forms** targeting **.NET Framework 4.8**, BAT-Manager runs out-of-the-box on any modern Windows 10 or Windows 11 installation without requiring external runtimes, dependencies, or installers.

---

## 📦 Downloads

Pre-built binaries and archives are available in the [**Releases**](https://github.com/vincenzomy03/BAT-Manager/releases) section:

| Asset | Description |
| :--- | :--- |
| **`BAT-Manager.exe`** | **Standalone executable**. Download and place anywhere (e.g. Desktop, Start Menu, or USB drive). No installer required. |
| **`BAT-Manager-vX.Y.Z-windows.zip`** | **Full package**. Contains the standalone executable, pre-configured sample scripts, `README.md`, and license. |
| **`SHA256SUMS.txt`** | Cryptographic verification checksums for all release binaries. |

---

## ✨ Features

- **Embedded Metadata System**: Transparently saves human-readable metadata in standard batch header comments:
  ```bat
  REM @name: Database Backup
  REM @description: Creates a timestamped dump of local databases
  REM @runAsAdmin: true
  ```
  The built-in editor presents only your script commands, keeping metadata tags managed behind the scenes.
- **Configurable Administrator Privileges (UAC Elevation)**: Check the `🛡️ Always run as Administrator` box in the script editor to automatically elevate via Windows UAC upon execution. The main window clearly flags elevated scripts with `🛡️ Yes`.
- **Interactive Multi-Column Sorting**: Click any column header (`Script Name`, `Admin`, `Description`, `File`, `Last Modified`) to sort ascending or descending with visual arrow indicators (`▲` / `▼`). Clicking **Admin** immediately brings all elevated scripts to the top.
- **Drag & Drop File Import**: Drag any `.bat` or `.cmd` file from Windows Explorer or your Desktop into the application window. BAT-Manager automatically extracts existing comments and opens the editor dialog so you can adjust the script name, description, filename, and admin rights before saving. In case of filename collisions, a numerical suffix is safely appended (`script-1.bat`).
- **Clean Desktop & Centralized Storage**: Scripts are stored in `%LOCALAPPDATA%\BAT-Manager\scripts`. You can move or copy `BAT-Manager.exe` directly to your Desktop without creating any clutter or empty directories on your Desktop. Quick access to the scripts folder is always available via the **📁 Open Folder** button.
- **Strict Windows File System Validation**:
  - Blocks forbidden file system characters: `\ / : * ? " < > |`.
  - Blocks reserved Windows device names: `CON`, `PRN`, `AUX`, `NUL`, `COM1-COM9`, `LPT1-LPT9`.
  - Automatically generates safe file slug names from user titles (`My Backup Script` ➔ `My-Backup-Script.bat`).
- **Modern Encoding & Resilient Fallback**:
  - Saves in **UTF-8 (without BOM)** for clean Git diffs and cross-platform compatibility.
  - Automatically detects and falls back to **CodePage 850 (MS-DOS OEM)** or ANSI when reading legacy batch files with accented characters.
- **Execution & Testing**:
  - **▶️ Run**: Executes the script in a console window (`cmd.exe /c`, closes automatically upon completion) or directly by double-clicking it in the list.
  - **🛡️ Run as Admin**: Elevates the script with administrative privileges on demand.
  - **▶️ Test Script**: Lets you preview and run the script directly from the editor window before committing changes to disk.
- **Real-Time Search**: Instant search bar filtering by name, description, filename, or batch command contents.
- **High-DPI Support**: Native `PerMonitorV2` DPI scaling configured in `app.manifest` and `App.config` for sharp, crisp visuals on 1080p, 1440p, and 4K displays.

---

## 🏛️ Project Architecture

BAT-Manager adheres strictly to the **Separation of Concerns (SoC)** principle, isolating business logic, parsing, and file I/O from the Windows Forms graphical layer:

```
BAT-Manager/
├── BAT-Manager.sln                     # Visual Studio solution
├── README.md                           # Documentation (English)
├── LICENSE                             # MIT License
├── package.ps1                         # Local release packager & build script
│
├── .github/
│   └── workflows/
│       └── release.yml                 # Automated GitHub Actions release pipeline
│
├── BAT-Manager/                        # Main Windows Forms project (.NET 4.8)
│   ├── BAT-Manager.csproj
│   ├── App.config                      # High-DPI & runtime configuration
│   ├── app.manifest                    # Windows application manifest (PerMonitorV2)
│   ├── Program.cs                      # Application entry point
│   │
│   ├── Models/
│   │   └── BatScript.cs                # Core data model (Name, Description, Content, RunAsAdmin, etc.)
│   │
│   ├── Services/
│   │   ├── BatParser.cs                # Metadata parser (@name, @description, @runAsAdmin)
│   │   ├── ScriptRepository.cs         # File I/O, AppData persistence, naming validation & encoding
│   │   └── ScriptRunner.cs             # Process launcher (cmd.exe /c, UAC elevation)
│   │
│   ├── Forms/
│   │   ├── MainForm.cs                 # Main dashboard with ListView, ToolStrip, search & sort
│   │   ├── MainForm.Designer.cs
│   │   ├── EditorForm.cs               # Dedicated script editor dialog with Consolas monospace font
│   │   └── EditorForm.Designer.cs
│   │
│   ├── Resources/
│   │   ├── app.ico                     # Multi-resolution embedded application icon
│   │   └── app_icon.png
│   │
│   └── scripts/                        # Sample scripts bundled with the application
│       ├── info_sistema.bat            # Hardware and OS diagnostics
│       ├── ping_dns.bat                # DNS latency and connectivity test
│       └── pulisci_temp.bat            # Windows temporary files cleanup (admin)
│
└── BAT-Manager.Tests/                  # Automated unit tests project
    ├── BAT-Manager.Tests.csproj
    ├── BatParserTests.cs               # Metadata parsing & content separation tests
    ├── ScriptRepositoryTests.cs        # Windows name validation, slug generation & CRUD tests
    └── Program.cs                      # Console test runner
```

---

## 🛠️ Building From Source

### Prerequisites
- Windows 10 or 11
- [.NET Framework 4.8 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/net48) (or Visual Studio 2022 with the *"Desktop development with .NET"* workload)

### Option 1: Using Visual Studio 2022
1. Clone the repository:
   ```cmd
   git clone https://github.com/vincenzomy03/BAT-Manager.git
   cd BAT-Manager
   ```
2. Double-click `BAT-Manager.sln` to open the solution in Visual Studio.
3. Select **Release** and **Any CPU** in the configuration toolbar.
4. Press **Ctrl+Shift+B** to build or **F5** to run.

### Option 2: Using the Command Line (MSBuild)
You can compile directly via PowerShell or Command Prompt using Microsoft MSBuild:

```powershell
& "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" "BAT-Manager.sln" /t:Clean,Build /p:Configuration=Release
```

The compiled standalone executable will be located at:
```
BAT-Manager\bin\Release\BAT-Manager.exe
```

---

## 🧪 Running Automated Tests

Run the test suite using the pre-compiled test runner or build it with MSBuild:

```powershell
& ".\BAT-Manager.Tests\bin\Release\BAT-Manager.Tests.exe"
```

Expected output:
```text
==================================================
       BAT-Manager - Unit Tests Runner            
==================================================

[1/2] Testing BatParser...
  [PASS] All BatParser tests passed!

[2/2] Testing ScriptRepository...
  [PASS] All ScriptRepository tests passed!

>> SUCCESS: All unit tests passed successfully! <<
```

---

## 🚀 Creating & Publishing a GitHub Release

### Automated via Git Tags (Recommended)
This repository includes a GitHub Actions workflow (`.github/workflows/release.yml`). When you tag a commit and push it to GitHub, GitHub automatically compiles the release, verifies unit tests, packages both the standalone `.exe` and the `.zip` bundle, and creates an official GitHub Release:

```powershell
# 1. Commit and push your changes
git add .
git commit -m "Prepare v1.0.0 release"
git push origin main

# 2. Create and push a version tag
git tag v1.0.0
git push origin v1.0.0
```

GitHub Actions will trigger, build the binaries on a clean Windows runner, and attach:
- `BAT-Manager.exe`
- `BAT-Manager-v1.0.0-windows.zip`
- `SHA256SUMS.txt`

### Manual Packaging via PowerShell
If you prefer to generate and inspect release assets locally before publishing:

```powershell
.\package.ps1 -Version "1.0.0"
```

This script automatically builds the release binary, runs tests, and packages all release files inside the `dist/` directory ready to be uploaded to GitHub:
1. Go to your repository on GitHub: `https://github.com/vincenzomy03/BAT-Manager/releases`.
2. Click **"Draft a new release"**.
3. Choose or create a tag (e.g. `v1.0.0`).
4. Drag and drop the files from your local `dist/` folder into the binaries attachment box.
5. Click **"Publish release"**.

---

## 🤖 AI Transparency & Provenance

In adherence to open-source transparency best practices and modern AI disclosure recommendations, this software was crafted through collaborative AI pair programming (**Google Antigravity**).

- **Human Oversight**: Architectural patterns, Windows Forms UX design, strict file system validation logic, and automated test suites were guided, reviewed, and validated under human supervision.
- **Code Integrity**: The application logic and codebase are completely open source under the MIT License and verified against Microsoft MSBuild 4.8.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE) — feel free to use, modify, and distribute it.
