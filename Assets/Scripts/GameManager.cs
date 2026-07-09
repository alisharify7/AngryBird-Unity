using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int MaxNumberOfShots = 3;
    private int _usedNumberOfShot = 0;

    private IconHandler _iconHandler;
    [SerializeField] private float _SecondsToWaiteBeforeDeathCheck = 3.0f;
    private List<Piggie> _piggies = new List<Piggie>();
    [SerializeField] private GameObject _restartScreenObject;

    [SerializeField] private SlingShotHandler _slingShothandler;



    public void Awake() //singlton
    {
        if (instance  == null)
        {
            instance = this;
        }
        _iconHandler = GameObject.FindObjectOfType<IconHandler>();

        Piggie[] currentPiggiesInTheGame = FindObjectsOfType<Piggie>(); // find all the piggies from game and return it into array
        for (int i = 0; i < currentPiggiesInTheGame.Length; i++)
        {
            _piggies.Add(currentPiggiesInTheGame[i]); 
        }
    }

    public void UseShot()
    {
        _usedNumberOfShot += 1;
        _iconHandler.MarkBird(_usedNumberOfShot);
        CheckForLastShot();
    }

   public bool HasEnoughShots()
    {
        if(_usedNumberOfShot < MaxNumberOfShots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckForLastShot()
    {
        if (_usedNumberOfShot == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaiteTime());
        }
    }

    private IEnumerator CheckAfterWaiteTime()
    {
        yield return new WaitForSeconds(_SecondsToWaiteBeforeDeathCheck);
        if (_piggies.Count <= 0)
        {
            // win
            WinGame();
        }
        else
        {
            // lose
            RestartGameWorld();
        }

    }


    public void RemovePiggie(Piggie pigge)
    {
        _piggies.Remove(pigge);
        CheckForAllDeadPiggies();
    }
    
    private void CheckForAllDeadPiggies()
    {
        if (_piggies.Count == 0)
        {
            WinGame();
        }
    }

    #region win lose
    private void WinGame()
    {
        Debug.Log("Win Game");
        _restartScreenObject.SetActive(true);
        _slingShothandler.enabled = false;
    }
    public void RestartGameWorld()
    {
        Debug.Log("Lose Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion


}
