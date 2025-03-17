using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the transitions between scenes or otherwise.
/// </summary>
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }
    public Animator transitionAnimator;

    event Action _OnTransitionEnd;
    public event Action OnTransitionEnd
    {
        add
        {
            _OnTransitionEnd += value;
        }
        remove
        {
            _OnTransitionEnd -= value;
        }
    }

    // Queue of tasks to be performed after the start transition finishes and before the end animation begins (e.g level loading)
    Queue<Func<IEnumerator>> taskQueue = new Queue<Func<IEnumerator>>();

    void Awake()
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

    /// <summary>
    /// Adds a task to the task queue to be performed during the transition.
    /// </summary>
    /// <param name="task"></param>
    public static void AddTask(Func<IEnumerator> task)
    {
        Instance.taskQueue.Enqueue(task);
        Debug.Log("Added task " + task.Method.Name);
    }

    // Performs the entire transition, ran by other scripts
    public static void StartTransition()
    {
        Instance.StartCoroutine(Instance.HandleTransition());
    }

    // Coroutine to perform transition start, tasks, transition end, and notify objects
    IEnumerator HandleTransition(string transitionName = "RectWipe")
    {
        string start = $"{transitionName}_Start";
        string end = $"{transitionName}_End";

        // Perform the start transition
        transitionAnimator.SetTrigger(start);
        yield return new WaitForSeconds(GetAnimationClipLength(start));

        while (taskQueue.Count > 0)
        {
            var task = taskQueue.Dequeue();
            Debug.Log("Performing task " + task.Method.Name);
            yield return task();
            // Could later be done in parallel to improve efficiency if needed, but will cause issues with synchronization so don't bother until necessary
        }

        // Perform the end transition
        transitionAnimator.SetTrigger(end);
        yield return new WaitForSeconds(GetAnimationClipLength(end));

        // Notify objects that the transition has ended
        // E.g to start panning the camera, begin accepting player input, etc.
        _OnTransitionEnd?.Invoke();
    }

    float GetAnimationClipLength(string clipName) {
        // I dont know if this is sketchy or not
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
