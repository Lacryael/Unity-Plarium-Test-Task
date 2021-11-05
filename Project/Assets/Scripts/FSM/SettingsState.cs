using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.FSM;

namespace States
{
    public class SettingsState : State
    {
        private UiController uiCont;

        public SettingsState(FSM fsm, UiController ui) : base(fsm)
        {
            uiCont = ui;
        }

        public override void Enter()
        {
            Time.timeScale = 0;
            uiCont.settings.SetActive(true);
            base.Enter();
        }
        public override void Exit()
        {
            Time.timeScale = 1;
            uiCont.settings.SetActive(false);
            uiCont.buttonCancel.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.buttonApply.GetComponent<ButtonCheck>().isClicked = false;

            base.Exit();
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || uiCont.buttonCancel.GetComponent<ButtonCheck>().isClicked == true || uiCont.buttonApply.GetComponent<ButtonCheck>().isClicked == true)
            {
                int nextId = (int)UiController.uiStates.DEFAULT;
                State nextState = m_fsm.GetState(nextId);
                m_fsm.SetCurrentState(nextState);
            }
        }
    }
}
