using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.FSM;

namespace States
{
    public class MenuState : State
    {
        private UiController uiCont;

        public MenuState(FSM fsm, UiController ui) : base(fsm)
        {
            uiCont = ui;
        }

        public override void Enter()
        {
            Time.timeScale = 0;
            uiCont.ministats.SetActive(false);
            uiCont.menu.SetActive(true);
            uiCont.stats.SetActive(false);
            uiCont.help.SetActive(false);
            uiCont.settings.SetActive(false);
            base.Enter();
        }
        public override void Exit()
        {
            Time.timeScale = 1;
            uiCont.buttonContinue.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.buttonExit.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.buttonSettings.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.buttonApply.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.buttonClose2.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.buttonClose.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.buttonCancel.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.buttonHelp.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.buttonStats.GetComponent<ButtonCheck>().isClicked = false;
            uiCont.ministats.SetActive(true);
            uiCont.menu.SetActive(false);
            base.Exit();
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || uiCont.buttonContinue.GetComponent<ButtonCheck>().isClicked == true)
            {
                int nextId = (int)UiController.uiStates.DEFAULT;
                State nextState = m_fsm.GetState(nextId);
                m_fsm.SetCurrentState(nextState);
            }
            if (uiCont.buttonSettings.GetComponent<ButtonCheck>().isClicked == true)
            {
                int nextId = (int)UiController.uiStates.SETTINGS;
                State nextState = m_fsm.GetState(nextId);
                m_fsm.SetCurrentState(nextState);
            }
            if (uiCont.buttonExit.GetComponent<ButtonCheck>().isClicked == true)
            {
                int nextId = (int)UiController.uiStates.EXIT;
                State nextState = m_fsm.GetState(nextId);
                m_fsm.SetCurrentState(nextState);
            }
            if (uiCont.buttonStats.GetComponent<ButtonCheck>().isClicked == true)
            {
                int nextId = (int)UiController.uiStates.STATS;
                State nextState = m_fsm.GetState(nextId);
                m_fsm.SetCurrentState(nextState);
            }
            if (uiCont.buttonHelp.GetComponent<ButtonCheck>().isClicked == true)
            {
                int nextId = (int)UiController.uiStates.HELP;
                State nextState = m_fsm.GetState(nextId);
                m_fsm.SetCurrentState(nextState);
            }
        }
    }
}