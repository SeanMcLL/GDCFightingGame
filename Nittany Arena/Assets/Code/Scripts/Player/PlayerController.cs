using Prime31;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Profiling;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D), typeof(PlayerData))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Fall,
        Walk,
        Jump,
        Knockback,
    }

    private delegate void OnEnterFunction();
    private delegate PlayerState OnUpdateFunction();
    private delegate void OnExitFunction();

    private OnEnterFunction[] m_OnEnterFunctions;
    private OnUpdateFunction[] m_OnUpdateFunctions;
    private OnExitFunction[] m_OnExitFunctions;

    private PlayerState m_CurrentState;

    private CharacterController2D m_Controller;
    private PlayerData m_Data;

    public Vector2 CurrentKnockbackSpeed = Vector2.zero;

    public PlayerState GetCurrentState()
    {
        return m_CurrentState;
    }

    private void Awake()
    {
        m_Controller = GetComponent<CharacterController2D>();
        m_Data = GetComponent<PlayerData>();

        m_OnEnterFunctions = new OnEnterFunction[]
        {
            OnEnterIdle,
            OnEnterFall,
            OnEnterWalk,
            OnEnterJump,
            OnEnterKnockback,
        };

        m_OnUpdateFunctions = new OnUpdateFunction[]
        {
            OnUpdateIdle,
            OnUpdateFall,
            OnUpdateWalk,
            OnUpdateJump,
            OnUpdateKnockback,
        };

        m_OnExitFunctions = new OnExitFunction[]
        {
            OnExitIdle,
            OnExitFall,
            OnExitWalk,
            OnExitJump,
            OnExitKnockback,
        };

        m_CurrentState = PlayerState.Idle;
        m_OnEnterFunctions[(int)m_CurrentState].Invoke();
    }

    private void FixedUpdate()
    {
        bool jump = m_Data.PlayerControlsData.CurrentFrame.Jump, jumpLast = m_Data.PlayerControlsData.PreviousFrame.Jump;

        PlayerState requestedState = m_OnUpdateFunctions[(int)m_CurrentState].Invoke();

        if (requestedState != m_CurrentState)
            OnStateChange(requestedState);
    }

    public void OnStateChange(PlayerState newState)
    {
        m_OnExitFunctions[(int)m_CurrentState].Invoke();
        m_CurrentState = newState;
        m_OnEnterFunctions[(int)m_CurrentState].Invoke();
    }

    private void OnEnterIdle()
    {

    }

    private PlayerState OnUpdateIdle()
    {
        if (!m_Controller.isGrounded)
            return PlayerState.Fall;

        float xInput = m_Data.PlayerControlsData.CurrentFrame.Movement.x;

        bool jump = m_Data.PlayerControlsData.CurrentFrame.Jump, jumpLast = m_Data.PlayerControlsData.PreviousFrame.Jump;

        if (jump && !jumpLast)
            return PlayerState.Jump;

        if (xInput != 0.0f)
            return PlayerState.Walk;

        return PlayerState.Idle;
    }

    private void OnExitIdle()
    {

    }

    private void OnEnterFall()
    {

    }

    private PlayerState OnUpdateFall()
    {
        Vector2 velocity = m_Controller.velocity;

        if (m_Controller.isGrounded)
        {
            if (velocity.x == 0.0f)
                return PlayerState.Idle;

            return PlayerState.Walk;
        }

        float gravityScale = m_Data.MovementData.GravityScale;

        velocity.y += Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime;

        m_Controller.move(velocity * Time.fixedDeltaTime);

        return PlayerState.Fall;
    }

    private void OnExitFall()
    {

    }

    private void OnEnterWalk()
    {

    }

    private PlayerState OnUpdateWalk()
    {
        if (!m_Controller.isGrounded)
            return PlayerState.Fall;

        Vector2 velocity = m_Controller.velocity;

        float xInput = m_Data.PlayerControlsData.CurrentFrame.Movement.x;
        float walkSpeed = m_Data.MovementData.WalkSpeed;
        float friction = m_Data.MovementData.Friction;

        float desiredSpeed = xInput * walkSpeed;
        float acceleration = walkSpeed * friction;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredSpeed, acceleration * Time.fixedDeltaTime);
        velocity.y = -1.0f;
        
        m_Controller.move(velocity * Time.fixedDeltaTime);

        bool jump = m_Data.PlayerControlsData.CurrentFrame.Jump, jumpLast = m_Data.PlayerControlsData.PreviousFrame.Jump;

        if (jump && !jumpLast)
            return PlayerState.Jump;

        if (velocity.x == 0.0f)
            return PlayerState.Idle;

        return PlayerState.Walk;
    }

    private void OnExitWalk()
    {

    }

    private void OnEnterJump()
    {
        Vector2 velocity = m_Controller.velocity;
        velocity.y = m_Data.MovementData.AirJumpHeight;

        m_Controller.move(velocity * Time.fixedDeltaTime);
    }

    private PlayerState OnUpdateJump()
    {
        Vector2 velocity = m_Controller.velocity;
        float gravityScale = m_Data.MovementData.GravityScale;

        velocity.y += Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime;

        m_Controller.move(velocity * Time.fixedDeltaTime);

        if (velocity.y < 0.0f)
            return PlayerState.Fall;

        return PlayerState.Jump;
    }

    private void OnExitJump()
    {

    }

    private void OnEnterKnockback()
    {

    }

    private PlayerState OnUpdateKnockback()
    {
        //TODO: STOP USING MAGIC NUMBERS FOR TESTING
        if (CurrentKnockbackSpeed.sqrMagnitude < 0.1f)
            return PlayerState.Fall;

        CurrentKnockbackSpeed.x = Mathf.MoveTowards(CurrentKnockbackSpeed.x, 0.0f, Time.fixedDeltaTime * 5.0f * m_Data.MovementData.GravityScale);

        m_Controller.move(CurrentKnockbackSpeed * Time.fixedDeltaTime);

        CurrentKnockbackSpeed.y += Physics2D.gravity.y * m_Data.MovementData.GravityScale * Time.fixedDeltaTime;

        if (m_Controller.isGrounded)
            return PlayerState.Idle;

        return PlayerState.Knockback;
    }

    private void OnExitKnockback()
    {
        CurrentKnockbackSpeed = Vector2.zero;
    }

    private bool SmashedStickHorizontal()
    {
        float sensitivity = m_Data.ControlLayoutData.StickSmashSensitivity;

        Vector2 oldMovement = m_Data.PlayerControlsData.PreviousFrame.Movement;
        Vector2 movement = m_Data.PlayerControlsData.CurrentFrame.Movement;

        float oldXAbs = Mathf.Abs(oldMovement.x);
        float xAbs = Mathf.Abs(movement.x);

        if ((oldXAbs != 0.0f && xAbs != 0.0f) && Mathf.Sign(oldMovement.x) != Mathf.Sign(movement.x))
            return true;

        float deltaMovement = xAbs - oldXAbs;

        if (deltaMovement < 0)
            return false;

        return deltaMovement > (1.0f - sensitivity);
    }
}
