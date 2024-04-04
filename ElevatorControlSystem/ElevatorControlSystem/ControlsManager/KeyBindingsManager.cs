using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ElevatorControlSystem.ControlsManager
{


    public class KeyBindingsManager
    {
        private readonly Dictionary<KeyGesture, Action> bindings = new Dictionary<KeyGesture, Action>();
        private readonly Dictionary<KeyGesture, bool> keyStates = new Dictionary<KeyGesture, bool>();

        public void AddBinding(KeyGesture keyGesture, Action action)
        {
            bindings[keyGesture] = action;
            keyStates[keyGesture] = false;
        }

        public void AddBinding(KeyGesture keyGesture, Action firstAction, Action secondAction)
        {
            bindings[keyGesture] = firstAction;
            keyStates[keyGesture] = false;
            AddBinding(keyGesture, secondAction);
        }

        public void RegisterBindings(UIElement element)
        {
            element.PreviewKeyDown += Element_PreviewKeyDown;
            element.PreviewKeyUp += Element_PreviewKeyUp;
        }

        private void Element_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            foreach (var binding in bindings)
            {
                if (binding.Key.Matches(sender, e) && !keyStates[binding.Key])
                {
                    keyStates[binding.Key] = true;
                    binding.Value.Invoke();
                    e.Handled = true;
                    return;
                }
            }
        }

        private void Element_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            foreach (var binding in bindings)
            {
                if (binding.Key.Matches(sender, e))
                {
                    keyStates[binding.Key] = false;
                }
            }
        }
    }

}
