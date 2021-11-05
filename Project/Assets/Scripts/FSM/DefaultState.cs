using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.FSM;

namespace States
{
    public class DefaultState : State
    {
        private UiController uiCont;

        public DefaultState(FSM fsm, UiController ui) : base(fsm)
        {
            uiCont = ui;
        }

        public override void Enter()
        {
            uiCont.ministats.SetActive(true);
            Time.timeScale = 1;
            uiCont.menu.SetActive(false);
            base.Enter();
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                int nextId = (int)UiController.uiStates.MENUOPEN;
                State nextState = m_fsm.GetState(nextId);
                m_fsm.SetCurrentState(nextState);
            }
        }
    }
}