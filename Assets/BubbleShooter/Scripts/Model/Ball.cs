using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Ball : MonoBehaviour {
    public Image sprite;
    public Text number;
    public Color[] clrs = new Color[11];
    int[] scores = { 2, 4 ,8,16,32,64,128,256,512,1024,2048};
    public Animator anim;

    Common.BallColors _color;
    public Common.BallColors GetBallColor(){
        return _color;
    }

    public void SetBallColorAndNumber(){
        int rnd = Random.Range(0,6);
        number.text = scores[rnd].ToString();
        sprite.color = getColorById(System.Convert.ToInt32(number.text));
    }

    public void SetBallColor(int number)
    {
        sprite.color = getColorById(number);
    }

    GridCell _gridPosition;
    public GridCell GetGridPosition(){
        return _gridPosition;
    }

    public int GetNumberBall(Ball ball)
    {
        return System.Convert.ToInt32(ball.number.text);
    }

    public void SetGridPosition(GridCell grid){
        _gridPosition = grid;
    }

    private Rigidbody2D _rigidBody;
    private bool _isMoving = false;
    private BallManager _ballManager;
    private Counter _counter;


    void Awake(){
        _rigidBody = GetComponent<Rigidbody2D>();
        _counter = GetComponent<Counter>();
        anim = GetComponent<Animator>();
    }

    Color getColorById(int i)
    {
        switch (i)
        {
            case 2: return new Color(0.32f, 0.75f, 0.93f);
            case 4: return new Color(0.45f, 0.75f, 0.75f);
            case 8: return new Color(0.67f, 0.79f, 0.51f);
            case 16: return new Color(0.9f, 0.8f, 0.52f);
            case 32: return new Color(0.85f, 0.6f, 0.37f);
            case 64: return new Color(0.87f, 0.45f, 0.38f);
            case 128: return new Color(0.81f, 0.35f, 0.42f);
            case 256: return new Color(0.84f, 0.68f, 0.81f);
            case 512: return new Color(0.52f, 0.43f, 0.66f);
            case 1024: return new Color(0.35f, 0.51f, 0.74f);
            case 2048: return new Color(0.36f, 0.36f, 0.36f);
        }
        return new Color(0.32f, 0.75f, 0.93f);
    }

    void Update(){
        if (_isMoving) transform.eulerAngles = Vector3.zero;
    }

    public void Init(BallManager ballManager){
        _ballManager = ballManager;
    }

    public void FixPosition(){
        _isMoving = false;
        _rigidBody.bodyType = RigidbodyType2D.Static;
        transform.eulerAngles = Vector3.zero;
    }

    public void AssignBulletToGrid(GridCell gridClue){
        _ballManager.AssignBulletToGrid(this, gridClue);
    }

    public void AssignBulletToGrid(Vector3 position){
        _ballManager.AssignBulletToGrid(this, position);
    }

    public void RemoveBall(){
        Destroy(gameObject);
    }

    public void SetGravity(){
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _rigidBody.gravityScale = 100;
    }

    Color getRealColor(Common.BallColors color){
        Color colorResult = Color.white;
        switch (color)
        {
            case Common.BallColors.Blue:
                colorResult = Color.blue;
                break;
            case Common.BallColors.Green:
                colorResult = Color.green;
                break;
            case Common.BallColors.Red:
                colorResult = Color.red;
                break;
            case Common.BallColors.Yellow:
                colorResult = Color.yellow;
                break;
            case Common.BallColors.Pink:
                colorResult = new Color(1, 0.2f, 1);
                break;
        }

        return colorResult;
    }
	
    public void WasShoot(Transform bulletRoot, Vector3 force){
        releaseFromGun(bulletRoot);
        addForce(force);
    }

    void releaseFromGun(Transform bulletRoot){
        transform.parent = bulletRoot;
    }

    void addForce(Vector3 force){
        _rigidBody.AddForce(new Vector2(force.x, force.y), ForceMode2D.Force);
        _isMoving = true;
    }

    public void SetNewLayer(string newLayer){
        gameObject.layer = LayerMask.NameToLayer(newLayer);
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled=false;
        collider.enabled=true;
    }

    public void EffectFallingBall()
    {
        anim.SetBool("playBlast", true);
        SetNewLayer(Common.LAYER_NONE);
        SetGravity();
        _counter.StartTimerUpdatePercentage(2, () =>
            {
                RemoveBall();
            }, null);

    }

    public void EffectExplodeBall()
    {
        SetNewLayer(Common.LAYER_NONE);
        //SetGravity();
        WasShoot(transform.parent,new Vector3(Random.Range(-2000,2000),Random.Range(-2000,2000),0));

        anim.SetBool("playBlast", true);
        
         _counter.StartTimerUpdatePercentage(0.09f, () =>
            {
                RemoveBall();
            }, null);
    }

    public void OnCollisionEnter2D(Collision2D other) {

        if (_isMoving && gameObject.tag.Equals(Common.LAYER_BULLET))
        {
            Debug.Log("Hit " + other.collider.name);
            string nameHit = other.gameObject.tag;
            if (nameHit.Equals(Common.LAYER_BALL) || nameHit.Equals(Common.LAYER_WALL))
            {
                gameObject.tag = Common.LAYER_BALL;
                SetNewLayer(Common.LAYER_BALL);
                FixPosition();

                if (nameHit.Equals(Common.LAYER_BALL))
                {
                    AssignBulletToGrid(other.gameObject.GetComponent<Ball>().GetGridPosition());
                }
                else
                {
                    AssignBulletToGrid(other.transform.localPosition);
                }

                _ballManager.ExplodeSameColorBall(this);
            }

        }






        /*
        if (_isMoving && gameObject.tag.Equals(Common.LAYER_BULLET))
        {
            Debug.Log("Hit " + other.collider.name);
            string nameHit = other.gameObject.tag;
            if(nameHit.Equals(Common.LAYER_BALL) || nameHit.Equals(Common.LAYER_WALL)){
                gameObject.tag = Common.LAYER_BALL;
                SetNewLayer(Common.LAYER_BALL);
                FixPosition();

                if (nameHit.Equals(Common.LAYER_BALL))
                {
                    AssignBulletToGrid(other.gameObject.GetComponent<Ball>().GetGridPosition());
                }
                else
                {
                    AssignBulletToGrid(other.transform.localPosition);
                }

                _ballManager.ExplodeSameColorBall(this);
            }
         
        }*/
    }

}
