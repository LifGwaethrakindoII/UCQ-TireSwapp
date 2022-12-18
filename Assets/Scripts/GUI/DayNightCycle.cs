using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UrielChallenge;
using UnityEngine.UI;

namespace Ulyssess
{
    public class DayNightCycle : MonoBehaviour
    {
        public UrielChallenge.ApplicationData data;
        private Coroutine cycleChange;
        public Text[] texts;

        public Material materialTitleGame;

        public void SetUpDayCycle()
        {
            if(data.dayTime != DayTime.Day)
            this.StartCoroutine(UpdateCycle(0f), ref cycleChange);
        }

        public void SetUpNightCycle()
        {
            if(data.dayTime != DayTime.Night)
            this.StartCoroutine(UpdateCycle(1f), ref cycleChange);
        }

        private IEnumerator UpdateCycle(Vector3 _rotation)
        {
            float n = 0.0f;
            Vector3 initialRotation = transform.eulerAngles;

            while(n<1.0f)
            {
                transform.eulerAngles = Vector3.Lerp(initialRotation, _rotation, n);
                n += Time.deltaTime / data.cycleChangeDuration;
                yield return null;
            }
            
        }

        private IEnumerator UpdateCycle(float _destiny)
        {
            float n = _destiny == 1f ? 0f : 1f;
            Material SkyboxMaterial = RenderSettings.skybox;

            while (_destiny == 1f ? n < 1.0f : n > 0f)
            {
                SkyboxMaterial.SetFloat("_Blend", n);
                if(_destiny == 1f)
                {
                    n += Time.deltaTime / data.cycleChangeDuration;
                }
                else
                {
                    n -= Time.deltaTime / data.cycleChangeDuration;
                }
                
                yield return null;
            }
            SkyboxMaterial.SetFloat("_Blend", _destiny);
            materialTitleGame.SetColor("_Color", Color.Lerp(Color.black, Color.white, n));
            foreach(Text text in texts)
            {
                text.color = Color.Lerp(Color.black, Color.white, n);
            }
        }
    }
}
