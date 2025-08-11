using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MovingObject
{
 
    private Vector2 [] movementSave;
    private bool ReadyToMove;
    [SerializeField] private GameObject objImage = null;
    int step;
    public bool ControlEnable { get; set; } = false;

    [SerializeField]  private Animator mainAnimator = null;
    [SerializeField] private Animator innerAnimator = null;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (!ControlEnable) { return; }

        Vector2 moveinput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        moveinput.Normalize();

        if (moveinput.sqrMagnitude > 0.5)
        {
            if (!isMoving)
            {
                Move(moveinput);
            }

        }

    }

    protected override void PlayMoveSFX()
    {
        AudioManager.Instance.PlaySFX(DataStore.Instance.SFX_PLAYER_MOVE);
    }

    public void Move(Vector2 direction)
    {
        StartCoroutine(MoveInner(direction));
        //StartCoroutine(MoveTweenAnimation());
    }



    private IEnumerator MoveInner(Vector2 direction)
    {

        if (Mathf.Abs(direction.x) < 0.5)
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }

        direction.Normalize();

        PlayAnimation(direction);

        if (Blocked(transform.position, direction, false))
        {
            yield break;
        }
        else
        {
            InnerAnimation(true);
            MovementAnimation(direction);
            while (isMoving)
            {
                yield return null;
            }

            InnerAnimation(false);
            if (GameManager.Instance.CheckVictory())
            {
                ControlEnable = false;
                yield return new WaitForSeconds(0.5f);
                GameManager.Instance.VictoryFunctions();
            }
        }
    }



    private void InnerAnimation(bool isMove)
    {
        innerAnimator.SetBool("Move", isMove);
    }

    private void PlayAnimation(Vector2 direction)
    {
        Debug.Log($"direction = {direction}");

        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0.1)
            {
                mainAnimator.SetTrigger("Right");
            }
            else if(direction.x < -0.1)
            {
                mainAnimator.SetTrigger("Left");
            }
        }
        else
        {
            if (direction.y > 0)
            {
                mainAnimator.SetTrigger("Up");
            }
            else if (direction.y < -0.1)
            {
                mainAnimator.SetTrigger("Down");
            }
        }
    }
}
