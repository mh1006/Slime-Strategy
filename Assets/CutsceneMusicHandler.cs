using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimeStrategy
{
    public class CutsceneMusicHandler : MonoBehaviour
    {
        private AudioSource _audioSource;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            SceneManager.activeSceneChanged += SceneChange;
        }

        private void SceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.name != "Menu") return;
            _audioSource.Stop();
            Destroy(gameObject);
        }
    }
}
