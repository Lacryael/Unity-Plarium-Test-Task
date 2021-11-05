using UnityEngine;
using Assets.Data;
using UnityEngine.UI;
using States;

namespace Assets.Scripts
{
    public class UiController : MonoBehaviour
    {
        public GameObject textHives;
        public GameObject textBees;
        public GameObject textBeesFly;
        public GameObject textFlowers;
        public GameObject textHives2;
        public GameObject textBees2;
        public GameObject textBeesFly2;
        public GameObject textFlowers2;

        public GameObject ministats;

        public GameObject menu;
        public GameObject settings;
        public GameObject stats;
        public GameObject help;

        public GameObject buttonApply;
        public GameObject buttonCancel;
        public GameObject buttonClose;
        public GameObject buttonClose2;

        public GameObject buttonContinue;
        public GameObject buttonSettings;
        public GameObject buttonStats;
        public GameObject buttonExit;
        public GameObject buttonHelp;

        public enum uiStates
        {
            MENUOPEN,
            SETTINGS,
            STATS,
            EXIT,
            HELP,
            DEFAULT
        }
        private FSM.FSM m_fsm = new FSM.FSM();

        void Start()
        {
            m_fsm.Add((int)uiStates.MENUOPEN, new MenuState(m_fsm, this));
            m_fsm.Add((int)uiStates.DEFAULT, new DefaultState(m_fsm, this));
            m_fsm.Add((int)uiStates.SETTINGS, new SettingsState(m_fsm, this));
            m_fsm.Add((int)uiStates.STATS, new StatsState(m_fsm, this));
            m_fsm.Add((int)uiStates.EXIT, new ExitState(m_fsm, this));
            m_fsm.Add((int)uiStates.HELP, new HelpState(m_fsm, this));

            m_fsm.SetCurrentState(m_fsm.GetState((int)uiStates.MENUOPEN));
        }

        void Update()
        {
            if (m_fsm != null)
            {
                m_fsm.Update();
            }

            textHives.GetComponent<Text>().text = $"{GlobalData.allHives}";
            textBees.GetComponent<Text>().text = $"{GlobalData.aliveBees}";
            textBeesFly.GetComponent<Text>().text = $"{GlobalData.flyingBees}";
            textFlowers.GetComponent<Text>().text = $"{GlobalData.aliveFlowers}";

            textHives2.GetComponent<Text>().text = $"{GlobalData.allHives}";
            textBees2.GetComponent<Text>().text = $"{GlobalData.aliveBees}";
            textBeesFly2.GetComponent<Text>().text = $"{GlobalData.flyingBees}";
            textFlowers2.GetComponent<Text>().text = $"{GlobalData.aliveFlowers}";
        }
        public void Exit()
        {
            m_fsm = null;
        }
    }
}
