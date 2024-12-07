using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Basketball : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] Transform _ball;
    [SerializeField] Transform _arms;
    [SerializeField] Transform _posOverhead;
    [SerializeField] Transform _posDribble;
    [SerializeField] Transform _target;

    private bool _isBallInHands = true;
    private bool _isBallFlying = false;
    private float _t = 0;

    private Vector3 randomVector;

    // Update is called once per frame
    private void Start()
    {
        randomVector = new Vector3(Random.Range(0, 3), Random.Range(0, 2), Random.Range(0, 2));

    }

    void Update()
    {
        //Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = new Vector3(0, 0, 0);
        bool isGetKey = false;

        if (Input.GetKey(KeyCode.W))
        {
            direction = new Vector3(0, 0, 1);
            isGetKey = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = new Vector3(0, 0, -1);
            isGetKey = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = new Vector3(1, 0, 0);
            isGetKey = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction = new Vector3(-1, 0, 0);
            isGetKey = true;
        }

        if (isGetKey)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        transform.position += direction * _speed * Time.deltaTime;
        transform.LookAt(transform.position + direction);

        if (_isBallInHands)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _ball.position = _posOverhead.position;
                _arms.localEulerAngles = Vector3.right * -25; // 180

                //transform.LookAt(_target.position);
            }
            else
            {
                _ball.position = _posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                _arms.localEulerAngles = Vector3.right * 0;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _isBallInHands = false;
                _isBallFlying = true;
                _t = 0;
            }
        }

        if (_isBallFlying)
        {
            _t += Time.deltaTime;
            float duration = 0.5f;
            float t01 = _t / duration;

            Vector3 a = _posOverhead.position;
            //Vector3 b = _target.position;
            Vector3 b = _target.position + randomVector;
            Vector3 pos = Vector3.Lerp(a, b, t01);

            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            _ball.position = pos + arc;

            if (t01 >= 1)
            {
                _isBallFlying = false;
                _ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            if (!_isBallInHands && !_isBallFlying)
            {
                randomVector = new Vector3(Random.Range(0, 3), Random.Range(0, 2), Random.Range(0, 2));
                _isBallInHands = true;
                _ball.GetComponent<Rigidbody>().isKinematic = true;
                Debug.Log("Balllllll");
            }
        }
    }
}
