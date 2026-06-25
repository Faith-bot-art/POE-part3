# POE-part3
# CYBERBOT - Cybersecurity Awareness Chatbot

## Overview

Cyberbot is an intelligent, GUI-based cybersecurity awareness chatbot that helps users learn about online safety. It features keyword recognition, sentiment detection, memory recall, random responses, voice support, task management, a cybersecurity quiz, and an activity log. Built with C# Windows Forms, this application provides an engaging and interactive way to learn about cybersecurity topics.

---

## Features

### 1. Graphical User Interface (GUI)
- Modern dark-themed design with proper spacing and colors
- ASCII art logo displayed prominently in bold
- Chat display area, input box, and control buttons
- Status bar showing current application state
- Side buttons for Quiz, Tasks, Activity Log, and Speak

### 2. Keyword Recognition
Recognizes and provides detailed guidance on:
- Password Security - Strong password creation and management
- Scam Detection - Identifying and avoiding online scams
- Privacy Protection - Keeping personal data safe
- Phishing Awareness - Spotting fake emails and messages

### 3. Random Responses
- Multiple predefined responses for each topic
- Random selection using lists for varied interactions
- Makes conversations feel natural and engaging

### 4. Conversation Flow
- Handles follow-up questions like "Tell me more" or "Another tip"
- Maintains current topic context
- Continues discussions without restarting the conversation

### 5. Memory and Recall
- Stores user's name when told: "My name is [name]"
- Remembers user's interests: "I am interested in [topic]"
- Personalizes responses using stored information

### 6. Sentiment Detection
Detects and responds appropriately to:
- Worried - Provides reassurance and empathetic responses
- Frustrated - Simplifies information and offers support
- Curious - Gives enthusiastic and encouraging answers

### 7. Task Management
- Add cybersecurity tasks: "Add task - [description]"
- View all tasks: "Show tasks"
- Complete tasks: "Complete task [number]"
- Tasks stored in-memory during session

### 8. Cybersecurity Quiz
- 12+ multiple-choice and true/false questions
- Questions cover passwords, phishing, scams, privacy, and more
- Immediate feedback with explanations
- Score tracking and final results

### 9. Activity Log
- Records all user actions with timestamps
- Logs tasks added, quiz attempts, questions asked
- View recent activity: "Show activity log"

### 10. Voice Support
- Text-to-speech functionality using System.Speech
- Speak button to read text aloud
- Automatic voice responses from Cyberbot

### 11. Error Handling
- Graceful handling of unrecognized inputs
- Professional default responses
- No crashes or unexpected terminations

### 12. Code Optimization
- Dictionaries and lists for efficient response management
- Object-Oriented Programming principles
- Methods for specific functionalities
- Ready for future expansion

---

## Technologies Used

| Technology | Purpose |
|------------|---------|
| C# | Main programming language |
| Windows Forms | GUI framework |
| .NET Framework 4.8 | Runtime environment |
| System.Speech | Text-to-speech functionality |

---

## Project Structure
