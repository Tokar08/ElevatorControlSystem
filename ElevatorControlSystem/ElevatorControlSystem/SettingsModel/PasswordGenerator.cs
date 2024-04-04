using ElevatorControlSystem.ControlsManager;
using System;
using System.Collections.Generic;
using System.Windows;
using static ElevatorControlSystem.MainWindow;

namespace ElevatorControlSystem.SettingsModel
{
    public class PasswordGenerator
    {
        private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";


        private static Dictionary<string, Roles> RoleMap = new Dictionary<string, Roles>
         {
         { "Лаборатория", Roles.Laboratory },
         { "Производственный отдел", Roles.Production },
         { "Бухгалтерия", Roles.Accounting },
         { "Отдел кадров", Roles.HR },
         };
        private static string GetPostfixForRole(Roles role)
        {
            switch (role)
            {
                case Roles.Laboratory:
                    return "_Lab";
                case Roles.Production:
                    return "_Prod";
                case Roles.Accounting:
                    return "_Acc";
                case Roles.HR:
                    return "_HR";
                case Roles.Director:
                    return "_Dir";
                default:
                    return "_ERROR! ( : ౦ ‸ ౦ : )";
            }
        }

        public Roles ParseRole(string roleText)
        {
            try
            {
                if (RoleMap.TryGetValue(roleText, out Roles role))
                {
                    return role;
                }
                else
                {
                     NotificationManager.ShowErrorMessageBox("Выберите роль для генерации соответствующего пароля!");
                    return Roles.Def;
                }
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox("Произошла ошибка: " + ex.Message);
                throw;
            }
        }
        public string GeneratePassword(Roles role)
        {
            string postfix = GetPostfixForRole(role);
            char[] password = new char[7 + postfix.Length];

            for (int i = 0; i < 7; i++)
            {
                password[i] = AllowedChars[new Random().Next(AllowedChars.Length)];
            }

            for (int i = 0; i < postfix.Length; i++)
            {
                password[7 + i] = postfix[i];
            }

            return new string(password);

        }


    }
}
