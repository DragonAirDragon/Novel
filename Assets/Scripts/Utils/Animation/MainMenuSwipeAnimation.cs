using DG.Tweening;
using UnityEngine;


public enum PositionUISwipe{
    Left,
    Right
}

public class MainMenuSwipeAnimation : MonoBehaviour
{

    public float timeTransition;
    public Vector2 LeftPos;
    public Vector2 RightPos;
    public RectTransform elementMoving;
    public PositionUISwipe currentPositionUISwipe;
    private Sequence sequenceSwipeMenu;


    private void Start() {
        
        sequenceSwipeMenu = DOTween.Sequence().SetAutoKill(false); // Disable auto-killing of the sequence
        sequenceSwipeMenu.Pause(); 
        SwipeDetector.OnSwipe += MovingImageBySwipe;

    }

    private void MovingImageBySwipe(SwipeData data){
       
        sequenceSwipeMenu.Kill(); // Kill the previous sequence to clear any ongoing animations
        sequenceSwipeMenu = DOTween.Sequence();


        if (data.Direction == SwipeDirection.Right){
            if(currentPositionUISwipe==PositionUISwipe.Left) {
                return;
            }
            sequenceSwipeMenu.Append(elementMoving.DOAnchorPos(LeftPos,timeTransition));
            currentPositionUISwipe = PositionUISwipe.Left;
        }  
        if (data.Direction == SwipeDirection.Left){
            if(currentPositionUISwipe==PositionUISwipe.Right) {
                return;
            }
            sequenceSwipeMenu.Append(elementMoving.DOAnchorPos(RightPos,timeTransition));
            currentPositionUISwipe = PositionUISwipe.Right;
        }
         sequenceSwipeMenu.Play();  
    }
}
