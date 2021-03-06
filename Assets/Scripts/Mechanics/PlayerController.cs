using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Factory;
using Mechanics;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject, IPlayerController
    {
        [SerializeField] private FacadeInput inputCustom;
        [SerializeField] private GameObject pointToReference;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        [SerializeField] private PjFactory _pjFactory;
        [SerializeField] private string nameOfFirstPj;
        [SerializeField] private List<string> listOfAliens;
        [SerializeField] private float timeToWaitAfterReturnToBen;
        [SerializeField] private Image imageToTransformInAlien; 
        private Pj pj;
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        private bool countRevertTimeTOReturnToBen;
        private float timeToTransformInAlien;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled;

        bool jump;
        Vector2 move;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            controlEnabled = true;
            _pjFactory.Configure();
            CreatePj(nameOfFirstPj);
        }

        private void ConfigurePj()
        {
            inputCustom.Configure(pj);
            pj.transform.SetParent(transform);
            animator = pj.GetAnimator();
            spriteRenderer = pj.GetComponent<SpriteRenderer>();
            inputCustom.OnTransform += OnTransform;
            pj.transform.localPosition = pointToReference.transform.localPosition;
            pj.ConfigurePj(this);
        }

        private void OnTransform()
        {
            //Here get a random name of alien and constructing
            var random = Random.Range(0, listOfAliens.Count);
            CreatePj(listOfAliens[random]);
            countRevertTimeTOReturnToBen = true;
        }

        private void CreatePj(string nameOfPj)
        {
            inputCustom.OnTransform -= OnTransform;
            inputCustom.DestroyPj(pj);
            pj = _pjFactory.SpawnPj(nameOfPj);
            ConfigurePj();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = inputCustom.GetHorizontal();
                if (jumpState == JumpState.Grounded && inputCustom.IsJumpPress() && !inputCustom.IsAction())
                    jumpState = JumpState.PrepareToJump;
                else if (inputCustom.IsJumpPress())
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }
            if(countRevertTimeTOReturnToBen)timeToTransformInAlien += Time.deltaTime;
            if (timeToTransformInAlien > timeToWaitAfterReturnToBen)
            {
                timeToTransformInAlien = 0;
                CreatePj(nameOfFirstPj);
                countRevertTimeTOReturnToBen = false;
            }
            UpdatingImageToCanTranform();
            UpdateJumpState();
            base.Update();
        }

        private void UpdatingImageToCanTranform()
        {
            imageToTransformInAlien.fillAmount = inputCustom.GetPorcentToTransformInAlien();
        }

        void UpdateJumpState()
        {
            jump = false;

            
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        animator.SetBool("jump", true);
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    animator.SetBool("jump", false);
                    if (velocity.y < 0)
                    {
                        animator.SetBool("jump_down", true);
                    }
                    if (IsGrounded)
                    {
                        animator.SetBool("inGround", true);
                        animator.SetBool("jump_down", false);
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                        inputCustom.FinishPunch();
                    }
                    else
                    {
                        animator.SetBool("inGround", false);
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
                case JumpState.Grounded:
                    
                    if (!IsGrounded)
                    {
                        animator.SetBool("inGround", false);
                        jumpState = JumpState.InFlight;
                    }
                    else
                    {
                        animator.SetBool("punch", inputCustom.IsAction());
                        if (inputCustom.IsAction())
                        {
                            move.x = 0;
                            pj.Action();
                        }
                    }
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;
            
            Debug.Log($"{move.x} {spriteRenderer.flipX} {spriteRenderer.gameObject.name}");

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            if (inputCustom.IsAction() && IsGrounded)
            {
                targetVelocity.x = 0;
            }
            else
            {
                targetVelocity = move * maxSpeed;   
            }  
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        public void PlayerWin()
        {
            animator.SetTrigger("victory");
            controlEnabled = false;
            StartCoroutine(ReturnToStart());
            
        }

        private IEnumerator ReturnToStart()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(1);
        }

        public void MoveForward(float smoothFactor)
        {
            Debug.Log($"spriteRenderer.flipX {spriteRenderer.flipX}");
            transform.position = Vector2.Lerp(transform.position, transform.position + (spriteRenderer.flipX ? Vector3.left : Vector3.right), smoothFactor);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Finish"))
            {
                PlayerWin();
            }
        }
    }
}