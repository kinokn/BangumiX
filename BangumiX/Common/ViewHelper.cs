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
using Windows.UI.Xaml.Controls;
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

    public static class ExceptionDialog
    {
        public static async Task DisplayNoNetworkDialog()
        {
            ContentDialog noNetworkDialog = new ContentDialog
            {
                Title = "网络连接异常",
                Content = "请检查网络连接后重试。",
                CloseButtonText = "好的"
            };

            ContentDialogResult result = await noNetworkDialog.ShowAsync();
        }

        public static async Task DisplayNoAuthDialog()
        {
            ContentDialog noAuthDialog = new ContentDialog
            {
                Title = "未登录",
                Content = "请先登录后重试。",
                CloseButtonText = "好的"
            };

            ContentDialogResult result = await noAuthDialog.ShowAsync();
        }

        public static async Task DisplayNoCollectDialog()
        {
            ContentDialog noAuthDialog = new ContentDialog
            {
                Title = "错误",
                Content = "未收藏该条目。",
                CloseButtonText = "好的"
            };

            ContentDialogResult result = await noAuthDialog.ShowAsync();
        }
    }

    public class ObservableViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
