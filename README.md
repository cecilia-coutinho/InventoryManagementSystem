<h1 align="center" style="margin: 1rem;">Inventory Management System 💻📦🏷️📈 </h1>

<p align="center" style="font-size: 1rem; margin-top: 2rem;">
by <a href="https://github.com/Cecilia-Coutinho">Cecilia Coutinho</a>
</p>

<div align="center" style="font-size: 2rem; margin-top: 3rem; margin-bottom: 2rem;">
🔍 Overview
</div>

<div style="text-align: left; font-size: 1rem; margin-top: 2rem; margin-bottom: 2rem;">
<p>
This project is a simplified Inventory Management System designed to be a functional and user-friendly application. It follows a microservice architecture, covering both the backend and frontend.
</p>
<p>
<b style="color: #4caf50; font-size:16px;">Backend</b>: Implemented using .NET 8 with ASP.NET Core, SQL Server, and Entity Framework.

<b style="color: #4caf50; font-size:16px;">Frontend</b>: Built with Vue 3, Nuxt 3, and Vuetify 3.

</p>
</div>

## 💻 Technology Stack

- .NET 8

- ASP.NET Core Web API

- C#

- SQL Server Management Studio (SSMS)

- Entity Framework

- Vue 3

- Nuxt 3

- Vuetify 3

- Visual Studio

- GitHub

## 📋 Additional Information

### 🗃️ SQL Design

The SQL design follows a relational database model, with tables representing entities such as Contacts, Customers, Inventories, Products, ProductCategories, Suppliers, Product Suppliers, Orders and OrderDetails.

![ER Model](/Frontend/inventory-system-client/public/assets/images/diagram-inventory-system-db.png)

### 🎨UI Design

The UI was created to be easy to use and include essential features.

- Dashboard:

![Dashboard](/Frontend/inventory-system-client/public/assets/images/dashboard.png)

\*Menu

![Menu](/Frontend/inventory-system-client/public/assets/images/menu.png)

- Pages:

![Produtos](/Frontend/inventory-system-client/public/assets/images/produtos.png)

![Categorias](/Frontend/inventory-system-client/public/assets/images/categorias.png)

### 🏗️ Code Structure

On the server-side, it includes Models and DTOs for managing data, along with a Generic Repository pattern to handle data access. This project also implements a Specification Pattern for condition rules. Custom Action Filters help validate the data before processing and Controllers manage how data flows in and out of the application, connecting to the database through the AppContext. Everything is organized in folders for easy navigation and maintenance.

The client-side handles creating a user-friendly interface and sending requests to the server. It follows Nuxt3 guidelines, organizing components, pages, and services into separate folders, which makes the code easier to manage and expand.

### 📦📜 Repository and Specification Pattern

The repository pattern is implemented to manage data access and abstract the data source, promoting separation of concerns and providing a structured method for handling data. Interfaces are created for each repository, enabling interaction with the generic repository. These interfaces incorporate specific tasks, leveraging polymorphism to ensure flexibility and clarity. Additionally, some tasks are implemented as non-virtual methods, serving as new functionalities that call upon the base methods, rather than being directly overridden.

The Specification Pattern is implemented to ensure that the data access layer remains independent from the business logic layer within this project. By implementing this pattern, the project maintains easier maintainability and testing capabilities, by doing so, it stops data access and application logic from getting mixed up.

These patterns ensure organized data management and maintain separation between data access and application logic in the project.

### 🔍 Action Filters

Action filters are implemented to perform additional logic before or after an action method is executed. This allows for common logic to be applied across multiple controllers and action methods. The implementation needs to be refactored to improve functionalities and make it more robust.

### 🧩 Components

Th e client side has components that are reusable pieces of code that can be shared across different parts of the application. They help to keep the code organized and make it easier to maintain and update the application.

Refactoring is still needed to improve and use components that can help to reduce code duplication and improve the overall structure of the application.

### ✅ Validation

Validation is implemented to prevent invalid data from being stored in the database. The validation model is implemented using Data Annotations and are used to define validation rules in the DTOs classes.

### 🛠️ Future Improvements

There's a feature called the required modifier in C# that could be used to ensure that certain properties always have a value when a new object is created. This project doesn't use this feature yet, but it might be considered for future improvements. It's helpful because it provides compile-time checking, which means errors are caught before running the program. This is different from data annotations, which are checked at runtime. Compile-time checking offers immediate feedback during development, reducing the chances of shipping code with missing property assignments.

The backend is missing the implementation of Orders and OrderDetails Repositories and Controllers, something that will be considered for implementation later on.

The frontend also is not complete and could be improved to enhance the Minimal Viable Product for this application.

This project doesn't have a login feature, and other potential improvements may include implementing access levels based on user roles and integrating with a MicroService responsible for managing Application User authorization.

### 🏁 Conclusion

In conclusion, this project utilizes ASP.NET on the server side and Vue3 with Nuxt 3 and Vuetify 3 on the client side to build an interactive web application. The design includes separate server-side controllers and implements repository and Specification patterns. Additionally, components are employed on the client side to facilitate data management and coding.

There's room for improvements, particularly in adding missing features in upcoming stages.
