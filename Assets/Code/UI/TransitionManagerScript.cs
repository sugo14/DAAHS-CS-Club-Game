using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManagerScript : MonoBehaviour
{
    public static TransitionManagerScript Instance { get; private set; } // Don't allow other scripts to modify this
    public Animator transitionAnimator;
    public event Action OnTransitionEnd;
    // Queue of tasks to be performed after the start transition finishes and before the end animation begins (e.g level loading)
    Queue<Func<IEnumerator>> taskQueue = new Queue<Func<IEnumerator>>();

    private void Awake()
    {
        // Prevent any duplicate SceneTransitioner objects from existing simultaneously
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Queue of tasks to be performed after the start transition finishes and before the end animation begins (e.g level loading)
    public void AddTask(Func<IEnumerator> task)
    {
        taskQueue.Enqueue(task);
    }

    // Performs the entire transition
    public void StartTransition()
    {
        StartCoroutine(HandleTransition());
    }

    private IEnumerator HandleTransition(string transitionName = "RectWipe")
    {
        string start = $"{transitionName}_Start";
        string end = $"{transitionName}_End";

        transitionAnimator.SetTrigger(start);
        yield return new WaitForSeconds(GetAnimationClipLength(start));

        while (taskQueue.Count > 0)
        {
            var task = taskQueue.Dequeue();
            Debug.Log("Performing task " + task.Method.Name);
            yield return task();
            // Could later be done in parallel to improve efficiency if needed, but may cause issues with synchronization so don't bother until necessary
        }

        transitionAnimator.SetTrigger(end);
        yield return new WaitForSeconds(GetAnimationClipLength(end));

        // Notify objects that the transition has ended
        // E.g to start panning the camera, begin accepting player input, etc.
        OnTransitionEnd?.Invoke();
    }

    float GetAnimationClipLength(string clipName) {
        AnimationClip[] clips = transitionAnimator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        return 0f;
    }
}
