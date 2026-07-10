using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ClearAllNotifications
{
    [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
    internal static class PlaySettingsPatch
    {
        private static void Postfix(WidgetRow row)
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                return;
            }

            LetterStack letterStack = Find.LetterStack;
            int dismissibleCount = CountDismissibleLetters(letterStack);
            if (dismissibleCount == 0)
            {
                return;
            }

            string tooltip = "CAN_ClearNotificationsTooltip".Translate(dismissibleCount);
            if (row.ButtonIcon(TexButton.Delete, tooltip))
            {
                ShowConfirmation(dismissibleCount);
            }
        }

        private static int CountDismissibleLetters(LetterStack letterStack)
        {
            if (letterStack == null)
            {
                return 0;
            }

            int count = 0;
            List<Letter> letters = letterStack.LettersListForReading;
            for (int index = 0; index < letters.Count; index++)
            {
                Letter letter = letters[index];
                if (letter != null && letter.CanDismissWithRightClick)
                {
                    count++;
                }
            }

            return count;
        }

        private static void ShowConfirmation(int dismissibleCount)
        {
            Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                "CAN_ClearNotificationsConfirmation".Translate(dismissibleCount),
                ClearDismissibleLetters,
                destructive: true,
                title: "CAN_ClearNotificationsTitle".Translate()));
        }

        private static void ClearDismissibleLetters()
        {
            LetterStack letterStack = Find.LetterStack;
            if (letterStack == null)
            {
                return;
            }

            List<Letter> snapshot = new List<Letter>(letterStack.LettersListForReading);
            for (int index = 0; index < snapshot.Count; index++)
            {
                Letter letter = snapshot[index];
                if (letter != null && letter.CanDismissWithRightClick)
                {
                    letterStack.RemoveLetter(letter);
                }
            }
        }
    }
}
