using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.FSM;

namespace States
{
    public class ExitState : State
    {
        private UiController uiCont;

        public ExitState(FSM fsm, UiController ui) : base(fsm)
        {
            uiCont = ui;
        }

        public override void Enter()
        {
            Application.Quit();
            base.Enter();
        }

        public override void Update()
        {
            int nextId = (int)UiController.uiStates.MENUOPEN;
            State nextState = m_fsm.GetState(nextId);
            m_fsm.SetCurrentState(nextState);
        }
    }
}
