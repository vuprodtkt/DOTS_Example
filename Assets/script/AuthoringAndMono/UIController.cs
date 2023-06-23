using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script.AuthoringAndMono
{
    public class UIController : MonoBehaviour
    {
        public Canvas m_canvas;
        public Text m_text_score;
        public Text m_text_level;

        // Use this for initialization
        void Start()
        {
            SetupExampleUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetupExampleUI()
        {
            m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
            m_canvas.pixelPerfect = true;
            m_canvas.scaleFactor = 1.0f;

            m_text_score.text = "Score: " + 0;
            m_text_level.text = "Level: " + 1;

        }
    }
}