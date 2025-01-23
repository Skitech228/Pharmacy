# Pharmacy

**Pharmacy** is a desktop application for pharmacy management, developed using WPF and C#. The project follows the MVVM design pattern, implemented with the Prism library, and uses Entity Framework to access an SQL database.

## Features

- **Product Management**: Add, edit, and delete information about medicines.
- **Order Management**: Process, view, and handle customer orders.
- **Client Management**: Store and update client information.

## Technologies

- **C#**: The primary programming language for the project.
- **WPF (Windows Presentation Foundation)**: Used for creating the user interface.
- **MVVM (Model-View-ViewModel)**: A design pattern that ensures separation of presentation logic and business logic.
- **Prism**: A library for implementing MVVM and modularity in WPF applications.
- **Dependency Injection (DI)**: Enhances testability and maintainability of the code.
- **Entity Framework (EF)**: An ORM for interacting with the database.
- **SQL**: Used for application data storage.

## Architecture

The project is divided into several layers:

- **Pharmacy.Domain**: Contains the core entities of the application.
- **Pharmacy.Application**: Responsible for handling operations and communication between layers.
- **Pharmacy.Database**: Manages database access using Entity Framework.
- **Pharmacy.UI**: Implements the user interface using WPF and the MVVM design pattern.
- **Pharmacy.Shared**: Includes shared components and utilities used across different layers.
