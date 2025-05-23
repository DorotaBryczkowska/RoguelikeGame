﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    public int sceneBuildIndex;
    public Animator transition;

    private readonly float transitionTime=1f;
    private bool isPlayerInRange = false;
    
    [SerializeField] 
    private AbstractDungeonGenerator generator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(MoveLevel());
        }
    }

    IEnumerator MoveLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        if (sceneBuildIndex == 0)
        {
            generator.GenerateDungeon();
            transition.SetTrigger("End");
        }
        else
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == sceneBuildIndex)
        {
            generator = FindObjectOfType<AbstractDungeonGenerator>();
            if (generator != null)
            {
                generator.GenerateDungeon();
            }
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
