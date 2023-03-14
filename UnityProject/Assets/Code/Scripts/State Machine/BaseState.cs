using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    public StateMachine StateManager;
    public PlayerStateFactory StateFactory;

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}
