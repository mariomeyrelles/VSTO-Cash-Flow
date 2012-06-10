using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace ModernCashFlow.WpfControls.Util
{
    public class ConfirmBehavior : Behavior<UIElement>
    {

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is ButtonBase)
            {
                ((ButtonBase)AssociatedObject).Click += new RoutedEventHandler(PromptAndExecuteCommand);
            }
            else
            {
                AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(PromptAndExecuteCommand);
            }

        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            ((Button)AssociatedObject).Click -= PromptAndExecuteCommand;
            AssociatedObject.MouseLeftButtonDown -= PromptAndExecuteCommand;
        }

        void PromptAndExecuteCommand(object sender, RoutedEventArgs e)
        {
            if (!IsConfirm || MessageBoxResult.OK == MessageBox.Show(ConfirmMessage, ConfirmCaption, MessageBoxButton.OKCancel))
            {
                if (Command != null)
                {
                    Command.Execute(CommandParameter);
                }
            }
        }



        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ConfirmBehavior), null);
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ConfirmBehavior), null);
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty IsConfirmProperty = DependencyProperty.Register("Confirm", typeof(bool), typeof(ConfirmBehavior), null);
        public bool IsConfirm
        {
            get { return (bool)GetValue(IsConfirmProperty); }
            set { SetValue(IsConfirmProperty, value); }
        }

        public static readonly DependencyProperty ConfirmCaptionProperty = DependencyProperty.Register("ConfirmCaption", typeof(string), typeof(ConfirmBehavior), null);
        public string ConfirmCaption
        {
            get { return (string)GetValue(ConfirmCaptionProperty); }
            set { SetValue(ConfirmCaptionProperty, value); }
        }

        public static readonly DependencyProperty ConfirmMessageProperty = DependencyProperty.Register("ConfirmMessage", typeof(string), typeof(ConfirmBehavior), null);
        public string ConfirmMessage
        {
            get { return (string)GetValue(ConfirmMessageProperty); }
            set { SetValue(ConfirmMessageProperty, value); }
        }


    }
}

