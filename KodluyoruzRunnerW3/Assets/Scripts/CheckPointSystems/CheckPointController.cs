using UnityEngine;

namespace CheckPointSystems
{
    public class CheckPointController : MonoBehaviour
    {
        //uygun durumda mangera haber verecek

        public CheckPointManager CheckPointManager;
        [SerializeField] private CheckPointData _checkPointData;
        [SerializeField] private Material[] _checkPointMaterials;
        public bool isMyTurn;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (isMyTurn)
                {
                    isMyTurn = false;
                    _checkPointData.isPassed = true;
                    ChangeColor();
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (_checkPointData.isPassed)
                {
                    CheckPointManager.SetLastPassedCheckPoint(_checkPointData.checkPointID);
                }
            }
        }
        public void ResetCheckPoint()
        {
            _checkPointData.isPassed = false;
            ChangeColor();
        }
        private void ChangeColor()
        {
            _checkPointData.checkPointRenderer.material = _checkPointMaterials[_checkPointData.isPassed ? 1 : 0];
        }
    }
    [System.Serializable]
    public class CheckPointData
    {
        public int checkPointID;
        public bool isPassed;
        public Renderer checkPointRenderer;
    }
}