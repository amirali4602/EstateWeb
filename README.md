# EstateWeb

> A .NET-based real estate agency web application featuring a clean architecture.

![GitHub stars](https://img.shields.io/github/stars/amirali4602/EstateWeb?style=for-the-badge&logo=github) ![GitHub forks](https://img.shields.io/github/forks/amirali4602/EstateWeb?style=for-the-badge&logo=github) ![GitHub issues](https://img.shields.io/github/issues/amirali4602/EstateWeb?style=for-the-badge&logo=github) ![Last commit](https://img.shields.io/github/last-commit/amirali4602/EstateWeb?style=for-the-badge&logo=github) ![npm version](https://img.shields.io/npm/v/toastr?style=for-the-badge&logo=npm&logoColor=white) ![npm downloads](https://img.shields.io/npm/dm/toastr?style=for-the-badge&logo=npm&logoColor=white) ![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) ![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=white) ![Node.js](https://img.shields.io/badge/Node.js-339933?style=for-the-badge&logo=nodedotjs&logoColor=white) ![License](https://img.shields.io/badge/license-MIT-green?style=for-the-badge)

## 📑 Table of Contents

- [Description](#description)
- [Key Features](#key-features)
- [Use Cases](#use-cases)
- [Screenshots](#screenshots)
- [Tech Stack](#tech-stack)
- [Quick Start](#quick-start)
- [Key Dependencies](#key-dependencies)
- [Available Scripts](#available-scripts)
- [Project Structure](#project-structure)
- [Development Setup](#development-setup)
- [Contributors](#contributors)
- [Contributing](#contributing)
- [License](#license)

## 📝 Description

EstateWeb is a software application designed for real estate agencies, built with a .NET backend and a JavaScript-supported frontend. The project provides a structured foundation for managing real estate data, utilizing a multi-layered design to separate core business rules from infrastructure and presentation concerns.

## ✨ Key Features

- **📁 Clean Layered Architecture** — Organized into Api, Application, DataAccess, and Domain layers to maintain a strict separation of concerns.
- **💻 Dotnet Backend Implementation** — Utilizes .NET for executing core business workflows and managing data access services.
- **🔔 Client-Side Toast Notifications** — Integrates ToastrJS and jQuery to deliver non-blocking Gnome/Growl style notifications to users.
- **🧪 Dual Test Runners** — Includes pre-configured scripts to execute backend dotnet tests alongside frontend npm test suites.

## 🎯 Use Cases

- Building and launching a real estate agency web application with a structured .NET backend.
- Learning or implementing clean architecture design patterns using the Domain, DataAccess, and Api layer structure.
- Developing interactive client-side components that leverage non-blocking toast notifications in a .NET project.

## 🛠️ Tech Stack

- 🔷 **.NET**
- 🟨 **JavaScript**
- ⬢ **Node.js**

## ⚡ Quick Start

```bash

# 1. Clone the repository
git clone https://github.com/amirali4602/EstateWeb.git

# 2. Install dependencies
npm install

# 3. Start the dev server
dotnet run
```

## 📦 Key Dependencies

```
jquery: >=1.12.0
```

## 🚀 Available Scripts

- **test** — `npm run test`
- **run** — `dotnet run`
- **test** — `dotnet test`

## 🛠️ Development Setup

### Node.js / JavaScript
1. Install Node.js (v18+ recommended)
2. Install dependencies: `npm install` (or `yarn` / `pnpm install` / `bun install`)
3. Start the dev server: see the **Quick Start** above

### .NET
1. Install the [.NET SDK](https://dotnet.microsoft.com/)
2. `dotnet restore && dotnet run`

## 👥 Contributors

Thanks to everyone who has contributed to this project:

<p align="left">
<a href="https://github.com/amirali4602" title="amirali4602"><img src="https://avatars.githubusercontent.com/u/106028739?v=4&s=64" width="64" height="64" alt="amirali4602" style="border-radius:50%" /></a>
</p>

[See the full list of contributors →](https://github.com/amirali4602/EstateWeb/graphs/contributors)

## 👥 Contributing

Contributions are welcome! Here's the standard flow:

1. **Fork** the repository
2. **Clone** your fork: `git clone https://github.com/amirali4602/EstateWeb.git`
3. **Branch**: `git checkout -b feature/your-feature`
4. **Commit**: `git commit -m 'feat: add some feature'`
5. **Push**: `git push origin feature/your-feature`
6. **Open** a pull request

Please follow the existing code style and include tests for new behavior where applicable.

## 📜 License

This project is licensed under the **MIT** License.

---
*This README was generated with ❤️ by [ReadmeBuddy](https://readmebuddy.com)*
