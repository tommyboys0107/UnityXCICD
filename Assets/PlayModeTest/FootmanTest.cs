using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class FootmanTest : InputTestFixture
{
    private Mouse mouse;
    private Joystick joystick;
    
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        mouse = InputSystem.AddDevice<Mouse>();
        joystick = InputSystem.AddDevice<Joystick>();
    }

    [UnityTest]
    public IEnumerator FootmanCanMoveHorizontally([Values(-1, 1)]int direction)
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("GameFeel");
        yield return new WaitForSeconds(0.5f);
        var footmanTransform = GameObject.FindGameObjectWithTag("Player").transform;
        var originalPosition = footmanTransform.position;
        
        // Act
        Move(joystick.stick, Vector2.right * direction, null, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        // Assert
        Assert.AreNotSame(originalPosition.x, footmanTransform.position.x);
    }
    
    [UnityTest]
    public IEnumerator FootmanCanMoveVertically([Values(-1, 1)]int direction)
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("GameFeel");
        yield return new WaitForSeconds(0.5f);
        var footmanTransform = GameObject.FindGameObjectWithTag("Player").transform;
        var originalPosition = footmanTransform.position;
        
        // Act
        Move(joystick.stick, Vector2.up * direction, null, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        // Assert
        Assert.AreNotSame(originalPosition.z, footmanTransform.position.z);
    }

    [UnityTest]
    public IEnumerator FootmanCanPrimaryAttack()
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("GameFeel");
        var footmanAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<FootmanAnimator>();
        
        // Act
        Press(mouse.leftButton);
        yield return new WaitForSeconds(0.3f);
        
        // Assert
        Assert.IsTrue(footmanAnimator.IsInAnimationState("LightAttack") || footmanAnimator.IsNextAnimationState("LightAttack"));
    }
    
    [UnityTest]
    public IEnumerator FootmanCanPrimaryAttackHitEnemy()
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("GameFeel");
        var zombie = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Zombie>();
        var originalHealthPoint = zombie.healthPoint;
        
        // Act
        yield return new WaitForSeconds(1.5f);
        Press(mouse.leftButton);
        yield return new WaitForSeconds(1.0f);
        
        // Assert
        Assert.Less(zombie.healthPoint, originalHealthPoint);
    }
    
    [UnityTest]
    public IEnumerator FootmanCanSecondaryAttack()
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("GameFeel");
        var footmanAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<FootmanAnimator>();
        
        // Act
        Press(mouse.rightButton);
        yield return new WaitForSeconds(0.6f);
        
        // Assert
        Assert.IsTrue(footmanAnimator.IsInAnimationState("HeavyAttack") || footmanAnimator.IsNextAnimationState("HeavyAttack"));
    }
    
    [UnityTest]
    public IEnumerator FootmanCanSecondaryAttackHitEnemy()
    {
        // Arrange
        yield return SceneManager.LoadSceneAsync("GameFeel");
        var zombie = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Zombie>();
        var originalHealthPoint = zombie.healthPoint;
        
        // Act
        yield return new WaitForSeconds(1.5f);
        Press(mouse.rightButton);
        yield return new WaitForSeconds(1.0f);
        
        // Assert
        Assert.Less(zombie.healthPoint, originalHealthPoint);
    }
}
