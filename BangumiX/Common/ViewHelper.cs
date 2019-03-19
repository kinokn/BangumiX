using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace BangumiX.Common
{
    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LengthVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return (value is int && (int)value == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return (value is Visibility && (Visibility)value == Visibility.Visible) ? 1 : 0;
        }
    }

    //public class RelayCommand : ICommand
    //{
    //    protected readonly Action<object> _execute;
    //    protected readonly Predicate<object> _can_execute;

    //    public RelayCommand() { }

    //    public RelayCommand(Action<object> execute) : this(execute, null) { }

    //    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    //    {
    //        _execute = execute;
    //        _can_execute = canExecute;
    //    }

    //    public event EventHandler CanExecuteChanged;

    //    public bool CanExecute(object parameter)
    //    {
    //        return _can_execute(parameter);
    //    }

    //    public void Execute(object parameter)
    //    {
    //        _execute(parameter);
    //    }
    //}

    public class ObservableViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
