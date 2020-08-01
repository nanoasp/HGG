using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IState
{
    void Enter();

    void Execute();
    
    void Exit();
}

public class StateMachine
{
    IState mCurrentState;
    List<IState> allStates = new List<IState>();
    int mCurrentStateEnum = -2;
    
    public void RegisterState(IState newState, int stateEnum = -1)
    {
        allStates.Add(newState);
    }

    public void ChangeState(int stateEnum = -1)
    {
        if (stateEnum == mCurrentStateEnum)
            return;

        if (mCurrentState != null)
            mCurrentState.Exit();

        mCurrentState = allStates[stateEnum];
        mCurrentStateEnum = stateEnum;
        mCurrentState.Enter();
    }

    public void ChangeState(IState newState, int stateEnum = -1)
    {
        if (stateEnum == mCurrentStateEnum)
            return;

        if (mCurrentState != null)
            mCurrentState.Exit();

        mCurrentState = newState;
        mCurrentStateEnum = stateEnum;
        mCurrentState.Enter();
    }

    public void Update()
    {
        if (mCurrentState != null) mCurrentState.Execute();
    }

    public int GetStateNumber()
    {
        return mCurrentStateEnum;
    }
}