# Testing Documentation

This document outlines our testing strategy for our Unity 2D game project. We have experimented with the Unity Test Framework (UTF) to implement unit tests that help ensure the quality and correctness of our game code.

---

## Testing Library/Framework

**Unity Test Framework (UTF)**
- **Overview:**  
  The Unity Test Framework is integrated into the Unity Editor and is built on top of NUnit. It supports both Edit Mode tests (for testing non-runtime logic) and Play Mode tests (for testing runtime behaviors).

- **Why We Chose It:**  
  - Seamless integration with Unity.
  - Supports both Edit and Play Mode tests.
  - Provides a robust set of assertions and a user-friendly test runner.
  - Enables automated testing, which can be integrated with our CI/CD pipeline.

---

## Testing Approach

### Types of Tests

1. **Edit Mode Tests:**  
   - **Purpose:** Test non-runtime logic, utility functions, and isolated code components.
   - **Location:** Typically located in the `Assets/Tests/EditMode` folder.

2. **Play Mode Tests:**  
   - **Purpose:** Test gameplay interactions, component behaviors, and dynamic game object interactions during runtime.
   - **Location:** Typically located in the `Assets/Tests/PlayMode` folder.

### Implementation Process

1. **Setup Testing Environment:**  
   - Create a `Tests` folder in the Unity project (e.g., `Assets/Tests`).
   - Use the Unity Package Manager to ensure the Unity Test Framework is installed.

2. **Writing Unit Tests:**  
   - Identify critical components in our game (e.g., mechanics, score management, etc.).
   - Use NUnit attributes such as `[Test]`, `[SetUp]`, and `[TearDown]` to structure tests.
   - Write tests in C# files within the appropriate test folders (Edit Mode or Play Mode).

3. **Running Tests:**  
   - Open Unity and go to `Window > General > Test Runner`.
   - Choose between Edit Mode and Play Mode tests.
   - Run the tests and review the results within the Test Runner window.

---

## 1. Installing the Unity Test Framework
Before writing tests, we installed the **Unity Test Framework (UTF)** using **Package Manager**.

### Steps to Install:
1. Open **Unity**.
2. Go to **Window > Package Manager**.
3. Select **Unity Registry** and search for **Unity Test Framework**.
4. Click **Install**.

✅ After installation, **we were able to run Edit Mode and Play Mode tests** inside the **Test Runner**.

---

## 2. Setting Up a Test Scene
We created a **separate scene** (`TestScene`) specifically for testing purposes.

### Steps to Create and Use the Test Scene:
1. Open **Unity** and go to **File > New Scene**.
2. Save it as **`TestScene.unity`** inside the `Assets/Scenes/` folder.
3. **Add a Player GameObject** (same as in the main game).
4. Go to **File > Build Settings** and **add `TestScene` to the build settings**.
5. Write tests to verify that the **player exists in the scene**.

---

## 3. Unit Testing: Verifying the Player Exists
The first test we implemented was to **verify that the player exists in the scene**.

### Testing Library Used:
- **Unity Test Framework (UTF)**
- **NUnit (Assertions)**

### Test: Checking if Player Exists
```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerExistenceTest
{
    private GameObject player;

    [SetUp]
    public void Setup()
    {
        // Load the test scene before running the test
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator Player_SpawnsCorrectly()
    {
        yield return new WaitForSeconds(1); // Allow scene to load

        // Find the player in the scene
        player = GameObject.FindWithTag("Player");

        // Check if the player exists
        Assert.IsNotNull(player, "Player object was not found in the test scene.");
    }
}
```
### Test Result: Success

<img width="416" alt="Screenshot 2025-02-21 at 9 02 27 PM" src="https://github.com/user-attachments/assets/15d71c59-b05d-4b97-81e8-3286e95336ea" />


## Plans for future

1. Continue testing critical mechanics like scoring system and UI updates.
2. Avoid testing physics-heavy interactions that are better suited for integration or E2E tests.
3. Refactor existing tests to improve efficiency and coverage.

##  Implementing End-to-End Testing 
### Testing Library Used: 
1. Unity Test Framework (UTF) (Play Mode Tests)
### Tested Components:
1. Player exists
2. Player movement across the scene

##  Plans for Higher-Level Testing Going Forward
1. Continue using E2E tests for player-object interactions.
2. No immediate plan for full BDD tests due to time constraints, but Play Mode tests will be refined.
3. Exploring CI/CD integration for automated testing workflows.




