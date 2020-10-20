using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerSettings _playerSettings; // settings of player movement;
        private CharacterController _controller; //this is from unity standard assets;
        private Vector3 _moveDirection;
        private Vector3 _moveRotation;
        private float _currentYRotationValue; //rotation value from mouse; 
        private bool _isJumping;
        public Transform enemy;
        private Animator _anim;
        public bool jump;

        public Text scoreTxt, hScoreTxt;
        public int Score { get; private set; } 
        public int HScore { get; private set; }
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _anim = gameObject.GetComponentInChildren<Animator>();
            LoadHighScore();
        }
        private void LoadHighScore()
        {
            HScore = PlayerPrefs.GetInt("hiscore");
            hScoreTxt.text = HScore.ToString();
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Obstacle")
            {
                MakeScore();
            }
        }

        private void MakeScore()
        {
            Score++;
            scoreTxt.text = Score.ToString();
            if (Score > HScore)
            {
                HScore = Score;
                hScoreTxt.text = HScore.ToString();
                PlayerPrefs.SetInt("hiscore", HScore);
            }
        }

        private void Update()
        {
            
            if (!_isJumping) //eğer zıplamıyorsa ok tuşları tönünde hareket et;
            {
                _anim.SetTrigger("gameStarted");
                _moveDirection = new Vector3(InputManager.Instance().horizontalInput, 0, InputManager.Instance().verticalInput); //horizontal da rotasyon, vertical ile move sağlamak için;
                //mouse ile rotsayon yapmak için;
                //_moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            }
            else // eğer zıplıyorsa y ekseninde jumpForce uygula;
            {
                jump = true;
                _moveDirection = new Vector3(0, _playerSettings.jumpForce, 0);
            }

            //_currentYRotationValue += InputManager.Instance().horizontalInput; //player mouse yerine artık sağ sol ok tuşlarıyla rotasyon yapacak; döner kebap ekseninde dönüş için hareket datasını alıyoruz;
            _moveRotation = new Vector3(0, _currentYRotationValue, 0);
            _moveDirection = Quaternion.Euler(_moveRotation) * _moveDirection; // rotasyon datası ile hareket vektörünü çarptık;

            ////mouse ile karakterin y ekseninde dönmesini sağladık; <3
            //if (Input.GetKey(KeyCode.Mouse0))
            //{
            //    _currentYRotationValue += Input.GetAxis("Mouse X");
            //    _moveRotation = new Vector3(0, _currentYRotationValue, 0);
            //}

            if (_controller.isGrounded && !_isJumping) //eğer player yerde ise ve zıplamıyorsa zıpla;
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _anim.SetTrigger("jump");
                    //_moveDirection.y = _playerSettings.jumpForce;
                    //StartCoroutine(Jump()); //zıplamanın frame süresini artırmak için coroutine kullandık;
                }
            }
            //transform.rotation = Quaternion.Euler(_moveRotation); //rotasyonu _moveRotationa eşitle;
            //Vector3 targetPosition = new Vector3(enemy.position.x, this.transform.position.y, enemy.position.z);
            //transform.LookAt(targetPosition);
            transform.rotation = Quaternion.Euler(_moveRotation);
            if (!_isJumping) //player zıplamıyorsa yer çekimi oluşturduk, çünkü playerin rigidbodysi yok onun yerine standard assetsten gelen character controller var;
            {
                //rigidbody olmadan yer çekimi yapmak;
                _moveDirection.y += _moveDirection.y + Physics.gravity.y * _playerSettings.gravityScale * Time.deltaTime; // y ekseni için gravity;
            }
           
            _controller.Move(_moveDirection * Time.deltaTime * _playerSettings.moveSpeed);
        }
        /// <summary>
        /// Control jump with given time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Jump() //coroutinemiz, zıplamayı jumpTime kadar bekletecek;
        {
            _isJumping = true;
            yield return new WaitForSeconds(_playerSettings.jumpTime);
            _isJumping = false;
        }
    }
    
    //farklı leveller yaptığımızda playersettings değişebilir. Bu struct yapısın bize kolaylık sağlar.
    //birleştirebildiğimiz şeyleri birleştirelim :)
    [System.Serializable]
    public struct PlayerSettings
    {
        public float moveSpeed;
        public float jumpForce;
        public float jumpTime;
        public float gravityScale;
    }
}
