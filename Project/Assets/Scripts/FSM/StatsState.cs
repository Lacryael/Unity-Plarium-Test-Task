using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.FSM;

namespace States
{
    public class StatsState : State
    {
        private UiController uiCont;

        public StatsState(FSM fsm, UiController ui) : base(fsm)
        {
            uiCont = ui;
        }

        public override void Enter()
        {
            Time.timeScale = 0;
            uiCont.stats.SetActive(true);
            base.Enter();
        }
        public override void Exit()
        {
            Time.timeScale = 1;
            uiCont.buttonClose.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.stats.SetActive(false);
            base.Exit();
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || uiCont.buttonClose.GetComponent<ButtonCheck>().isClicked == true)
            {
                int nextId = (int)UiController.uiStates.DEFAULT;
                State nextState = m_fsm.GetState(nextId);
                m_fsm.SetCurrentState(nextState);
            }
        }
    }
}