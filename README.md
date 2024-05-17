# BulkyMvc

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Installation](#installation)
4. [Usage](#usage)
5. [API Endpoints](#api-endpoints)
6. [Contributing](#contributing)
7. [License](#license)

## Introduction

**BulkyMvc** is a comprehensive web-based book store application built with ASP.NET MVC. It allows users to browse, search, and purchase books online. Administrators can manage the inventory, categories, and orders through an intuitive administrative interface.

## Features

- User registration and authentication
- Browse and search for books by category, author, or title
- Detailed book descriptions and reviews
- Shopping cart and order management
- Administrative interface for managing books, categories, and orders

## Installation

To install and run BulkyMvc locally, follow these steps:

1. **Clone the repository:**
   ```sh
   git clone https://github.com/ramirocastro1995/Bulky_MVC.git
   cd BulkyMvc

2. **Set up the database:**
   -Create a database for the application.
   -Update the database configuration in appsettings.json.
   
4. **Install dependencies:**
   ```sh
   dotnet restore
   
5. **Run database migrations:**
   ```sh
    dotnet ef database update
## Usage

### User Registration and Authentication

- **Register:** Users can create a new account by providing their name, email, and password.
- **Login:** Registered users can log in using their email and password.
- **Logout:** Users can log out from their account.

### Browsing and Searching Books

- **Browse:** Users can browse books by categories.
- **Search:** Users can search for books by title, author, or keywords.
- **Book Details:** Users can view detailed information about a book, including its description, author, price, and reviews.

### Shopping Cart and Order Management

- **Add to Cart:** Users can add books to their shopping cart.
- **View Cart:** Users can view the contents of their shopping cart.
- **Checkout:** Users can proceed to checkout to place an order.
- **Order History:** Users can view their past orders.

### Administrative Interface

- **Manage Books:** Administrators can add, edit, or delete books.
- **Manage Categories:** Administrators can add, edit, or delete book categories.
- **Manage Orders:** Administrators can view and manage customer orders.
