# 💼 Naseem Ganji - C# Project Portfolio

A collection of C# applications demonstrating ASP.NET MVC, Entity Framework, OOP principles, and console application development.

---

## 🚗 Project 1: AutoShield Car Insurance (ASP.NET MVC)

**Tech Stack:** ASP.NET MVC 5 | Entity Framework 6 | SQL Server | Bootstrap  
**Repository:** [View on GitHub](https://github.com/naseemganji/ASP.NET-MVC-Entity-Framework-Assignment---CarInsurance)

### Overview
A full-stack web application for managing car insurance quotes with automated pricing, CRUD operations, and an admin dashboard.

### Key Features
- 🤖 **Auto Quote Calculator** - Calculates premiums based on age, vehicle, DUI, speeding tickets, and coverage type
- 📊 **Admin Dashboard** - View all quotes with statistics (total, average, revenue)
- ✉️ **Contact System** - Database-integrated contact form
- 🔄 **CRUD Operations** - Complete Create, Read, Update, Delete functionality
- 🎨 **Modern UI** - Responsive design with gradient styling

### Quote Logic
Base: $50 | Age: +$25-$100 | Vehicle Year: +$25 | Porsche: +$25-$50 | DUI: +25% | Full Coverage: +50%

**Example:** 22-year-old, 2020 Honda, 2 tickets, full coverage = $202.50/month

### Pages
- **Home** - Landing page with hero section and features
- **Contact** - Form with quote CTA and contact info
- **Quote Request** - Customer input form (auto-calculates quote)
- **Admin Dashboard** - View all quotes with statistics
- **CRUD Views** - Index, Edit, Details, Delete

---

## 🃏 Project 2: BlackJack Console App (C# Console)

**Tech Stack:** C# | .NET Framework 4.8 | OOP Principles | Console Application  
**Repository:** [View on GitHub](#) *(Add your BlackJack repo link)*

### Overview
A console-based BlackJack card game demonstrating object-oriented programming, inheritance, polymorphism, and game logic implementation.

### Key Features
- 🎮 **Full Game Logic** - Complete BlackJack rules (hit, stand, bust, blackjack)
- 🃏 **Card Management** - Deck shuffling, dealing, and card value calculation
- 👥 **Player vs Dealer** - Simulates casino-style gameplay
- 🎯 **OOP Design** - Classes for Card, Deck, Player, Dealer, Game
- 🏆 **Win/Loss Tracking** - Score keeping and game statistics
- ♠️ **Ace Handling** - Dynamic value calculation (1 or 11)

### Technical Highlights
- **Inheritance** - Base `Player` class, derived `Dealer` class
- **Polymorphism** - Overridden methods for dealer AI behavior
- **Encapsulation** - Private fields with public properties
- **Exception Handling** - Try-catch blocks for invalid inputs
- **LINQ** - Card shuffling and hand calculations
- **Collections** - Lists for deck and player hands

### Game Flow
1. Player places bet
2. Cards dealt (2 to player, 2 to dealer - 1 hidden)
3. Player decides: Hit or Stand
4. Dealer reveals and plays (must hit on 16 or less)
5. Winner determined and payouts processed

### Sample Code Snippet
