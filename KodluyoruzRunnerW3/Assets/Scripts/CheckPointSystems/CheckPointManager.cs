//#define LAP
using System.Collections.Generic;
using UnityEngine;

namespace CheckPointSystems
{
    public class CheckPointManager : MonoBehaviour
    {
#if LAP
    private const int TOTAL_LAP = 3;
    private int _lap = 0;
#endif

        [SerializeField] private List<CheckPointController> checkPoints = new List<CheckPointController>();
        private int _lastPassedCheckPoint;
        private void Start()
        {
            for (int i = 0; i < checkPoints.Count; i++)
            {
                checkPoints[i].CheckPointManager = this;
                if (i == 0) checkPoints[i].isMyTurn = true;
            }
        }
        public void SetLastPassedCheckPoint(int id)
        {
            _lastPassedCheckPoint = id;
            GameManager.Instance().ChangeCheckPoint(_lastPassedCheckPoint);
            if (checkPoints.Count - 1 > id)
            {
                checkPoints[id + 1].isMyTurn = true;
            }
            else
            {
#if LAP
            if(_lap < TOTAL_LAP)
            {
                ResetLap();
            }
            else
            {
                EndGame();
            }
#else
                EndGame();
#endif
            }
        }
#if LAP
    private void ResetLap()
    {
        _lap++;
        for(int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].ResetCheckPoint();
            if (i == 0)
            {
                 checkPoints[i].isMyTurn = true;
            }
            else
            {
                checkPoints[i].isMyTurn = false;
            }
        }
    }
#endif
        public void EndGame()
        {
            Debug.Log("Level Completed");
        }
    }
}