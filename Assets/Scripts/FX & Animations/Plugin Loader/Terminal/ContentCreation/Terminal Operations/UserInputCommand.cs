using System;
using System.Collections;
using Story.Terminal.System;
using UnityEngine;

namespace Story.Terminal.ContentCreation.Terminal_Operations
{
    [Serializable]
    public class UserInputCommand : TerminalCommand
    {
        [Header("Input Command")]
        private static int _tryCount = 0;
        [SerializeReference] public StylizedText inputPrompt;
        [SerializeReference] public string secondTryInputPrompt = "Somethings wrong, Try Again: ";
        [SerializeReference] public StylizedText oncePasswordIsEntered;
        [SerializeReference] public string expectedPassword;
        [SerializeReference] public TextColor inputTextColor;
        [Space(20)]
        [SerializeField] public InputValidationRoutine validationRoutine;


        public UserInputCommand(string password = "123456",
            TextColor textColor = default, string prompt = "Enter Password: ")
        {
            inputPrompt = new StylizedText(prompt, TypeSpeed.Normal, TextColor.White, false);
            expectedPassword = password;
            this.inputTextColor = textColor;
        }

        public UserInputCommand(UserInputCommand otherToCopy)
        {
            inputPrompt =new StylizedText(secondTryInputPrompt, TypeSpeed.Normal, TextColor.White, false);
            expectedPassword = otherToCopy.expectedPassword;
            validationRoutine = otherToCopy.validationRoutine;
            inputTextColor = otherToCopy.inputTextColor;
        }

        // private IEnumerator MakeInvalidMassageRoutine(TerminalProgramRunner terminal)
        // {
        //     yield return PauseDuration(.4f);
        //     // yield return new StylizedText(invalidPasswordMessage, TypeSpeed.Fast,
        //         // TextColor.Red, true).Execute(terminal);
        //     yield return new SimpleText(string.Empty).Execute(terminal);
        //     yield return new SimpleText(string.Empty).Execute(terminal);
        //     yield return new UserInputCommand(this).Execute(terminal);
        // }

        private string PasswordPlaceHolder()
        {
            string placeHolder = "[";
            for (int i = 0; i < expectedPassword.Length; i++)
            {
                placeHolder += "_";
            }
            return placeHolder + "]";
        }

        public override IEnumerator Execute(TerminalProgramRunner terminal, TerminalScreen screen)
        {
            ScreenBuffer buffer = screen.CreateNewLine();
           yield return RevealLineCharByChar(buffer, inputPrompt.text, inputPrompt.typeSpeed, inputPrompt.textColor);
           yield return PauseDuration(.1f);
           yield return RevealLineCharByChar(buffer, PasswordPlaceHolder(), inputPrompt.typeSpeed, inputPrompt.textColor);
           
           // terminal.ProgramState.SetListeningState(expectedPassword, validationRoutine, new UserInputCommand(this));
           _tryCount++;
            yield return null;
        }
    }
}