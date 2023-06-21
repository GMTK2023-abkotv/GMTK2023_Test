using UnityEngine;
using Unity.Mathematics;

[DefaultExecutionOrder(-1)]
public class InputCommandsUpdatingContainer : MonoBehaviour
{
    MoveCommand _moveCommand;

    JumpCommand _jumpCommand;
    DashCommand _dashCommand;

    /// <summary>
    /// TODO: Initialize Input Commands through PlayerPrefs or other method
    /// </summary>
    void Awake()
    {
        _moveCommand = new MoveCommand();
        _moveCommand.TriggeringKeyCodeToLeft = KeyCode.A;
        _moveCommand.TriggeringKeyCodeToRight = KeyCode.D;
        _moveCommand.TriggeringKeyCodeToDown = KeyCode.S;
        _moveCommand.TriggeringKeyCodeToUp = KeyCode.W;

        _jumpCommand = new JumpCommand();
        _jumpCommand.TriggeringKeyCode = KeyCode.Space;

        _dashCommand = new DashCommand();
        _dashCommand.TriggeringKeyCode = KeyCode.Q;

        InputDelegatesContainer.GetMoveCommand  += GetMoveCommand;

        InputDelegatesContainer.GetJumpCommand  += GetJumpCommand;
        InputDelegatesContainer.GetDashCommand  += GetDashCommand;
    }

    void OnDestroy()
    {
        InputDelegatesContainer.GetMoveCommand -= GetMoveCommand;

        InputDelegatesContainer.GetJumpCommand  -= GetJumpCommand;
        InputDelegatesContainer.GetDashCommand  -= GetDashCommand;
    }

    #region MovementCommandsGetterMethods
    MoveCommand GetMoveCommand()
    {
        return _moveCommand;
    }

    JumpCommand GetJumpCommand()
    {
        return _jumpCommand;
    }

    DashCommand GetDashCommand()
    {
        return _dashCommand;
    }
    #endregion//MovementCommandsGetterMethods

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        
        ResetCommands();
        UpdateMoveCommand();

        if (Input.GetKeyDown(_jumpCommand.TriggeringKeyCode))
        {
            _jumpCommand.IsTriggered = true;
        }

        if (Input.GetKeyDown(_dashCommand.TriggeringKeyCode))
        {
            _dashCommand.IsTriggered = true;
        }

        void UpdateMoveCommand()
        {
            if (Input.GetKey(_moveCommand.TriggeringKeyCodeToLeft))// || true)
            {
                _moveCommand.IsTriggered = true;
                _moveCommand.CommandingToLeft = true;
            }
            else if (Input.GetKey(_moveCommand.TriggeringKeyCodeToRight))
            {
                _moveCommand.IsTriggered = true;
                _moveCommand.CommandingToRight = true;
            }

            if (Input.GetKey(_moveCommand.TriggeringKeyCodeToDown))
            {
                _moveCommand.IsTriggered = true;
                _moveCommand.CommandingToDown = true;
            }
            else if (Input.GetKey(_moveCommand.TriggeringKeyCodeToUp))
            {
                _moveCommand.IsTriggered = true;
                _moveCommand.CommandingToUp = true;
            }

            if (_moveCommand.IsTriggered)
            {
                if (_moveCommand.CommandingToUp)
                {
                    _moveCommand.Direction.z = 1;
                }
                if (_moveCommand.CommandingToDown)
                {
                    _moveCommand.Direction.z = -1;
                }
                if (_moveCommand.CommandingToRight)
                {
                    _moveCommand.Direction.x = 1;
                }
                if (_moveCommand.CommandingToLeft)
                {
                    _moveCommand.Direction.x = -1;
                }

                _moveCommand.Direction = math.normalize(_moveCommand.Direction);
            }
            else
            {
                _moveCommand.Direction = float3.zero;
            }
        }
    }

    void ResetCommands()
    { 
        #region MovementCommandsResetting
        _moveCommand.IsTriggered = false;
        _moveCommand.CommandingToLeft = false;
        _moveCommand.CommandingToRight = false;
        _moveCommand.CommandingToDown = false;
        _moveCommand.CommandingToUp = false;

        _jumpCommand.IsTriggered = false;
        _jumpCommand.IsConsumed = false;

        _dashCommand.IsTriggered = false;
        #endregion//MovementCommandsResetting
    }
}