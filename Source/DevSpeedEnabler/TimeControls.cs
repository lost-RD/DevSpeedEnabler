using System;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace DevSpeedEnabler
{
	public static class _TimeControls
	{
		public static readonly Vector2 TimeButSize = new Vector2(32f, 24f);

		private static readonly string[] SpeedSounds = new string[]
		{
			"ClockStop",
			"ClockNormal",
			"ClockFast",
			"ClockSuperfast",
			"ClockSuperfast"
		};

		private static readonly TimeSpeed[] CachedTimeSpeedValues = (TimeSpeed[])Enum.GetValues(typeof(TimeSpeed));

		private static void PlaySoundOf(TimeSpeed speed)
		{
			SoundDef.Named(_TimeControls.SpeedSounds[(int)speed]).PlayOneShotOnCamera();
		}

        public static void _DoTimeControlsGUI(Rect timerRect)
		{
			TickManager tickManager = Find.TickManager;
			GUI.BeginGroup(timerRect);
			Rect rect = new Rect(0f, 0f, _TimeControls.TimeButSize.x, _TimeControls.TimeButSize.y);
			for (int i = 0; i < _TimeControls.CachedTimeSpeedValues.Length; i++)
			{
				TimeSpeed timeSpeed = _TimeControls.CachedTimeSpeedValues[i];
				if (timeSpeed != TimeSpeed.Ultrafast)
				{
					if (Widgets.ButtonImage(rect, _TexButton._SpeedButtonTextures[(int)timeSpeed]))
					{
						if (timeSpeed == TimeSpeed.Paused)
						{
							tickManager.TogglePaused();
						}
						else
						{
							tickManager.CurTimeSpeed = timeSpeed;
						}
						_TimeControls.PlaySoundOf(tickManager.CurTimeSpeed);
						ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
					}
					if (tickManager.CurTimeSpeed == timeSpeed)
					{
						GUI.DrawTexture(rect, TexUI.HighlightTex);
					}
					rect.x += rect.width;
				}
			}
			if (Find.TickManager.slower.ForcedNormalSpeed)
			{
				Widgets.DrawLineHorizontal(rect.width * 2f, rect.height / 2f, rect.width * 2f);
			}
			GUI.EndGroup();
			GenUI.AbsorbClicksInRect(timerRect);
			TutorUIHighlighter.HighlightOpportunity("TimeControls", timerRect);
			if (Event.current.type == EventType.KeyDown)
			{
				if (KeyBindingDefOf.TogglePause.KeyDownEvent)
				{
					Find.TickManager.TogglePaused();
					_TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
					ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.SpacePause, KnowledgeAmount.Total);
					Event.current.Use();
				}
				if (KeyBindingDefOf.TimeSpeedNormal.KeyDownEvent)
				{
					Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
					_TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
					ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
					Event.current.Use();
				}
				if (KeyBindingDefOf.TimeSpeedFast.KeyDownEvent)
				{
					Find.TickManager.CurTimeSpeed = TimeSpeed.Fast;
					_TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
					ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
					Event.current.Use();
				}
				if (KeyBindingDefOf.TimeSpeedSuperfast.KeyDownEvent)
				{
					Find.TickManager.CurTimeSpeed = TimeSpeed.Superfast;
					_TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
					ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
					Event.current.Use();
				}
                if (KeyBindingDefOf.TimeSpeedUltrafast.KeyDownEvent)
                {
                    Find.TickManager.CurTimeSpeed = TimeSpeed.Ultrafast;
                    _TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
                    Event.current.Use();
                }
                if (Prefs.DevMode)
				{
					if (KeyBindingDefOf.DebugTickOnce.KeyDownEvent && tickManager.CurTimeSpeed == TimeSpeed.Paused)
					{
						tickManager.DoSingleTick();
						SoundDef.Named(_TimeControls.SpeedSounds[0]).PlayOneShotOnCamera();
					}
				}
			}
		}
	}
}
