using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace _196723
{
    public class StatsEffect : MonoBehaviour
    {
        private StatType type;
        private float speed;
        private float duration;
        private StatsContainer container;

        private Timer timer;

        private static List<StatsEffect> playerEffects = new List<StatsEffect>();

        public static StatsEffect AddEffect(GameObject target, StatType type, float speed, float duration)
        {
            if (target.GetComponent<StatsContainer>() != null)
            {
                GameObject effectObject = new GameObject();
                effectObject.name = "Stat Effect";
                effectObject.transform.parent = target.transform;
                effectObject.AddComponent<StatsEffect>();
                effectObject.AddComponent<Timer>();
                effectObject.GetComponent<StatsEffect>().StartEffect(target, type, speed, duration);
                if (target.CompareTag("Player"))
                {
                    playerEffects.Add(effectObject.GetComponent<StatsEffect>());
                }
                return effectObject.GetComponent<StatsEffect>();
            }
            return null;
        }

        public void StartEffect(GameObject target, StatType type, float speed, float duration)
        {
            this.type = type;
            this.speed = speed;
            this.duration = duration;
            container = target.GetComponent<StatsContainer>();
            timer = GetComponent<Timer>();
            timer.SetTimer(duration);
            timer.OnValueChanged().AddListener(OnTimerChange);
            timer.OnEnd().AddListener(OnTimerEnd);
            timer.Activate();
        }

        public void StopEffect()
        {
            OnTimerEnd();
        }

        private void OnTimerChange()
        {
            if (type == StatType.MANA)
            {
                container.ChangeMana(speed * timer.GetDelta());
            }
            else if (type == StatType.HEALTH)
            {
                if (container.Health <= 0.0f)
                {
                    StopEffect();
                }
                container.ChangeHP(speed * timer.GetDelta());
            }
        }

        private void OnTimerEnd()
        {
            timer.Remove();
            Destroy(gameObject);
        }

        public static void ClearPlayerEffects()
        {
            for (int i = playerEffects.Count - 1; i >= 0; i--)
            {
                if (playerEffects[i] != null)
                {
                    playerEffects[i].StopEffect();
                }
                playerEffects.RemoveAt(i);
            }
        }

    }
}
