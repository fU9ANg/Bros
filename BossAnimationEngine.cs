// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEngine : MonoBehaviour
{
	protected void Awake()
	{
	}

	protected void LateUpdate()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0666f) * this.speedM;
		this.animationIncrement = this.t;
		if (this.animationFaceDelay > 0f)
		{
			this.animationFaceDelay -= this.t;
			if (this.animationFaceDelay <= 0f)
			{
				this.FadeOutAnimations(this.animationFadeTime);
			}
		}
		this.RunAnimations();
	}

	public void StepAnimations()
	{
		this.LateUpdate();
	}

	public void PlayAnimation(AnimationState state)
	{
		if (state == null)
		{
			UnityEngine.Debug.LogError("State is null");
		}
		for (int i = 0; i < this.fadingStates.Count; i++)
		{
			if (this.fadingStates[i].layer == state.layer)
			{
				this.fadingStates[i].weight = 0f;
				this.fadingStates[i].enabled = false;
			}
		}
		foreach (AnimationState animationState in this.currentStates.ToArray())
		{
			if (animationState.layer == state.layer)
			{
				animationState.enabled = false;
				animationState.weight = 0f;
				this.currentStates.Remove(animationState);
			}
		}
		this.currentStates.Add(state);
		state.weight = 1f;
		state.time = 0f;
		state.enabled = true;
		this.finishedState = false;
	}

	public void FadeOutAnimations(float fadeTime)
	{
		foreach (AnimationState animationState in this.currentStates.ToArray())
		{
			if (animationState != null)
			{
				this.fadingStates.Add(animationState);
			}
			this.currentStates.Remove(animationState);
		}
		this.animationFadeTime = fadeTime;
	}

	public void CrossfadeAnimation(AnimationState state, float fadeTime)
	{
		if (state == null)
		{
			UnityEngine.Debug.LogError("State is null");
		}
		foreach (AnimationState x in this.currentStates.ToArray())
		{
			if (x == state)
			{
				return;
			}
		}
		this.animationFadeTime = fadeTime;
		this.animationFaceDelay = 0f;
		float time = 0f;
		float weight = 0f;
		foreach (AnimationState animationState in this.fadingStates.ToArray())
		{
			if (state.name == animationState.name)
			{
				time = animationState.time;
				weight = animationState.weight;
				this.fadingStates.Remove(animationState);
			}
		}
		foreach (AnimationState animationState2 in this.currentStates.ToArray())
		{
			if (animationState2.layer == state.layer)
			{
				this.fadingStates.Add(animationState2);
				this.currentStates.Remove(animationState2);
			}
		}
		this.currentStates.Add(state);
		state.weight = weight;
		state.time = time;
		state.enabled = true;
		this.finishedState = false;
	}

	public void CrossfadeAnimation(AnimationState state, float fadeTime, float fadeDelay)
	{
		this.CrossfadeAnimation(state, fadeTime);
		this.animationFaceDelay = fadeDelay;
	}

	public bool FadeOutAnimation(AnimationState state, float aI)
	{
		state.weight -= aI / this.animationFadeTime / this.speedM;
		state.time += aI * state.speed;
		if (state.weight <= 0f)
		{
			state.enabled = false;
			return true;
		}
		return false;
	}

	public void FadeInAnimation(AnimationState state, float aI)
	{
		state.weight = Mathf.Clamp(state.weight + aI / this.animationFadeTime / this.speedM, 0f, 1f);
		if (state.wrapMode == WrapMode.ClampForever || state.wrapMode == WrapMode.Default)
		{
			state.time = Mathf.Clamp(state.time + aI * state.speed, 0f, state.length);
		}
		else
		{
			state.time += aI * state.speed;
		}
	}

	public void FadeInCurrentStates(float aI)
	{
		foreach (AnimationState state in this.currentStates.ToArray())
		{
			this.FadeInAnimation(state, aI);
		}
	}

	protected void RunAnimations()
	{
		if (this.t > 0f)
		{
			while (this.tempT < this.t)
			{
				this.tempT += this.animationIncrement;
				foreach (AnimationState animationState in this.fadingStates.ToArray())
				{
					if (animationState != null)
					{
						if (this.FadeOutAnimation(animationState, this.animationIncrement))
						{
							this.fadingStates.Remove(animationState);
						}
					}
					else
					{
						this.fadingStates.Remove(animationState);
					}
				}
				this.FadeInCurrentStates(this.animationIncrement);
			}
			this.tempT -= this.t;
		}
	}

	protected float animationCounter;

	[HideInInspector]
	public float speedM = 1f;

	protected float tempT;

	protected float t;

	protected float facingAngle;

	[HideInInspector]
	protected List<AnimationState> currentStates = new List<AnimationState>();

	protected List<AnimationState> fadingStates = new List<AnimationState>();

	protected float animationFadeTime = 0.15f;

	protected float animationFaceDelay;

	protected float animationIncrement = 0.033f;

	protected float currentStateTime;

	protected float currentStateTimeTo;

	public string currentStateName = string.Empty;

	public float currentStateTimeCount;

	public float currentStateWeightCount;

	public bool finishedState;
}
