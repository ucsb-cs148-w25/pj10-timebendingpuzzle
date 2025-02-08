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

## Example Unit Test

The following example demonstrates how we implemented a unit test for a simple functionality in our game. Assume we have a `RotateDoor` script that handles updating the player's score. 

```csharp
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class RotatingDoorTests
{
    private GameObject doorObject;
    private RotatingDoor rotatingDoor;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject and attach the RotatingDoor component.
        doorObject = new GameObject("TestDoor");
        rotatingDoor = doorObject.AddComponent<RotatingDoor>();
        // In PlayMode tests, the Start() method is automatically called on the first frame update.
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up the GameObject after each test.
        Object.Destroy(doorObject);
    }

    [UnityTest]
    public IEnumerator RotateDoor_SingleRotation_Rotates90Degrees()
    {
        // Wait one frame so that Start() has been executed.
        yield return null;

        float initialAngle = doorObject.transform.eulerAngles.z;
        
        // Trigger the door rotation (should rotate 90° in the forward direction).
        rotatingDoor.RotateDoor();
        
        // Calculate the expected angle after one rotation.
        float expectedAngle = (initialAngle + 90f) % 360f;
        
        // Wait until the door has finished rotating (i.e. until its current angle is near the expected angle).
        yield return new WaitUntil(() =>
            Mathf.Abs(Mathf.DeltaAngle(doorObject.transform.eulerAngles.z, expectedAngle)) < 0.1f);
        
        // Assert that the door's rotation is approximately 90° from the initial rotation.
        Assert.AreEqual(expectedAngle, doorObject.transform.eulerAngles.z, 0.1f, 
            "Door did not rotate by 90 degrees as expected.");
    }

    [UnityTest]
    public IEnumerator RotateDoor_FourRotations_ReturnsToInitialAngle()
    {
        // Wait one frame so that Start() has been executed.
        yield return null;
        
        float initialAngle = doorObject.transform.eulerAngles.z;

        // --- First Rotation: should add 90° ---
        rotatingDoor.RotateDoor();
        float expectedAngle = (initialAngle + 90f) % 360f;
        yield return new WaitUntil(() =>
            Mathf.Abs(Mathf.DeltaAngle(doorObject.transform.eulerAngles.z, expectedAngle)) < 0.1f);
        Assert.AreEqual(expectedAngle, doorObject.transform.eulerAngles.z, 0.1f);

        // --- Second Rotation: should add another 90° (total 180°) ---
        rotatingDoor.RotateDoor();
        expectedAngle = (initialAngle + 180f) % 360f;
        yield return new WaitUntil(() =>
            Mathf.Abs(Mathf.DeltaAngle(doorObject.transform.eulerAngles.z, expectedAngle)) < 0.1f);
        Assert.AreEqual(expectedAngle, doorObject.transform.eulerAngles.z, 0.1f);

        // --- Third Rotation: now the door should rotate in reverse (subtract 90°), back to 90° ---
        rotatingDoor.RotateDoor();
        expectedAngle = (initialAngle + 90f) % 360f;
        yield return new WaitUntil(() =>
            Mathf.Abs(Mathf.DeltaAngle(doorObject.transform.eulerAngles.z, expectedAngle)) < 0.1f);
        Assert.AreEqual(expectedAngle, doorObject.transform.eulerAngles.z, 0.1f);

        // --- Fourth Rotation: subtract another 90° to return to the initial rotation ---
        rotatingDoor.RotateDoor();
        expectedAngle = initialAngle % 360f;  // same as initialAngle
        yield return new WaitUntil(() =>
            Mathf.Abs(Mathf.DeltaAngle(doorObject.transform.eulerAngles.z, expectedAngle)) < 0.1f);
        Assert.AreEqual(expectedAngle, doorObject.transform.eulerAngles.z, 0.1f, 
            "After four rotations, the door did not return to its initial angle.");
    }
}

