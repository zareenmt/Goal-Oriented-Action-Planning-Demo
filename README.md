**<h1 align = "left">Introduction</h1>**

This project demonstrates an implementation of Goal Oriented Action Planning (GOAP) for controlling Non-Player Characters (NPCs) in a game environment. GOAP enables NPCs to dynamically create action plans to achieve specified goals based on current world states.

**Example of the system in action**

https://github.com/user-attachments/assets/91b3a49e-c900-4f64-900c-ad565daf9e7c

**<h2 align = "left">System Architecture</h2>**

**<h3 align = "left">Finite State Machine</h3>**

The GOAP system operates within a Finite State Machine (FSM) with three primary states:

**Idle State** : The system evaluates the current world state and generates an action plan for the NPC

**MoveTo State** : Activated when an action requires the NPC to be within range of a target

**PerformAction State** : Executes the planned action once the NPC is within the required range

**<h3 align = "left">Core Components</h3>**

**IGoap Interface**

NPCs implement the IGoap interface to receive and process world state data. This information is crucial for the planning system to make informed decisions about possible actions and their outcomes.

**Planner Class**

The planning system utilizes the A* pathfinding algorithm to construct a tree of possible actions. This tree is traversed to find the most efficient sequence of actions that will achieve the desired goal state.

**GOAP Agent**

This script serves as the brain of the NPC, managing:

- Current state interpretation
  
- FSM state transitions
  
- Integration with the planner for decision-making

**GOAP Action**

An abstract class that defines the structure that all actions must follow in the system. Individual actions inherit from this class and implement their specific logic and requirements.



